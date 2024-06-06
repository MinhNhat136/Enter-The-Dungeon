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
