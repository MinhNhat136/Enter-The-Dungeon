using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character.Module
{
    /// <summary>
    /// OnAnimatorMove Event Sender 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventsListenerWithRootMotion : AnimatorEventsListener
    {
        public System.Action AnimatorMoveEvent;

        public void Awake()
        {
            IAnimatorMoveReceive[] animatorMoves = GetComponentsInParent<IAnimatorMoveReceive>();
            for (int i = 0; i < animatorMoves.Length; i++)
            {
                var receiver = animatorMoves[i];
                AnimatorMoveEvent += () =>
                {
                    if (receiver.enabled)
                    {
                        receiver.OnAnimatorMoveEvent();
                    }
                };
            }
        }

        public void OnAnimatorMove()
        {
            AnimatorMoveEvent?.Invoke();
        }
    }

}
