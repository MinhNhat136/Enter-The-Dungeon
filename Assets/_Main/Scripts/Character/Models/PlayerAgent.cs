using UnityEngine;
using UnityEngine.TextCore.Text;

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

        protected bool IsMoveInput =>
            !Mathf.Approximately(MotorController.LocomotionController.MoveInput.sqrMagnitude, 0f);


        //  Fields ----------------------------------------

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
        }


        private void AssignInputEvents()
        {
            InputControls.Character.Movement.performed += 
                context => MotorController.LocomotionController.MoveInput = context.ReadValue<Vector2>();
            InputControls.Character.Movement.canceled +=
                _ =>
                {
                    AgentAnimatorController.StopMovementAnimation();
                    MotorController.MoveDirection = Vector3.zero;
                    MotorController.LocomotionController.MoveInput = Vector2.zero;
                };
            InputControls.Character.Roll.performed +=
                _ => MotorController.IsRolling = true;
            InputControls.Character.Attack.started +=
                _ => MotorController.IsAttacking = true; 
            InputControls.Character.Attack.performed += 
                _ => MotorController.IsCharging = true;
            InputControls.Character.Attack.canceled +=
                _ => MotorController.IsCharging = false;
        }
        //  Event Handlers --------------------------------

        // Movement Behaviour
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public void ApplyMovementAnimation() => AgentAnimatorController.ApplyMovementAnimation();
        
        // Roll Behaviour 
        public void ApplyRoll() => MotorController.RollController.Roll();
        public void ApplyRollAnimation() => AgentAnimatorController.ApplyRollAnimation();

        // Ranged Attack Behaviour
        public void ApplyRangedAttack_Charging() => AgentAnimatorController.ApplyRangedAttack_Charge_Start_Animation();
        public void ApplyRangedAttack_Release() => AgentAnimatorController.ApplyRangedAttack_Charge_Release_Animation();

        // Melee Attack Behaviour


    }
}