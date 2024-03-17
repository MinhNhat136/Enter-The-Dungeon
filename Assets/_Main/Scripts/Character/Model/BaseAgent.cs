using Atomic.Character.Module;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public abstract class BaseAgent : MonoBehaviour, IInitializable, IDoEnable
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        #region Config
        public float Affiliation { get { return _affiliation; } }
        public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public bool IsInitialized { get { return _isInitialized; } }
        #endregion


        #region Module Controller 
        public Transform BodyWeakPoint { get { return _bodyWeakPoint; } }
        public NavMeshAgent BaseNavMeshAgent { get; protected set; }
        public virtual Animator BaseAnimator { get; protected set; }
        public virtual IAnimatorController AgentAnimatorController { get; protected set; }
        public virtual ILocomotionController LocomotionSystem { get; protected set; }
        public virtual IDamageable Damageable { get; protected set; }
        public virtual IVisionSystem SensorSystem { get; protected set; }
        public virtual IAiWeaponControlSystem WeaponControlSystem { get; protected set; }
        #endregion

        //  Fields ----------------------------------------
        [SerializeField] private int _affiliation;
        [SerializeField] private Transform _bodyWeakPoint;

        private Vector3 _moveDirection;
        private AgentsManager _agentManager;
        private bool _isInitialized;


        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _agentManager = FindObjectOfType<AgentsManager>();
                _agentManager.RegisterAgent(this);
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


        //  Event Handlers --------------------------------

    }
}

