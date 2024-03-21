using Atomic.Character.Module;
using Atomic.Core.Interface;
using System;
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
        private static long Controller_TakeDamageIndex = 1L << 2;
        private static long Controller_VisionIndex = 1L << 3;
        private static long Controller_HealthIndex = 1L << 4;
        private static long Controller_AnimatorControllerIndex = 1L << 5;
        private static long Controller_MemoryIndex = 1L << 7;
        private static long Controller_TargetingIndex = 1L << 8;

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        #region Config
        public float Affiliation { get { return _affiliation; } }
        public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public bool IsInitialized { get { return _isInitialized; } }
        public Transform BodyWeakPoint { get { return _bodyWeakPoint; } }
        #endregion

        #region Engine Components
        public NavMeshAgent BaseNavMeshAgent { get; protected set; }
        public virtual Animator BaseAnimator { get; protected set; }
        #endregion

        #region Module Controllers 
        public virtual IAnimatorController AgentAnimatorController { get; protected set; }
        public virtual ILocomotionController LocomotionController { get; protected set; }
        public virtual IHitBoxController HitBoxController { get; protected set; }
        public virtual ITakeDamageController Damageable { get; protected set; }
        public virtual IAiMemoryController MemoryController { get; protected set; }
        public virtual IVisionController VisionController { get; protected set; }
        public virtual IAiWeaponControlSystem WeaponControl { get; protected set; }
        public virtual ITargetingController TargetingController { get; protected set; }
        #endregion

        //  Collections -----------------------------------


        //  Fields ----------------------------------------
        [SerializeField] private int _affiliation;
        [SerializeField] private Transform _bodyWeakPoint;

        private Vector3 _moveDirection;
        private AgentsManager _agentManager;
        private bool _isInitialized;
        private BaseAgent targetAi;
        private long _controllerBitSequence = 0;

 

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
                SetController_Locomotion();
                SetController_HitBox();
                SetController_TakeDamage();
                SetController_Vision();
                SetController_Animator();
                SetController_Memory();
                SetController_Targeting();

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

        protected virtual void SetComponent_NavMeshAgent()
        {
            if (!TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                return;
            }
            BaseNavMeshAgent = agent;
        }

        protected virtual void SetComponent_Animator()
        {
            BaseAnimator = GetComponentInChildren<Animator>();
        }


        protected virtual void SetController_Locomotion()
        {
            if (!TryGetComponent<ILocomotionController>(out ILocomotionController locomotionController))
            {
                return;
            }
            LocomotionController = locomotionController;
            _controllerBitSequence |= Controller_LocomotionIndex;
        }

        protected virtual void SetController_Animator()
        {
            if (!TryGetComponent<IAnimatorController>(out IAnimatorController animatorController))
            {
                return;
            }
            AgentAnimatorController = animatorController;
            _controllerBitSequence |= Controller_AnimatorControllerIndex;
        }

        protected virtual void SetController_HitBox()
        {
            if (!TryGetComponent<IHitBoxController>(out IHitBoxController hitBoxController))
            {
                return;
            }
            HitBoxController = hitBoxController;
            _controllerBitSequence |= Controller_HitBoxIndex;
        }

        protected virtual void SetController_TakeDamage()
        {
            if (!TryGetComponent<ITakeDamageController>(out ITakeDamageController takeDamageController))
            {
                return;
            }
            Damageable = takeDamageController;
            _controllerBitSequence |= Controller_TakeDamageIndex;
        }

        protected virtual void SetController_Vision()
        {
            if (!TryGetComponent<IVisionController>(out IVisionController visionController))
            {
                return;
            }
            VisionController = visionController;
            _controllerBitSequence |= Controller_VisionIndex;
        }

        protected virtual void SetController_Memory()
        {
            if (!TryGetComponent<IAiMemoryController>(out IAiMemoryController memoryController))
            {
                return;
            }
            MemoryController = memoryController;
            _controllerBitSequence |= Controller_MemoryIndex;
        }

        protected virtual void SetController_Targeting()
        {
            if (!TryGetComponent<ITargetingController>(out ITargetingController targetingController))
            {
                return;
            }
            TargetingController = targetingController;
            _controllerBitSequence |= Controller_TargetingIndex;
        }

        //  Event Handlers --------------------------------

    }
}

