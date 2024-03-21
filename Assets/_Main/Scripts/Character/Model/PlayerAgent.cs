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

        public PlayerControls Controls { get; private set; }


        //  Fields ----------------------------------------
        [Header("MOVEMENT MODULE")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _turnSpeed;

        [Header("TARGETING MODULE")]
        [SerializeField] private Transform _defaultAimPoint;

        private bool _isRunning;

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();

                BaseAnimator = GetComponentInChildren<Animator>();
                BaseNavMeshAgent = GetComponent<NavMeshAgent>();
                MemoryController = GetComponent<AiMemoryController>();
                BaseAnimator = GetComponentInChildren<Animator>();
                VisionController = GetComponent<AiVisionSensorController>();


                Controls = new PlayerControls();
                AssignInputEvents();

                LocomotionController.TurnSpeed = _turnSpeed;
                LocomotionController.MoveSpeed = _walkSpeed;
            }
        }

        //  Unity Methods   -------------------------------
        public override void DoEnable()
        {
            base.DoEnable();
            Controls.Enable();
        }

        public override void DoDisable()
        {
            base.DoDisable();
            Controls.Disable();
        }

        //  Other Methods ---------------------------------
        public void AssignInputEvents()
        {
            Controls.Character.Movement.performed += context =>
            {
                LocomotionController.MoveInput = context.ReadValue<Vector2>();
            };
            Controls.Character.Movement.canceled += context =>
            {
                LocomotionController.MoveInput = Vector2.zero;
            };
            Controls.Character.Run.performed += context =>
            {
                LocomotionController.MoveSpeed = _runSpeed;
                _isRunning = true;
            };
            Controls.Character.Run.canceled += context =>
            {
                LocomotionController.MoveSpeed = _walkSpeed;
                _isRunning = false;
            };
        }
        //  Event Handlers --------------------------------

    }

}
