using Atomic.Character.Module;
using Atomic.Core;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Atomic.Character.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Base class for defining characters with modular control systems.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseAgent : MonoBehaviour, IInitializable
    {
        //  Statics ---------------------------------------


        //  Events ----------------------------------------


        //  Properties ------------------------------------
        #region Config Runtime
        public bool IsInitialized => _isInitialized;
        public Transform BodyWeakPoint => bodyWeakPoint;
        public BaseAgent TargetAgent { get; set; }

        #endregion

        #region Module Controllers 
        public virtual IAgentAnimator AgentAnimatorController
        {
            get => _agentAnimatorController;
            protected set => _agentAnimatorController = value;
        }
        public virtual AiMotorController MotorController
        {
            get => _motorController;
            protected set => _motorController = value;
        }

        public virtual AiHitBoxController HitBoxController
        {
            get => _hitBoxController;
            protected set => _hitBoxController = value;
        }

        public virtual AiMemoryController MemoryController
        {
            get => _memoryController;
            protected set => _memoryController = value;
        }

        public virtual IVisionController VisionController
        {
            get => _visionController;
            protected set => _visionController = value;
        }
        public virtual ITargetingController TargetingController
        {
            get => _targetingController;
            protected set => _targetingController = value;
        }

        public virtual AiHealth HealthController => _healthController;

        #endregion

        //  Fields ----------------------------------------
        [FormerlySerializedAs("_bodyWeakPoint")] 
        [SerializeField] private Transform bodyWeakPoint;

        private bool _isInitialized;

        private AgentsManager _agentManager;

        #region Controller
        private IAgentAnimator _agentAnimatorController;
        private IVisionController _visionController;
        private ITargetingController _targetingController;
        private AiHitBoxController _hitBoxController;
        private AiMotorController _motorController;
        #endregion

        #region Shared Controller
        private AiHealth _healthController;
        private AiMemoryController _memoryController;
        #endregion

        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _agentManager = FindObjectOfType<AgentsManager>();
                _agentManager.RegisterAgent(this);

                AssignControllers();
            }
        }


        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new System.Exception("Base Agent not initialized");
            }
        }

        public virtual void DoEnable()
        {

        }

        public virtual void DoDisable()
        {

        }

        //  Unity Methods   -------------------------------
        void OnDestroy()
        {
            _agentManager.UnRegisterAgent(this);
        }

        //  Other Methods ---------------------------------
        public abstract void Assign();

        protected virtual void AssignControllers()
        {
            _memoryController = new()
            {
                MemorySpan = 1
            };
            _healthController = new AiHealth();

            this.SetController(out _agentAnimatorController);
            this.SetController(out _visionController);
            this.SetController(out _targetingController);
            this.SetController(out _motorController);
            this.SetController(out _hitBoxController);
            
            _agentAnimatorController.Initialize(this);
            _visionController.Initialize(this);
            _targetingController.Initialize(this);
            _motorController.Initialize(this);
            _hitBoxController.Initialize(this);
        }
        //  Event Handlers --------------------------------

    }
}

