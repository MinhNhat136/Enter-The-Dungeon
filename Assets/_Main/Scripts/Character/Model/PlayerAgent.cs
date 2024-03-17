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
        public float RunSpeed { get { return _runSpeed; } }
        public float WalkSpeed { get { return _walkSpeed; } }
        public float TurnSpeed { get { return _turnSpeed; } }
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
                BaseAnimator = GetComponentInChildren<Animator>();
                SensorSystem = GetComponent<AiVisionSensorController>();
                LocomotionSystem = GetComponent<PlayerLocomotionController>();
                AgentAnimatorController = GetComponent<PlayerAnimatorController>();

                Controls = new PlayerControls();
                AssignInputEvents();
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
            Controls.Character.Run.performed += context =>
            {
                _isRunning = true;
            };

            Controls.Character.Run.canceled += context =>
            {
                _isRunning = false;
            };
        }



        //  Event Handlers --------------------------------

    }

}
