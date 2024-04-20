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
                CurrentActionState = CharacterActionState.MoveNextSkill;
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
                _ => ApplyRoll();
            InputControls.Character.Attack.started +=
                _ => ApplyRangedAttack_ChargingAnimation();
            InputControls.Character.Attack.canceled +=
                _ => CommandEvents[Command.Attack] = true;
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
        }

        private void AssignCharacterActionEvents()
        {
            RegisterActionTrigger(CharacterActionType.BeginRoll, () =>
            {
                CurrentActionState &= ~CharacterActionState.MoveNextSkill;
                CurrentActionState = CharacterActionState.Rolling;
                ApplyRoll();
            });
            RegisterActionTrigger(CharacterActionType.StopRoll, () =>
            {
                CurrentActionState &= ~CharacterActionState.Rolling;
                CurrentActionState |= CharacterActionState.MoveNextSkill;
                CancelPerformRollCommand();
            });
            RegisterActionTrigger(CharacterActionType.PrepareAttack, () =>
            {
                CurrentActionState &= ~CharacterActionState.MoveNextSkill;
                CurrentActionState |= CharacterActionState.BeginPrepareAttack;
            });

            RegisterActionTrigger(CharacterActionType.PrepareAttackCompleted, () =>
            {
                CurrentActionState |= CharacterActionState.EndPrepareAttack;
            });
            RegisterActionTrigger(CharacterActionType.ChargeFull, () => { });
            RegisterActionTrigger(CharacterActionType.BeginAttackMove, () => { });
            RegisterActionTrigger(CharacterActionType.StopAttackMove, () => { });
            RegisterActionTrigger(CharacterActionType.BeginAttack, () =>
            {
                CurrentActionState &= ~CharacterActionState.EndPrepareAttack;
                CurrentActionState |= CharacterActionState.Attacking;
            });
            RegisterActionTrigger(CharacterActionType.BeginAttackMove, () => { });
            RegisterActionTrigger(CharacterActionType.StopAttackMove, () => { });
            RegisterActionTrigger(CharacterActionType.EndAttack, () =>
            {
                CurrentActionState &= ~CharacterActionState.Attacking;
                CurrentActionState |= CharacterActionState.EndAttack;
                CurrentActionState |= CharacterActionState.MoveNextSkill;
                CancelPerformAttackCommand();
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
        private void ApplyRoll() => MotorController.RollController.Roll();
        public void ApplyRollAnimation() => AgentAnimatorController.ApplyRollAnimation();

        // Ranged Attack Behaviour
        public void BeginPrepareAttack()
        {
            MotorController.LocomotionController.ApplyStop();
        }

        public void ApplyPreparingAttack()
        {
            if (!CurrentActionState.HasFlag(CharacterActionState.PreparingAttack))
            {
                CurrentActionState &= ~CharacterActionState.BeginPrepareAttack;
                CurrentActionState |= CharacterActionState.PreparingAttack;
            }
        }


        public void ApplyRangedAttack_ChargingAnimation() =>
            AgentAnimatorController.ApplyRangedAttack_Charge_Start_Animation();

        public void ApplyRangedAttack_ReleaseAnimation() =>
            AgentAnimatorController.ApplyRangedAttack_Charge_Release_Animation();

        public void BeginAttack() => MotorController.CombatController.BeginAttack();
        public void EndAttack() => MotorController.CombatController.EndAttack();

        // Swap weapon
        public void SwapWeapon() => WeaponVisualsController.SwapWeapon();
        public void OnSwapWeaponCompleted() => CommandEvents[Command.SwapWeapon] = false;

        public void SwitchCombatMode()
        {
            Debug.Log("combat mode");
        }

        // Return Command 
        private void CancelAllCommand()
        {
            CurrentActionState = CharacterActionState.None;
            CommandEvents.Keys.ToList().ForEach(command => CommandEvents[command] = false);
        }

        private void CancelPerformRollCommand() => CommandEvents[Command.Roll] = false;

        private void CancelPerformAttackCommand()
        {
            CommandEvents[Command.PrepareAttack] = false;
            CommandEvents[Command.Attack] = false;
        }

        #endregion

        #region PreConditions

        public bool CanPerformRollAction => CommandEvents[Command.Roll] &&
                                            !CurrentActionState.HasFlag(CharacterActionState.Rolling);

        public bool CanPerformBeginPrepareAttackAction => CommandEvents[Command.PrepareAttack] &&
                                                          !CurrentActionState.HasFlag(CharacterActionState.Rolling) &&
                                                          !CurrentActionState.HasFlag(CharacterActionState.BeginPrepareAttack);

        public bool CanPerformPreparingAttackAction => CurrentActionState.HasFlag(CharacterActionState.BeginPrepareAttack);
        public bool CanPerformEndPrepareAttack => CurrentActionState.HasFlag(CharacterActionState.PreparingAttack);

        public bool CanPerformAttackMoveAction = false;

        public bool CanPerformAttack => CommandEvents[Command.Attack] &&
                                        !CurrentActionState.HasFlag(CharacterActionState.Rolling) &&
                                        CurrentActionState.HasFlag(CharacterActionState.EndPrepareAttack);

        public bool CanEndAttack = false;

        public bool CanPerformSwapWeaponAction => CommandEvents[Command.SwapWeapon] &&
                                                  CurrentActionState.HasFlag(CharacterActionState.MoveNextSkill) &&
                                                  !CurrentActionState.HasFlag(CharacterActionState.Attacking) &&
                                                  !CurrentActionState.HasFlag(CharacterActionState.EndPrepareAttack);

        public bool CanMoveNextSkill => CurrentActionState.HasFlag(CharacterActionState.MoveNextSkill);

        #endregion


        //  Event Handlers --------------------------------
    }
}