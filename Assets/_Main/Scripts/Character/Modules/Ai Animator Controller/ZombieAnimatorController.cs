using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class ZombieAnimatorController : AgentAnimator
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
        public void ApplyAppear()
        {
            
        }
        
        public void ApplyRise()
        {
            Animator.CrossFade(AnimatorStates.Rise, 0.25f);
        }
        
        public void ApplyLocomotionAnimation()
        {
            Animator.CrossFade("locomotion", 0.25f);
        }
        
        public void ApplyMovementAnimation()
        {
            Animator.SetFloat(AnimatorParameters.InputHorizontal, Model.MotorController.LocomotionController.MoveSpeed, .1f, Time.deltaTime);
            Animator.SetFloat(AnimatorParameters.InputVertical, Model.MotorController.LocomotionController.MoveSpeed, .1f, Time.deltaTime);
        }

        public void StopMovementAnimation()
        {
            Animator.SetFloat(AnimatorParameters.InputHorizontal, 0, .1f, Time.deltaTime);
            Animator.SetFloat(AnimatorParameters.InputVertical, 0, .1f, Time.deltaTime);
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
        
        public void ApplyHitAnimation()
        {
            float xHit = Vector3.Dot(Model.ForceHit, transform.right);
            float zHit = Vector3.Dot(Model.ForceHit, transform.forward);

            Animator.SetFloat(AnimatorParameters.HitHorizontal, xHit);
            Animator.SetFloat(AnimatorParameters.HitVertical, zHit);

            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.shortNameHash == AnimatorStates.HitReact)
            {
                if (currentState.normalizedTime < 1.0f)
                {
                    Animator.Play(AnimatorStates.HitReact, 0, 0.1f); 
                }
            }
            if (currentState.IsName("hit_loop"))
            {
                if (currentState.normalizedTime < 1.0f)
                {
                    Animator.Play("hit_loop", 0, 1.0f); 
                }
            }

            Animator.CrossFade(AnimatorStates.HitReact, 0.2f);
        }


        public void ApplyStunAnimation()
        {
            Animator.CrossFade("stun", 0.2f);
        }
        
        public void ApplyBreakAnimation()
        {
            Animator.CrossFade("break", 0.2f);
            
        }

        public void ApplyKnockDownAnimation()
        {
            Animator.CrossFade("knock_down", 0.2f);
        }
        
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
       
    }
    
}
