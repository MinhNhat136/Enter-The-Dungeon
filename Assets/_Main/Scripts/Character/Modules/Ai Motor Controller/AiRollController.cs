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
    public sealed class AiRollController : MonoBehaviour, IInitializableWithBaseModel<AiMotorController>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        public bool IsInitialized => _isInitialized;
        public AiMotorController Model => _model;

        //  Fields ----------------------------------------
        [SerializeField] private float distance;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private Color debugLineColor = Color.red;
        [SerializeField] private float debugLineDuration = 1f;

        private bool _isInitialized;

        private NavMeshAgent _navMeshAgent;
        private AiMotorController _model;

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
                _model.ActionTriggers.Add(CharacterActionType.StopRoll, OnStopRoll);
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
            Vector3 rollDirection = transform.forward;

            if (Physics.Raycast(transform.position, rollDirection, out _rayCastHit, rollDirection
                    .magnitude * distance, obstacleLayer))
            {
                if (NavMesh.SamplePosition(_rayCastHit.point, out _navMeshHit, 10, NavMesh.AllAreas))
                {
#if UNITY_EDITOR
                    DrawDebugLine(transform.position, _navMeshHit.position);
#endif
                    return _navMeshHit.position;
                }
            }
#if UNITY_EDITOR
            DrawDebugLine(transform.position, transform.position + rollDirection * distance);
#endif
            return transform.position + rollDirection * distance;
        }

#if UNITY_EDITOR
        private void DrawDebugLine(Vector3 startPoint, Vector3 endPoint)
        {
            Debug.DrawLine(startPoint, endPoint, debugLineColor, debugLineDuration);
        }
#endif

        public void Roll()
        {
            _model.IsGrounded = false;

            Vector3 targetPosition = SetDestinationForRoll();
            _navMeshAgent.SetDestination(targetPosition);
        }

        //  Event Handlers --------------------------------
        private void OnStopRoll()
        {
            _model.IsGrounded = true;
            _model.IsRolling = false;
        }
    }
}