using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    public sealed class AiDashController : IInitializableWithBaseModel<AiMotorController>
    {
        public bool IsInitialized => _isInitialized;
        public AiMotorController Model => _model;

        public float Distance { get; set; }
        public LayerMask ColliderLayer { get; set; }

        private bool _isInitialized;
        private AiMotorController _model;
        private NavMeshAgent _navMeshAgent;

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

        private Vector3 SetDestinationForDash()
        {
            Vector3 dashDirection = _model.transform.forward;
            Vector3 targetPosition = _model.transform.position + dashDirection * Distance;

            // Check if the target position is blocked
            if (Physics.Raycast(_model.transform.position, dashDirection, out var hit, Distance, ColliderLayer))
            {
                // If blocked, try to find a position above the obstacle
                targetPosition = hit.point + Vector3.up;
            }

            return targetPosition;
        }

        public void Dash()
        {
            Vector3 targetPosition = SetDestinationForDash();
            _navMeshAgent.SetDestination(targetPosition);
            _navMeshAgent.speed *= 10; // Increase speed for dashing
            _navMeshAgent.acceleration *= 10;
        }

        public void StopDash()
        {
            _navMeshAgent.speed /= 10; // Reset speed after dashing
            _navMeshAgent.acceleration /= 10;
        }
    }
}