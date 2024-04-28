using Atomic.Equipment;
using Unity.VisualScripting;
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
        public bool IsMovement => !Mathf.Approximately(MotorController.MoveInput.sqrMagnitude, 0f);
        
        #region PreConditions

        public bool NeedInterruptAction => !CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                           !CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
                                           !CurrentActionState.HasFlag(CharacterActionType.EndAttackMove);
        
        public bool CanMoveAgain => CurrentActionState.HasFlag(CharacterActionType.MoveNextSkill);
        
        public bool CanPerformRollAgain => Command.HasFlag(Command.Roll);

        public bool CanPerformPrepareAttackAgain => Command.HasFlag(Command.PrepareAttack) &&
                                                    CurrentActionState.HasFlag(CharacterActionType.EndAttack);
        
        public bool CanPerformAttackMoveAgain => CurrentActionState.HasFlag(CharacterActionType.BeginAttackMove) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack);

        public bool CanPerformAttackAgain => Command.HasFlag(Command.Attack);
        
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

                WeaponVisualsController.AttachDefaultWeapons();
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
            InputControls.Character.Attack.canceled += _ => ApplyAttackCommand();
            InputControls.Character.Movement.performed += context => MotorController.MoveInput = context.ReadValue<Vector2>();
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
            RegisterActionTrigger(CharacterActionType.Rolling, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginRoll;
                CurrentActionState |= CharacterActionType.Rolling;
            });
            RegisterActionTrigger(CharacterActionType.EndRoll, () =>
            {
                CurrentActionState &= ~CharacterActionType.Rolling;
                CurrentActionState |= CharacterActionType.EndRoll;
            });
            #endregion

            #region Prepare Attack
            RegisterActionTrigger(CharacterActionType.BeginPrepareAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginPrepareAttack;
                CurrentActionState &= ~CharacterActionType.EndPrepareAttack;
            });
            RegisterActionTrigger(CharacterActionType.PreparingAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginPrepareAttack;
                CurrentActionState |= CharacterActionType.PreparingAttack;
            });
            RegisterActionTrigger(CharacterActionType.EndPrepareAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.PreparingAttack;
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

            });
            RegisterActionTrigger(CharacterActionType.AttackMoving, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginAttackMove;
                CurrentActionState |= CharacterActionType.AttackMoving;
            });
            RegisterActionTrigger(CharacterActionType.EndAttackMove, () =>
            {
                CurrentActionState &= ~CharacterActionType.AttackMoving; 
                CurrentActionState |= CharacterActionType.EndAttackMove;
            });
            #endregion
            
            #region Attack
            RegisterActionTrigger(CharacterActionType.BeginAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.EndAttack;
                CurrentActionState |= CharacterActionType.BeginAttack;
                
            });
            RegisterActionTrigger(CharacterActionType.Attacking, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginAttack;
                CurrentActionState |= CharacterActionType.Attacking;
            });
            RegisterActionTrigger(CharacterActionType.EndAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.Attacking;
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

        #region Behaviours
        // Shared
        public void SetForwardDirection() => MotorController.SetForwardDirection();
        public void ApplyDirection() => MotorController.ApplyDirection();

        // Interrupt Behaviour 
        
        // Movement Behaviour
        public void ApplyStop() => MotorController.LocomotionController.ApplyStop();
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public void ApplyMovementAnimation() => AgentAnimatorController.ApplyMovementAnimation();
        public void StopMovementAnimation() => AgentAnimatorController.StopMovementAnimation();

        // Roll Behaviour 
        public void ApplyRoll() => MotorController.RollController.Roll();

        
        /// <summary>
        /// TODO: Fix this
        /// </summary>
        /// <param name="value"></param>
        public void SetWeaponVisible(bool value) {
            
            // MotorController.CombatController.CurrentWeapon.WeaponRoot.SetActive(value);
        }

        // Attack Behaviour
        public void AimTarget() => MotorController.CombatController.AimTarget();
        public void BeginPrepareAttack() => MotorController.CombatController.BeginPrepareAttack();
        public void PreparingAttack() => MotorController.CombatController.PreparingAttack();
        public void EndPrepareAttack() => MotorController.CombatController.EndPrepareAttack();
        
        public void BeginAttack() => MotorController.CombatController.BeginAttack();
        public void Attacking() => MotorController.CombatController.Attacking();
        public void EndAttack() => MotorController.CombatController.EndAttack();

        public void BeginAttackMove() => MotorController.CombatController.BeginAttackMove();
        public void AttackMoving() => MotorController.CombatController.Attacking();
        public void EndAttackMove() => MotorController.CombatController.EndAttackMove();

        public void CustomActionAttack() => MotorController.CombatController.CustomAction();
        public void InterruptAttack() => MotorController.CombatController.InterruptAction();

        // Swap weapon
        public void ActivateOtherWeapon() => WeaponVisualsController.ActivateOtherWeapon();

        public void SwitchCombatMode()
        {
            MotorController.SwitchCombatMode(CurrentCombatMode);
            MotorController.CombatController.CurrentWeapon = CurrentWeapon;
        }

        public void SwitchAnimatorMatchWithWeapon() =>
            AgentAnimatorController.SwitchAnimator(CurrentWeapon.WeaponType); 
        
        #endregion
        
        #region Command
        // Command 
        public void ApplyPerformMovementCommand() => Command |= Command.Move;
        public void ApplySwapWeaponCommand() => Command |= Command.SwapWeapon;
        public void ApplyRollCommand()
        {
            if (Command.HasFlag(Command.Roll)) return;
            Command |= Command.Interrupt;
            Command |= Command.Roll;
        }
        public void ApplyPrepareAttackCommand()
        {
            if (Command.HasFlag(Command.PrepareAttack)) return;
            Command |= Command.PrepareAttack;
        }
        public void ApplyAttackCommand()
        {
            if (!Command.HasFlag(Command.PrepareAttack)) return;
            Command &= ~Command.PrepareAttack;
            Command |= Command.Attack;
        }

        public void CancelPerformSwapWeaponCommand() => Command &= ~Command.SwapWeapon;
        public void CancelPerformRollCommand() => Command &= ~Command.Roll;
        public void CancelPerformPrepareAttackCommand() => Command &= ~Command.PrepareAttack;
        public void CancelPerformAttackCommand() => Command &= ~Command.Attack;
        public void CompletedInterrupt() => Command &= ~Command.Interrupt;

        public void ResetPerformCommandExcept(Command command)
        {
            Command &= 0;
            Command |= command;
        }

        public void ResetState() => CurrentActionState = DefaultActionState;
        #endregion

        //  Event Handlers --------------------------------
    }
}