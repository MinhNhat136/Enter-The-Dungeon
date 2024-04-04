using Atomic.Character.Config;
using Atomic.Character.Module;
using Atomic.Core;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

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
        public bool IsInitialized { get { return _isInitialized; } }
        public Transform BodyWeakPoint { get { return _bodyWeakPoint; } }
        public BaseAgent TargetAgent { get { return _targetAgent; } set { _targetAgent = value; } }
        public bool IsInVulnerable { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsDead { get; set; }


        #endregion

        #region Engine Components
        #endregion

        #region Module Controllers 
        public virtual IAgentAnimator AgentAnimatorController
        {
            get { return _agentAnimatorController; }
            protected set { _agentAnimatorController = value; }
        }
        public virtual AiMotorController MotorController
        {
            get { return _motorController; }
            protected set { _motorController = value; }
        }

        public virtual AiHitBoxController HitBoxController
        {
            get { return _hitBoxController; }
            protected set { _hitBoxController = value; }
        }

        public virtual AiMemoryController MemoryController
        {
            get { return _memoryController; }
            protected set { _memoryController = value; }
        }

        public virtual IVisionController VisionController
        {
            get { return _visionController; }
            protected set { _visionController = value; }
        }
        public virtual ITargetingController TargetingController
        {
            get { return _targetingController; }
            protected set { _targetingController = value; }
        }

        public virtual AiHealth HealthController
        {
            get { return _healthController; }
        }
        #endregion

        //  Fields ----------------------------------------
        [SerializeField] private Transform _bodyWeakPoint;

        private bool _isInitialized;

        private AgentsManager _agentManager;
        private BaseAgent _targetAgent;

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

            this.SetController<BaseAgent, IAgentAnimator>(ref _agentAnimatorController);
            this.SetController<BaseAgent, IVisionController>(ref _visionController);
            this.SetController<BaseAgent, ITargetingController>(ref _targetingController);
            this.SetController<BaseAgent, AiMotorController>(ref _motorController);
            this.SetController<BaseAgent, AiHitBoxController>(ref _hitBoxController);

        }
        //  Event Handlers --------------------------------

    }
}

