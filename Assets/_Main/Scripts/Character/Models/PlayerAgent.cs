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
                    CommandEvents[Command.Roll] = true;
                };
            InputControls.Character.Attack.started +=
                _ =>
                {
                    CommandEvents[Command.PrepareAttack] = true;
                };
            InputControls.Character.Attack.canceled +=
                _ =>
                {
                    CommandEvents[Command.PrepareAttack] = false;
                    CommandEvents[Command.Attack] = true;
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
                _ => CommandEvents[Command.SwapWeapon] = true;
            InputControls.Character.SwapWeapon.canceled +=
                _ => CommandEvents[Command.SwapWeapon] = false;
        }

        private void AssignCharacterActionEvents()
        {
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
            
            RegisterActionTrigger(CharacterActionType.BeginPrepareAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginPrepareAttack;
                CurrentActionState &= ~CharacterActionType.EndPrepareAttack;
                CurrentActionState &= ~CharacterActionType.MoveNextSkill;
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
            
            RegisterActionTrigger(CharacterActionType.ChargeFull, () =>
            {
                
            });
            
            RegisterActionTrigger(CharacterActionType.BeginAttackMove, () =>
            {
                CurrentActionState |= CharacterActionType.BeginAttackMove;
                CurrentActionState &= ~CharacterActionType.MoveNextSkill;
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
            
            RegisterActionTrigger(CharacterActionType.BeginAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginAttack;
                CurrentActionState &= ~CharacterActionType.MoveNextSkill;
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
            RegisterActionTrigger(CharacterActionType.MoveNextSkill, 
            () =>
            {
                CurrentActionState |= CharacterActionType.MoveNextSkill;
            });
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
        public void BeginPrepareAttack()
        {
            MotorController.LocomotionController.ApplyStop();
        }

        public void ApplyPreparingAttack()
        {
            
        }


        public void ApplyRangedAttack_ChargingAnimation() =>
            AgentAnimatorController.ApplyRangedAttack_Charge_Start_Animation();

        public void ApplyRangedAttack_ReleaseAnimation() =>
            AgentAnimatorController.ApplyRangedAttack_Charge_Release_Animation();

        public void BeginAttack() => MotorController.CombatController.BeginAttack();
        public void EndAttack() => MotorController.CombatController.EndAttack();

        // Swap weapon
        public void SwapWeapon() => WeaponVisualsController.SwapWeapon();

        public void SwitchCombatMode()
        {
            Debug.Log("combat mode");
        }

        // Return Command 
        public void ApplyPerformMovementCommand() => CommandEvents[Command.Move] = true;
        public void ApplyPerformRollCommand()
        {
            CommandEvents[Command.Roll] = true;
        }

        public void CancelPerformSwapWeaponCommand() => CommandEvents[Command.SwapWeapon] = false;
        public void CancelPerformRollCommand() => CommandEvents[Command.Roll] = false;

        public void CancelPerformAttackCommand()
        {
            CommandEvents[Command.PrepareAttack] = false;
            CommandEvents[Command.Attack] = false;
        }

        #endregion

        #region PreConditions
        public bool CanMoveAgain => CurrentActionState.HasFlag(CharacterActionType.MoveNextSkill);

        public bool CanPerformRollAgain => CommandEvents[Command.Roll];


        public bool CanPerformPrepareAttackAgain => CommandEvents[Command.PrepareAttack] &&
                                                    CurrentActionState.HasFlag(CharacterActionType.BeginPrepareAttack) &&
                                                    CurrentActionState.HasFlag(CharacterActionType.EndRoll);

        public bool CanPerformAttackMoveAgain => CurrentActionState.HasFlag(CharacterActionType.BeginAttackMove) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack);

        public bool CanPerformAttackAgain => CommandEvents[Command.Attack] &&
                                             CurrentActionState.HasFlag(CharacterActionType.BeginAttack) &&
                                             CurrentActionState.HasFlag(CharacterActionType.EndRoll) &&
                                             CurrentActionState.HasFlag(CharacterActionType.EndAttackMove) &&
                                             CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack);

        public bool CanPerformSwapWeaponAgain => CommandEvents[Command.SwapWeapon] &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
                                                 CurrentActionState.HasFlag(CharacterActionType.EndAttackMove);
        #endregion


        //  Event Handlers --------------------------------
    }
}