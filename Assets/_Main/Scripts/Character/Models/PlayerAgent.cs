using System;
using System.Linq;
using Atomic.Character.Module;
using Atomic.Equipment;
using Unity.VisualScripting;
using UnityEngine;

namespace Atomic.Character.Model
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
        public event Action Interrupt;
        
        //  Fields ----------------------------------------
        private AttachWeaponController _currentAttachWeapon;

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();
                InputControls = new PlayerControls();
                Assign();
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

        #region Assign Controls

        public override void Assign()
        {
            AssignInputEvents();
            AssignCharacterActionEvents();
            AssignControllerEvent();
        }

        private void AssignInputEvents()
        {
            InputControls.Character.Roll.started +=
                _ =>
                {
                    if (Command.HasFlag(Command.Roll)) return;
                    Command |= Command.Roll;
                };
            InputControls.Character.Attack.started +=
                _ =>
                {
                    Command |= Command.PrepareAttack;
                };
            InputControls.Character.Attack.canceled +=
                _ =>
                {
                    if (!Command.HasFlag(Command.PrepareAttack)) return;
                    Command &= ~Command.PrepareAttack;
                    Command |= Command.Attack;
                };
            InputControls.Character.Movement.performed +=
                context => MotorController.MoveInput = context.ReadValue<Vector2>();
            InputControls.Character.Movement.canceled +=
                _ =>
                {
                    MotorController.MoveDirection = Vector3.zero;
                    MotorController.MoveInput = Vector2.zero;
                };
            InputControls.Character.SwapWeapon.started +=
                _ => Command |= Command.SwapWeapon;
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

        private void AssignControllerEvent()
        {
            WeaponVisualsController.OnSwapWeapon += (combatMode) =>
            {
                MotorController.SwitchCombatMode(combatMode);
                CurrentCombatMode = combatMode;
            };
        }

        #endregion

        #region Behaviours

        // Shared
        public void SetForwardDirection() => MotorController.SetForwardDirection();
        public void ApplyDirection() => MotorController.ApplyDirection();

        // Movement Behaviour
        public void ApplyStop() => MotorController.LocomotionController.ApplyStop();
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public void ApplyMovementAnimation() => AgentAnimatorController.ApplyMovementAnimation();
        public void StopMovementAnimation() => AgentAnimatorController.StopMovementAnimation();

        // Roll Behaviour 
        public void ApplyRoll() => MotorController.RollController.Roll();

        // Ranged Attack Behaviour
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

        // Swap weapon
        public void SwapWeapon() => WeaponVisualsController.SwapWeapon();

        public void SwitchCombatMode()
        {
            Debug.Log("combat mode");
        }

        // Return Command 
        public void ApplyPerformMovementCommand() => Command |= Command.Move;
        public void ApplyPerformRollCommand()
        {
            Command |= Command.Roll;
        }

        public void CancelPerformSwapWeaponCommand() => Command &= ~Command.SwapWeapon;
        public void CancelPerformRollCommand() => Command &= ~Command.Roll;
        public void CancelPerformPrepareAttackCommand() => Command &= ~Command.PrepareAttack;
        public void CancelPerformAttackCommand()
        {
            Command &= ~Command.Attack;
        }

        public void ResetPerformCommandExcept(Command command)
        {
            Command &= 0;
            Command |= command;
        }

        public void ResetState() => CurrentActionState = DefaultActionState;

        #endregion

        #region PreConditions

        public bool NeedInterruptAction => !CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                           !CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
                                           !CurrentActionState.HasFlag(CharacterActionType.EndAttackMove);
        public bool CanMoveAgain => CurrentActionState.HasFlag(CharacterActionType.MoveNextSkill);

        public bool CanPerformRollAgain => Command.HasFlag(Command.Roll);


        public bool CanPerformPrepareAttackAgain =>Command.HasFlag(Command.PrepareAttack);

        public bool CanPerformAttackMoveAgain => CurrentActionState.HasFlag(CharacterActionType.BeginAttackMove) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack);

        public bool CanPerformAttackAgain => Command.HasFlag(Command.Attack);
                                             

        public bool CanPerformSwapWeaponAgain => Command.HasFlag(Command.SwapWeapon) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndAttackMove);
        #endregion


        //  Event Handlers --------------------------------
    }
}