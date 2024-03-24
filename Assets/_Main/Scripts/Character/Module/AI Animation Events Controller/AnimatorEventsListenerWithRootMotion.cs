using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventsListenerWithRootMotion : AnimatorEventsListener
    {
        private NavMeshAgent _agent;

        public void Awake()
        {
            _agent = GetComponentInParent<NavMeshAgent>();
        }

        public void OnAnimatorMove()
        {
            transform.parent.position = _agent.nextPosition;
        }
    }

}
