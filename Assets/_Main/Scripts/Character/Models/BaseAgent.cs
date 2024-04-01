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
    public abstract class BaseAgent : MonoBehaviour, IInitializable, IDoEnable
    {
        //  Statics ---------------------------------------
        private static long Controller_LocomotionIndex = 1L << 0;
        private static long Controller_HitBoxIndex = 1L << 1;
        private static long Controller_VisionIndex = 1L << 2;
        private static long Controller_AnimatorControllerIndex = 1L << 4;
        private static long Controller_MemoryIndex = 1L << 5;
        private static long Controller_TargetingIndex = 1L << 6;
        private static long Controller_WeaponIndex = 1L << 7;

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        #region Config Runtime
        public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public bool IsInitialized { get { return _isInitialized; } }
        public Transform BodyWeakPoint { get { return _bodyWeakPoint; } }
        public BaseAgent TargetAgent { get { return _targetAgent; } set { _targetAgent = value; } }
        #endregion

        #region Engine Components
        public NavMeshAgent BaseNavMeshAgent { get { return _navMeshAgent; } }
        public virtual Animator BaseAnimator { get { return _animator; } }
        #endregion

        #region Module Controllers 
        public virtual IAnimatorController AgentAnimatorController
        {
            get { return _agentAnimatorController; }
            protected set { _agentAnimatorController = value; }
        }
        public virtual ILocomotionController LocomotionController
        {
            get { return _locomotionController; }
            protected set { _locomotionController = value; }
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

        public virtual IAiWeaponControlSystem WeaponControl
        {
            get { return _weaponControlSystem; }
            protected set { _weaponControlSystem = value; }
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

        //  Collections -----------------------------------


        //  Fields ----------------------------------------
        [SerializeField] private Transform _bodyWeakPoint;

        private long _controllerBitSequence = 0;
        private bool _isInitialized;

        private Vector3 _moveDirection;
        private AgentsManager _agentManager;
        private BaseAgent _targetAgent;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        #region Interface Controller
        private ILocomotionController _locomotionController;
        private IAnimatorController _agentAnimatorController;
        private IVisionController _visionController;
        private ITargetingController _targetingController;
        private IAiWeaponControlSystem _weaponControlSystem;
        #endregion

        #region Monobehaviour Controller
        private AiHitBoxController _hitBoxController;
        private AiHealth _healthController;
        #endregion

        #region Non-Monobehaviour Controller
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

                // Engine Components
                SetComponent_NavMeshAgent();
                SetComponent_Animator();

                // Controllers
                this.SetController<ILocomotionController>(ref _locomotionController, Controller_LocomotionIndex, ref _controllerBitSequence);
                this.SetController<IVisionController>(ref _visionController, Controller_VisionIndex, ref _controllerBitSequence);
                this.SetController<IAnimatorController>(ref _agentAnimatorController, Controller_AnimatorControllerIndex, ref _controllerBitSequence);
                this.SetController<ITargetingController>(ref _targetingController, Controller_TargetingIndex, ref _controllerBitSequence);
                this.SetController<IAiWeaponControlSystem>(ref _weaponControlSystem, Controller_WeaponIndex, ref _controllerBitSequence);
                
                this.SetController<AiHitBoxController>(ref _hitBoxController, Controller_HitBoxIndex, ref _controllerBitSequence);

                AtomicExtension.AddController<AiMemoryController>(ref _memoryController, Controller_MemoryIndex, ref _controllerBitSequence);
                _healthController = GetComponent<AiHealth>();
                Debug.Log(LongToBitString(_controllerBitSequence));
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new System.Exception("Base Agent not initialized");
            }
        }

        //  Unity Methods   -------------------------------
        void OnDestroy()
        {
            _agentManager.UnRegisterAgent(this);
        }

        public virtual void DoEnable()
        {
        }

        public virtual void DoDisable()
        {

        }

        //  Other Methods ---------------------------------
        static string LongToBitString(long num)
        {
            int numBits = sizeof(long) * 8;

            string bitString = "";

            for (int i = numBits - 1; i >= 0; i--)
            {
                int bit = (int)((num >> i) & 1);
                bitString += bit;
            }

            return bitString;
        }

        #region Setup
        protected virtual void SetComponent_NavMeshAgent()
        {
            if (!TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                return;
            }
            _navMeshAgent = agent;
        }

        protected virtual void SetComponent_Animator()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        #endregion

        public abstract void Assign();
        //  Event Handlers --------------------------------

    }
}

