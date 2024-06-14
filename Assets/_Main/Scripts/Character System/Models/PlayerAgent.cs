using System;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAgent : BaseAgent
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        private PlayerControls InputControls { get; set; }
        private PlayerAnimatorController PlayerAnimatorController { get; set; }
        
        #region PreConditions
        
        public bool CanMoveAgain => CurrentActionState.HasFlag(CharacterActionType.MoveNextSkill);
        
        public bool CanPerformRollAgain => Command.HasFlag(Command.Roll);

        public bool CanPerformPrepareAttackAgain => Command.HasFlag(Command.PrepareAttack) &&
                                                    CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
                                                    CurrentActionState.HasFlag(CharacterActionType.EndAttack);
        
        public bool CanPerformAttackMoveAgain => CurrentActionState.HasFlag(CharacterActionType.BeginAttackMove) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack);
        
        public bool CanPerformSwapWeaponAgain => Command.HasFlag(Command.SwapWeapon) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndAttackMove);
        #endregion
        
        //  Fields ----------------------------------------
        
        
        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();
                IsPlayer = true;
                InputControls = new PlayerControls();
                Assign();
                PlayerAnimatorController = GetComponent<PlayerAnimatorController>();
                
                SwitchCombatMode();
                SwitchAnimatorMatchWithWeapon();
                
                
            }
        }
        
        public override void DoEnable()
        {
            base.DoEnable();
            InputControls.Enable();
        }

        public override void DoDisable()
        {
            base.DoDisable();
            InputControls.Disable();
        }

        //  Unity Methods   -------------------------------
        public void Awake()
        {
            Initialize();
        }

        public void OnEnable()
        {
            DoEnable();
        }

        public void OnDisable()
        {
            DoDisable();
        }

        //  Other Methods ---------------------------------
        #region Assign

        public override void Assign()
        {
            AssignInputEvents();
            AssignCharacterActionEvents();
        }

        private void AssignInputEvents()
        {
            InputControls.Character.Roll.started += _ => ApplyRollCommand();
            InputControls.Character.Attack.started += _ => ApplyPrepareAttackCommand();
            InputControls.Character.Attack.canceled += _ => CancelPerformPrepareAttackCommand();
            InputControls.Character.Movement.performed += context =>
            {
                ApplyPerformMovementCommand();
                MotorController.MoveInput = context.ReadValue<Vector2>();
            };
            InputControls.Character.Movement.canceled +=
                _ =>
                {
                    MotorController.MoveDirection = Vector3.zero;
                    MotorController.MoveInput = Vector2.zero;
                };
            InputControls.Character.SwapWeapon.started += _ => ApplySwapWeaponCommand();
        }

        private void AssignCharacterActionEvents()
        {
            #region Roll Events
            RegisterActionTrigger(CharacterActionType.BeginRoll, () =>
            {
                CurrentActionState |= CharacterActionType.BeginRoll;
                CurrentActionState &= ~CharacterActionType.EndRoll;
                CurrentActionState &= ~CharacterActionType.MoveNextSkill;
            });
            RegisterActionTrigger(CharacterActionType.EndRoll, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginRoll;
                CurrentActionState |= CharacterActionType.EndRoll;
            });
            #endregion

            #region Prepare Attack
            RegisterActionTrigger(CharacterActionType.BeginPrepareAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginPrepareAttack;
                CurrentActionState &= ~CharacterActionType.EndPrepareAttack;
            });
            RegisterActionTrigger(CharacterActionType.EndPrepareAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginPrepareAttack;
                CurrentActionState |= CharacterActionType.EndPrepareAttack;
            });
            #endregion

            #region Charge full
            RegisterActionTrigger(CharacterActionType.ChargeFull, () =>
            {
                
            });
            #endregion
            
            #region Attack Move
            RegisterActionTrigger(CharacterActionType.BeginAttackMove, () =>
            {
                CurrentActionState |= CharacterActionType.BeginAttackMove;
                CurrentActionState &= ~CharacterActionType.EndAttackMove;
                CurrentActionState &= ~CharacterActionType.MoveNextSkill;

            });
            RegisterActionTrigger(CharacterActionType.EndAttackMove, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginAttackMove; 
                CurrentActionState |= CharacterActionType.EndAttackMove;
            });
            #endregion
            
            #region Attack
            RegisterActionTrigger(CharacterActionType.BeginAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginAttack;
                CurrentActionState &= ~CharacterActionType.EndAttack;
                
            });
            RegisterActionTrigger(CharacterActionType.EndAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginAttack;
                CurrentActionState |= CharacterActionType.EndAttack;
            });
            #endregion

            #region  Move next skill
            RegisterActionTrigger(CharacterActionType.MoveNextSkill, 
                () =>
                {
                    CurrentActionState |= CharacterActionType.MoveNextSkill;
                });
            #endregion
        }
        
        #endregion
        
        private void SwitchAnimatorMatchWithWeapon() =>
            PlayerAnimatorController.SwitchAnimator(CurrentWeapon.weaponType); 

        
        #region Command
        // Command 
        private void ApplyPerformMovementCommand() => Command |= Command.Move;
        private void ApplySwapWeaponCommand() => Command |= Command.SwapWeapon;
        private void ApplyRollCommand()
        {
            if (Command.HasFlag(Command.Roll)) return;
            Command &= 0;
            Command |= Command.Roll;
        }
        
        private void ApplyPrepareAttackCommand()
        {
            if (Command.HasFlag(Command.PrepareAttack)) return;
            Command |= Command.PrepareAttack;
        }

        public void CancelPerformSwapWeaponCommand() => Command &= ~Command.SwapWeapon;
        public void CancelPerformRollCommand() => Command &= ~Command.Roll;
        public void CancelPerformPrepareAttackCommand() => Command &= ~Command.PrepareAttack;
        public void CompletedInterrupt() => Command &= ~Command.Interrupt;
        
        public void ResetState() => CurrentActionState = DefaultActionState;
        #endregion
        
        //  Event Handlers --------------------------------
        
        
    }
}