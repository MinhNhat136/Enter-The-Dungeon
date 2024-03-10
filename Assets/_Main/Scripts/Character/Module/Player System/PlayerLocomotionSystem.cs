using Atomic.Character.Player;
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
    public class PlayerLocomotionSystem : MonoBehaviour, ILocomotionSystem
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Vector2 MoveInput 
        { 
            get; private set; 
        }

        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public float RunSpeed
        {
            get
            {
                return _runSpeed;
            }
        }

        public float WalkSpeed
        {
            get { return _walkSpeed; }
        }

        public bool IsRunning
        {
            set { _isRunning = value; }
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        //  Fields ----------------------------------------

        [Header("Movement info")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _turnSpeed;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private PlayerAgent _model;

        private float _speed;
        private Vector3 _movementDirection;
        private bool _isRunning;

        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!_isInitialized)
            {
                _speed = _walkSpeed;
                AssignInputEvents();
            }
        }

        public void RequireIsInitialized()
        {

            if (!_isInitialized)
            {
                throw new System.Exception("Not initialized player locomotion system");
            }
        }

        //  Unity Methods   -------------------------------
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _model = GetComponent<PlayerAgent>();

        }

        //  Other Methods ---------------------------------
        public void AssignInputEvents()
        {
            _model.Controls.Character.Movement.performed += context =>
            {
                MoveInput = context.ReadValue<Vector2>();
            };
            _model.Controls.Character.Movement.canceled += context =>
            {
                MoveInput = Vector2.zero;
            };

            _model.Controls.Character.Run.performed += context =>
            {
                IsRunning = true;
                Speed = RunSpeed;
            };

            _model.Controls.Character.Run.canceled += context =>
            {
                IsRunning = false;
                Speed = WalkSpeed;
            };
        }

        public void ApplyAnimator()
        {
            float xVelocity = Vector3.Dot(_movementDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(_movementDirection.normalized, transform.forward);

            _animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
            _animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

            bool playRunAnimation = _isRunning & _movementDirection.magnitude > 0;
            _animator.SetBool("isRunning", playRunAnimation);
        }

        public void ApplyRotation()
        {
            if (_movementDirection == Vector3.zero)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(_movementDirection);

            desiredRotation.x = 0;
            desiredRotation.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _turnSpeed * Time.deltaTime);
        }

        public void ApplyMovement()
        {
            _movementDirection = new Vector3(MoveInput.x, 0, MoveInput.y);

            if (_movementDirection.magnitude > 0)
            {
                _navMeshAgent.Move(_movementDirection * _speed * Time.deltaTime);
            }
        }
        //  Event Handlers --------------------------------

    }

}
