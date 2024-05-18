using System;
using UnityEngine;

namespace Atomic.Character
{
    public class ZombieControls : MonoBehaviour
    {
        public event Action<Vector2> MovementInput;
        public event Action AttackInput;

        private Transform _playerTransform;
        private GameObject _playerObject;
        
        [SerializeField] 
        private float rangeAttack;

        [SerializeField] private float _timeWandering;
        private BaseTargetingAgentController _targetingAgentController;
        
        private void Start()
        {
            _playerObject = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = _playerObject.transform;
        }


        public void Update()
        {
            MoveDecision();
        }

        private void MoveDecision()
        {
            var directionToPlayer = (_playerTransform.position - transform.position);
            var moveInput = new Vector2(directionToPlayer.x, directionToPlayer.z);
            MovementInput?.Invoke(moveInput);
        }


    }
}
