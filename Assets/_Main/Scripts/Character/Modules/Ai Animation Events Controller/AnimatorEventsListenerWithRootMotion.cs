using System.Linq;
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
        private System.Action _animatorMoveEvent;

        public new void Awake()
        {
            base.Awake();
            IAnimatorMoveReceive[] animatorMoves = GetComponentsInParent<IAnimatorMoveReceive>();
            if (!animatorMoves.Any()) return;
            foreach (var receiver in animatorMoves)
            {
                _animatorMoveEvent += () =>
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
            _animatorMoveEvent?.Invoke();
        }
        

    }

}
