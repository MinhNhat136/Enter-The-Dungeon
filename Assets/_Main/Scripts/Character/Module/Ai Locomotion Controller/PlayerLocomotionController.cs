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
    public class PlayerLocomotionController : MonoBehaviour, ILocomotionController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Vector2 MoveInput 
        { 
            get; private set; 
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        //  Fields ----------------------------------------

        private NavMeshAgent _navMeshAgent;
        private PlayerAgent _model;

        private float _speed;

        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = GetComponent<PlayerAgent>();

                _speed = _model.WalkSpeed;
                _navMeshAgent = _model.BaseNavMeshAgent;

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


        public void Tick()
        {
            ApplyMovement();
            ApplyRotation();
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
                _speed = _model.RunSpeed;
            };

            _model.Controls.Character.Run.canceled += context =>
            {
                _speed = _model.WalkSpeed;
            };
        }

        public void ApplyRotation()
        {
            if (_model.MoveDirection == Vector3.zero)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(_model.MoveDirection);

            desiredRotation.x = 0;
            desiredRotation.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _model.TurnSpeed * Time.deltaTime);
        }

        public void ApplyMovement()
        {
            _model.MoveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);

            if (_model.MoveDirection.magnitude > 0)
            {
                _navMeshAgent.Move(_model.MoveDirection * _speed * Time.deltaTime);
            }
        }

        
        //  Event Handlers --------------------------------

    }

}
