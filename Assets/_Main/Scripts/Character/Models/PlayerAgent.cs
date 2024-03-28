using Atomic.Character.Config;
using Atomic.Character.Model;
using Atomic.Character.Module;
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
        [HideInInspector] public bool isAttack;

        //  Properties ------------------------------------
        public PlayerControls InputControls { get; private set; }
        public override IAnimatorController AgentAnimatorController
        {
            get { return _animatorController; }
        }

        //  Fields ----------------------------------------
        [SerializeField] private PlayerConfig _agentConfig;
        [SerializeField] private VisionConfig _visionConfig;
        [SerializeField] private TargetingConfig _targetingConfig; 

        private PlayerAnimatorController _animatorController;

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

        //  Unity Methods   -------------------------------
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

        //  Other Methods ---------------------------------
        public override void Assign()
        {
            AssignInputEvents();
            AssignLocomotionController();

            _visionConfig.Assign(VisionController);
            _targetingConfig.Assign(TargetingController);
        }

        public void AssignInputEvents()
        {
            InputControls.Character.Movement.performed += context =>
            {
                LocomotionController.MoveInput = context.ReadValue<Vector2>();
            };
            InputControls.Character.Movement.canceled += context =>
            {
                LocomotionController.MoveInput = Vector2.zero;
            };
            InputControls.Character.Roll.performed += context =>
            {
                _animatorController.ApplyRoll();
            };
            InputControls.Character.Attack.performed += context =>
            {
                isAttack = true;
            };

            InputControls.Character.Attack.canceled += context =>
            {
                isAttack = false;
            };
        }

        public void AssignLocomotionController()
        {
            LocomotionController.RotationSpeed = _agentConfig.RotateSpeed;
            LocomotionController.MoveSpeed = _agentConfig.WalkSpeed;
            LocomotionController.Acceleration = _agentConfig.Acceleration;
        }


        //  Event Handlers --------------------------------

    }

}
