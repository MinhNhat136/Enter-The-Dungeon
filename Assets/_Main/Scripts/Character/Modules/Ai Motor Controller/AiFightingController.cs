using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class AiFightingController : MonoBehaviour, ITickable
    {
        private PlayerAgent agent;
        private Animator animator; 

        public void Awake()
        {
            agent = GetComponent<PlayerAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        public void Attack()
        {
            Vector3 dirTarget = agent.TargetAgent.transform.position - transform.position;
            transform.forward = dirTarget;
            animator.Play("shotgun_charge_release");
            Debug.Log("Attack");
        }

        public void Tick()
        {
            Attack();
        }
    }

}
