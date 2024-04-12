using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    public sealed class AiRollController : MonoBehaviour, IInitializableWithBaseModel<AiMotorController>
    {
        public NavMeshAgent navMeshAgent;

        public bool IsInitialized => _isInitialized;
        public AiMotorController Model => _model; 

        private bool _isInitialized;
        private AiMotorController _model; 
        public void Initialize(AiMotorController model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;
                
                _model.ActionTriggers.Add(CharacterActionType.StopRoll, OnStopRoll);
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
        
        public void Roll()
        {
            _model.IsGrounded = false;
            Vector3 rollDirection = transform.forward;

            Vector3 targetPosition = transform.position + rollDirection * 5f; 
            navMeshAgent.SetDestination(targetPosition);

        }

        private void OnStopRoll()
        {
            Debug.Log("here");
            _model.IsGrounded = true;
            _model.IsRolling = false;
        }
    }
}

