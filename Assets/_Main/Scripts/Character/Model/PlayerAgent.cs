using Atomic.Character.Model;
using Atomic.Character.Module;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Player
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
        public bool IsRunning { get { return _isRunning; } }
        public Transform DefaultAimPoint { get { return _defaultAimPoint; } }

        public PlayerControls InputControls { get; private set; }
        public override IAnimatorController AgentAnimatorController 
        {
            get { return _animatorController; }
        }
        //  Fields ----------------------------------------
        [Header("MOVEMENT MODULE")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _turnSpeed;

        [Header("TARGETING MODULE")]
        [SerializeField] private Transform _defaultAimPoint;

        private PlayerAnimatorController _animatorController;
        private bool _isRunning;

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();

                InputControls = new PlayerControls();
                AssignInputEvents();

                LocomotionController.TurnSpeed = _turnSpeed;
                LocomotionController.MoveSpeed = _walkSpeed;
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

        }
        //  Event Handlers --------------------------------

    }

}
