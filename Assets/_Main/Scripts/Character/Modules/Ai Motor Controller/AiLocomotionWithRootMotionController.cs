using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controls locomotion for AI agents using a NavMeshAgent for navigation. 
    /// Note that the associated Animator component should utilize root motion.
    /// CAUTION: USING THIS CLASS WITH AnimatorEventsListernerWithRootMotion IN CHILD GAMEOBJECT CONTAIN ANIMATOR.
    /// </summary>
    public class AiLocomotionWithRootMotionController : MonoBehaviour, ILocomotionController, IAnimatorMoveReceive
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Vector2 MoveInput
        {
            get; set;
        }
        public float MoveSpeed
        {
            get; set;
        }
        public float RotationSpeed
        {
            get; set;
        }
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public AiMotorController Model
        {
            get
            {
                return _model;
            }
        }

        public float Acceleration
        {
            get; set;
        }
        public bool IsStopped
        {
            get
            {
                return _isStopped;
            }
            set
            {
                _isStopped = value; 
                if (value)
                {
                    Stop();
                }
            }
        }

        //  Fields ----------------------------------------

        private NavMeshAgent _navMeshAgent;
        private AiMotorController _model;
        private Animator _animator;


        private bool _isInitialized;
        private bool _isStopped; 

        //  Initialization  -------------------------------
        public void Initialize(AiMotorController model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = _model.BaseNavMeshAgent;
                _animator = _model.BaseAnimator;

                _animator.applyRootMotion = true;
                _navMeshAgent.updatePosition = false;
                _navMeshAgent.updateRotation = false;

                _navMeshAgent.speed = MoveSpeed;
                _navMeshAgent.angularSpeed = RotationSpeed;
                _navMeshAgent.acceleration = Acceleration;

                IsStopped = false;
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
        public void Tick()
        {

            ApplyMovement();
            ApplyRotation();
        }

        //  Other Methods ---------------------------------
        public void ApplyRotation()
        {
            if (_model.MoveDirection == Vector3.zero)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(_model.MoveDirection);

            desiredRotation.x = 0;
            desiredRotation.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, RotationSpeed * Time.deltaTime);
        }

        public void ApplyMovement()
        {
            _model.MoveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
            Vector3 destination = transform.position + _model.MoveDirection;
            _navMeshAgent.destination = destination;
        }

        public void Stop()
        {
            MoveInput = Vector2.zero;
            _navMeshAgent.destination = transform.position;
        }

        public void OnAnimatorMoveEvent()
        {
            transform.position = _navMeshAgent.nextPosition;
        }
    }

}
