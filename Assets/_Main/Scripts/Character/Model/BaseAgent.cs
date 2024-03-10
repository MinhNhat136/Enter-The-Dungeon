using Atomic.Character.Module;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public abstract class BaseAgent : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsPlayer { get { return _isPlayer; } }

        public bool IsDeath { get { return _isDeath; } set { _isDeath = value; } }
        public bool IsStun { get { return _isStun; } set { _isStun = value; } }
                
        public float Affiliation { get { return _affiliation; } }
        public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public Transform BodyWeakPoint { get { return _bodyWeakPoint; } }
        public NavMeshAgent Agent { get; private set; }
        public virtual Animator Animator { get; private set; }
        public virtual BaseAgent TargetAgent { get { return _targetAgent; } set { _targetAgent = value; } }
        public virtual ILocomotionSystem LocomotionSystem { get; protected set; }
        public virtual IDamageable Damageable { get; protected set; }
        public virtual IVisionSystem SensorSystem { get; protected set; }
        public virtual IAiWeaponControlSystem WeaponControlSystem { get; protected set; }

        //  Fields ----------------------------------------
        [SerializeField] private int _affiliation;
        [SerializeField] private Transform _bodyWeakPoint;
        [SerializeField] private bool _isPlayer;

        private Vector3 _moveDirection;
        private BaseAgent _targetAgent;
        private AgentsManager _agentManager;

        private bool _isStun; 
        private bool _isDeath;

        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------
        public void Awake()
        {
            _agentManager = FindObjectOfType<AgentsManager>();
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            _agentManager = GameObject.FindObjectOfType<AgentsManager>();

            _agentManager.RegisterAgent(this);
        }

        void OnDestroy()
        {
            _agentManager.UnRegisterAgent(this);
        }
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

    }
}

