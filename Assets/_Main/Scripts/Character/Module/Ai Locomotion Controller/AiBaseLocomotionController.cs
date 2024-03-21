using Atomic.Character.Model;
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
        public float TurnSpeed 
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

        //  Fields ----------------------------------------

        private NavMeshAgent _navMeshAgent;
        private BaseAgent _model;


        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = _model.BaseNavMeshAgent;
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

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, TurnSpeed * Time.deltaTime);
        }

        public void ApplyMovement()
        {
            _model.MoveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);

            if (_model.MoveDirection.magnitude > 0)
            {
                _navMeshAgent.Move(_model.MoveDirection * MoveSpeed * Time.deltaTime);
            }
        }


        //  Event Handlers --------------------------------

    }

}
