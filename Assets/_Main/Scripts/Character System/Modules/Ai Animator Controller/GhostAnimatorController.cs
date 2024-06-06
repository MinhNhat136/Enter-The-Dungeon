using UnityEngine;

namespace Atomic.Character
{
    public static partial class AnimatorStates
    {
        public static readonly int Dive = Animator.StringToHash("dive");

    }

    public class GhostAnimatorController : AgentAnimator
    {
        public void ApplyDiveAnimation()
        {
            Animator.CrossFade(AnimatorStates.Dive, 0.25f);
        }

        public bool IsDiveAnimationComplete(float percentage = 1f)
        {
            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);

            if (currentState.shortNameHash == AnimatorStates.Dive)
            {
                if (currentState.normalizedTime >= percentage)
                {
                    return true;
                }
            }

            return false;
        }
        
     
        public bool ApplyAttackAnimation()
        {
            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
            bool isInTransition = Animator.IsInTransition(0);

            if (currentState.IsName("attack_1") || isInTransition)
            {
                return false;
            }

            Animator.CrossFade("attack_1", 0.2f);
            return true;
        }
        
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
    }
    
}

