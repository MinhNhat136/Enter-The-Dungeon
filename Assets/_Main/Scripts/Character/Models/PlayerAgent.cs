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
        private PlayerControls InputControls
        {
            get;
            set;
        }

        protected bool IsMoveInput => !Mathf.Approximately(MotorController.LocomotionController.MoveInput.sqrMagnitude, 0f);


        //  Fields ----------------------------------------

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            Debug.Log("run");
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
            InputControls.Character.Movement.performed += context =>
            {
                MotorController.LocomotionController.MoveInput = context.ReadValue<Vector2>();
            };
            InputControls.Character.Movement.canceled += _ =>
            {
                MotorController.LocomotionController.MoveInput = Vector2.zero;
            };
            InputControls.Character.Roll.performed += _ =>
            {
                MotorController.IsRolling = true;
            };

            InputControls.Character.Attack.performed += _ =>
            {
            };

            InputControls.Character.Attack.canceled += _ =>
            {
            };
        }
        //  Event Handlers --------------------------------

        // Movement Behaviour
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public void ApplyMovementAnimation() => AgentAnimatorController.ApplyMovementAnimation();
      
        // Roll Behaviour
        public void ApplyRollMovement() => MotorController.RollController.Roll();
        public void ApplyRollAnimation() => AgentAnimatorController.ApplyRollAnimation();
        

        // Attack Behaviour
    }

}
