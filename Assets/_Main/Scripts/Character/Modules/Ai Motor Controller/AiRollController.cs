using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    public class AiRollController : MonoBehaviour, IAnimatorMoveReceive
    {
        bool isRolling;
        public NavMeshAgent navMeshAgent; 

        public virtual bool canRollAgain
        {
            get
            {
                return isRolling;
            }
        }
        public virtual void RollBehavior()
        {
            if (navMeshAgent != null && navMeshAgent.enabled)
            {
                navMeshAgent.SetDestination(transform.position + transform.forward); 
            }
        }

        public void OnAnimatorMoveEvent()
        {
            transform.position = navMeshAgent.nextPosition;
        }
    }
}

