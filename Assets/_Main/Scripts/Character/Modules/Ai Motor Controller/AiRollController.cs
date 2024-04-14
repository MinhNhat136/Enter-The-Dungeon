using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
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
        public float Distance { get; set; }
        public LayerMask ColliderLayer { get; set; }

        private bool _isInitialized;

        private AiMotorController _model;
        private NavMeshAgent _navMeshAgent;

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
        private Vector3 SetDestinationForRoll()
        {
            Vector3 rollDirection = _model.transform.forward;

            if (Physics.Raycast(_model.transform.position, rollDirection, out _rayCastHit, rollDirection
                    .magnitude * Distance, ColliderLayer))
            {
                if (NavMesh.SamplePosition(_rayCastHit.point, out _navMeshHit, 10, NavMesh.AllAreas))
                {
                    return _navMeshHit.position;
                }
            }
            return _model.transform.position + rollDirection * Distance;
        }
        
        public void Roll()
        {
            Debug.Log("roll");
            Vector3 targetPosition = SetDestinationForRoll();
            _navMeshAgent.SetDestination(targetPosition);
        }

        //  Event Handlers --------------------------------
        private void OnStopRoll()
        {
        }
    }
}