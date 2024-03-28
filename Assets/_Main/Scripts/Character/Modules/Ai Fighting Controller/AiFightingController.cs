using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class AiFightingController : MonoBehaviour, ITickable
    {
        private PlayerAgent agent;

        public void Awake()
        {
            agent = GetComponent<PlayerAgent>();
        }

        public void Attack()
        {
            Vector3 dirTarget = agent.TargetAgent.transform.position - transform.position;
            transform.forward = dirTarget;
            Debug.Log("Attack");
        }

        public void Tick()
        {
            Attack();
        }
    }

}
