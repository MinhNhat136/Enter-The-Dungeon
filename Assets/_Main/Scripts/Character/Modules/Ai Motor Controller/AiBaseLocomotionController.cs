using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controls locomotion for AI agents using a NavMeshAgent for navigation. 
    /// Note that the associated Animator component should not utilize root motion.
    /// CAUTION: USING THIS CLASS WITH AnimatorEventsListerner IN CHILD GAMEOBJECT CONTAIN ANIMATOR.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AiBaseLocomotionController : MonoBehaviour, ILocomotionController
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

        public BaseAgent Model
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

        AiMotorController IInitializableWithBaseModel<AiMotorController>.Model => throw new System.NotImplementedException();

        public bool IsStopped { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        //  Fields ----------------------------------------

        private NavMeshAgent _navMeshAgent;
        private BaseAgent _model;


        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            /*if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = _model.BaseNavMeshAgent;

                _navMeshAgent.speed = MoveSpeed;
                _navMeshAgent.angularSpeed = RotationSpeed;
                _navMeshAgent.acceleration = Acceleration;
            }*/
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
            /*if (_model.MoveDirection == Vector3.zero)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(_model.MoveDirection);

            desiredRotation.x = 0;
            desiredRotation.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, RotationSpeed * Time.deltaTime);*/
        }

        public void ApplyMovement()
        {
            /*_model.MoveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);

            if (_model.MoveDirection.magnitude > 0)
            {
                _navMeshAgent.Move(_model.MoveDirection * MoveSpeed * Time.deltaTime);
            }*/
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(AiMotorController model)
        {
            throw new System.NotImplementedException();
        }


        //  Event Handlers --------------------------------

    }

}
