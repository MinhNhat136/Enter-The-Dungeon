using Atomic.Core.Interface;
using Doozy.Runtime.Reactor;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Controls rolling behavior for an AI character, managing initialization, rolling actions, and stopping the roll.
    /// </summary>
    public sealed class AiRollController : IInitializableWithBaseModel<AiMotorController>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        public bool IsInitialized => _isInitialized;
        public AiMotorController Model => _model;

        //  Fields ----------------------------------------
        public float BoostSpeedValue { get; set; }
        public LayerMask ColliderLayer { get; set; }

        private bool _isInitialized;

        private AiMotorController _model;
        private NavMeshAgent _navMeshAgent;
        
        private Vector3 _rollDirection;
        
        private RaycastHit _rayCastHit;
        private NavMeshHit _navMeshHit;
        //  Initialization  -------------------------------
        public void Initialize(AiMotorController model)
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
            throw new System.NotImplementedException();
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void BeginRoll()
        {
            _model.Model.NavmeshAgent.enabled = false;
            _rollDirection = _model.Model.modelTransform.forward;
        }

        public void Rolling()
        {
            if (Physics.Raycast(_model.transform.position, _rollDirection, out _rayCastHit, _model.Model.MovementSpeed * _model.LocomotionController.MoveSpeed * BoostSpeedValue * Time.deltaTime, ColliderLayer))
            {
                if (NavMesh.SamplePosition(_rayCastHit.point, out _navMeshHit, 0.1f, NavMesh.AllAreas))
                {
                    _model.Model.modelTransform.position = _navMeshHit.position;
                    return;
                }
            }

            if (NavMesh.SamplePosition(_model.Model.modelTransform.position + _model.Model.MovementSpeed * _model.LocomotionController.MoveSpeed * BoostSpeedValue * Time.deltaTime * _rollDirection, out _navMeshHit,
                    0.1f, NavMesh.AllAreas))
            {
                _model.Model.modelTransform.position += _model.Model.MovementSpeed * _model.LocomotionController.MoveSpeed * BoostSpeedValue * Time.deltaTime * _rollDirection;
            }
        }

        public void EndRoll()
        {
            _model.Model.NavmeshAgent.Warp(_model.Model.modelTransform.position);
            _model.Model.NavmeshAgent.enabled = true; 
        }
        
        

        //  Event Handlers --------------------------------
    }
}