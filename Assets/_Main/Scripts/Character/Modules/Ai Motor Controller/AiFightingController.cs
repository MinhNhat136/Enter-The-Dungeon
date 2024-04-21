using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public class AiFightingController : MonoBehaviour, ITickable
    {
        private PlayerAgent _agent;
        private Animator _animator; 

        public void Awake()
        {
            _agent = GetComponent<PlayerAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Attack()
        {
            Vector3 dirTarget = _agent.TargetAgent.transform.position - transform.position;
            transform.forward = dirTarget;
            _animator.Play("shotgun_charge_release");
            Debug.Log("Attack");
        }

        public void Tick()
        {
            Attack();
        }
    }

}
