using Atomic.Character.Module;
using Atomic.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

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
        public override void Assign()
        {
            AssignInputEvents();
            AssignCharacterActionEvents();
            AssignControllerEvent();
        }
        
        private void AssignInputEvents()
        {
            InputControls.Character.Roll.performed +=
                _ =>
                {
                    StateManager.IsRolling = true;
                };
            InputControls.Character.Attack.started +=
                _ => StateManager.IsPreparingAttack = true; 
            InputControls.Character.Attack.performed += 
                _ => StateManager.IsPreparingAttack = true;
            InputControls.Character.Attack.canceled +=
                _ =>
                {
                    StateManager.IsPreparingAttack = false;
                    StateManager.IsAttacking = true;
                };
            InputControls.Character.Movement.performed += 
                context => MotorController.MoveInput = context.ReadValue<Vector2>();
            InputControls.Character.Movement.canceled +=
                _ =>
                {
                    AgentAnimatorController.StopMovementAnimation();
                    MotorController.MoveDirection = Vector3.zero;
                    MotorController.MoveInput = Vector2.zero;
                };
            InputControls.Character.SwapWeapon.started +=
                _ => StateManager.IsSwapWeapon = true;
        }
        private void AssignCharacterActionEvents()
        {
            RegisterActionTrigger(CharacterActionType.BeginRoll, () =>
            {
                StateManager.CanMoveNextSkill = false;
                StateManager.CanPrepareAttackAgain = false;
                StateManager.CanRollAgain = false;
            });
            RegisterActionTrigger(CharacterActionType.StopRoll, () =>
            {
                StateManager.IsRolling = false;
                StateManager.CanPrepareAttackAgain = true;
                StateManager.CanRollAgain = true;
            });
            RegisterActionTrigger(CharacterActionType.PrepareAttack, () =>
            {
                StateManager.CanAttack = false;
                StateManager.CanMoveNextSkill = false;
            });
            RegisterActionTrigger(CharacterActionType.PrepareAttackCompleted, () =>
            {
                StateManager.CanAttack = true;
            });
            RegisterActionTrigger(CharacterActionType.ChargeFull, () =>
            {
                
            });
            RegisterActionTrigger(CharacterActionType.BeginAttack, () =>
            {
                StateManager.CanPrepareAttackAgain = false;
            });
            RegisterActionTrigger(CharacterActionType.BeginAttackMove, () =>
            {
                
            });
            RegisterActionTrigger(CharacterActionType.StopAttackMove, () =>
            {
                
            });
            RegisterActionTrigger(CharacterActionType.EndAttack, () =>
            {
                StateManager.CanAttack = false;
                StateManager.CanPrepareAttackAgain = true;
            });
            RegisterActionTrigger(CharacterActionType.MoveNextSkill, () => StateManager.CanMoveNextSkill = true);
        }

        private void AssignControllerEvent()
        {
            WeaponVisualsController.OnSwapWeapon += (combatMode) =>
            {
                MotorController.SwitchCombatMode(combatMode);
                StateManager.CurrentCombatMode = combatMode;
            };
        }
        
        
        //  Event Handlers --------------------------------

        // Movement Behaviour
        public void SetForwardDirection() => MotorController.SetForwardDirection();
        public void ApplyDirection() => MotorController.ApplyDirection();
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public void ApplyStop() => MotorController.LocomotionController.ApplyStop();
        public void ApplyMovementAnimation() => AgentAnimatorController.ApplyMovementAnimation();
        
        // Roll Behaviour 
        public void ApplyRoll() => MotorController.RollController.Roll();
        public void ApplyRollAnimation() => AgentAnimatorController.ApplyRollAnimation();
        public void CancelRoll() => StateManager.IsRolling = false;

        // Ranged Attack Behaviour
        public void BeginAttack() => MotorController.CombatController.BeginAttack();
        public void ApplyRangedAttack_ChargingAnimation() => AgentAnimatorController.ApplyRangedAttack_Charge_Start_Animation();
        public void ApplyRangedAttack_ReleaseAnimation() => AgentAnimatorController.ApplyRangedAttack_Charge_Release_Animation();
        public void EndAttack() => MotorController.CombatController.EndAttack();
        
        // Swap weapon
        public void SwapWeapon() => WeaponVisualsController.SwapWeapon();

        public void SwitchCombatMode()
        {
            Debug.Log("combat mode");
        }

        // Attack Behaviour


    }
}