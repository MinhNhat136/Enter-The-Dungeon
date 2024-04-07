using Atomic.Character.Config;
using Atomic.Character.Model;
using Atomic.Core;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AiMotorController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>, ITickable
    {
        //  Statics ---------------------------------------
        private static int Controller_LocomotionIndex = 1 << 0;
        private static int Controller_JumpIndex = 1 << 1;
        private static int Controller_RollIndex = 1 << 2;
        private static int Controller_FlyIndex = 1 << 3;

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public ILocomotionController LocomotionController 
        {
            get { return _locomotionController; }
            set { _locomotionController = value; } 
        }

        public AiRollController RollController 
        { 
            get; private set; 
        }
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public BaseAgent Model 
        { 
            get { return _model; } 
        }

        public NavMeshAgent BaseNavMeshAgent 
        {
            get { return _navMeshAgent; } 
        }
        public virtual Animator BaseAnimator 
        { 
            get { return _animator; } 
        }

        public Vector3 MoveDirection
        { 
            get { return _moveDirection; } 
            set { _moveDirection = value; } 
        }

        //  Fields ----------------------------------------
        [SerializeField] private AgentConfig _config; 

        private int _controllerBitSequence = 0;
        private bool _isInitialized;
        private BaseAgent _model;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Vector3 _moveDirection;

        private ILocomotionController _locomotionController;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if(!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = GetComponent<NavMeshAgent>();
                _animator = GetComponentInChildren<Animator>();
                AssignLocomotionController();
                _locomotionController.Initialize(this);
            }
        }

        public void RequireIsInitialized()
        {
        }

        public void Tick()
        {
            _locomotionController.Tick();
        }

        //  Unity Methods   -------------------------------
        public void AssignLocomotionController()
        {
            this.SetController<AiMotorController, ILocomotionController>(ref _locomotionController, Controller_LocomotionIndex, ref _controllerBitSequence);
            if(_locomotionController == null)
            {
                return; 
            }
            LocomotionController.RotationSpeed = _config.RotateSpeed;
            LocomotionController.MoveSpeed = _config.WalkSpeed;
            LocomotionController.Acceleration = _config.Acceleration;
        }

        public void AssignRollController()
        {


        }

        public void AssignJumpController()
        {


        }

        public void AssignFlyController()
        {


        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
    }
}
