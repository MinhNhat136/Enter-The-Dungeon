using RMC.Core.Architectures.Mini.Context;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Player
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerLocomotionSystem : MonoBehaviour, ILocomotionSystem, IInitializable
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
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
        private PlayerModel _inputsSystem;

        private float _speed;
        private Vector3 _movementDirection;
        private bool _isRunning;

        private bool _isInitialized;
        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!_isInitialized)
            {
                _navMeshAgent = GetComponent<NavMeshAgent>();
                _animator = GetComponentInChildren<Animator>();
                _inputsSystem = GetComponent<PlayerModel>();

                _inputsSystem.OnStateRunning.AddListener((value) =>
                {
                    if (value)
                    {
                        Speed = RunSpeed;
                        IsRunning = true;
                    }
                    else
                    {
                        Speed = WalkSpeed;
                        IsRunning = false;
                    }
                });

                _speed = _walkSpeed;
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
        private void OnDestroy()
        {
            _inputsSystem.OnStateRunning.RemoveAllListeners();
        }


        //  Other Methods ---------------------------------
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
            _movementDirection = new Vector3(_inputsSystem.InputHorizontal, 0, _inputsSystem.InputVertical);

            if (_movementDirection.magnitude > 0)
            {
                _navMeshAgent.Move(_movementDirection * _speed * Time.deltaTime);
            }
        }



        //  Event Handlers --------------------------------

    }

}
