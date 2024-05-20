using System.Collections.Generic;
using Atomic.Equipment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAnimatorController : AgentAnimator
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------

        [field: SerializeField]
        private Dictionary<WeaponType, RuntimeAnimatorController> _animatorMatchWithWeapons = new(16);

        private MeleeCombatController _meleeCombatController;

        //  Initialization  -------------------------------
        
        //  Unity Methods   -------------------------------

        //  Other Methods ---------------------------------
        public void ApplyLocomotionAnimation()
        {
            Animator.CrossFade(AnimatorStates.Locomotion, 0.2f);
        }

        public void ApplyMovementAnimation()
        {
            float xVelocity = Vector3.Dot(Model.MotorController.MoveDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(Model.MotorController.MoveDirection.normalized, transform.forward);

            Animator.SetFloat(AnimatorParameters.InputHorizontal, xVelocity, .1f, Time.deltaTime);
            Animator.SetFloat(AnimatorParameters.InputVertical, zVelocity, .1f, Time.deltaTime);
        }

        public void StopMovementAnimation()
        {
            Animator.SetFloat(AnimatorParameters.InputHorizontal, 0, .1f, Time.deltaTime);
            Animator.SetFloat(AnimatorParameters.InputVertical, 0, .1f, Time.deltaTime);
        }

        public void ApplyRollAnimation() => Animator.CrossFade(AnimatorStates.Roll, 0.05f);

        public void ApplyRangedAttack_Charge_Start_Animation() =>
            Animator.CrossFade(AnimatorStates.RangedAttackChargeStart, 0.05f);

        public void ApplyRangedAttack_Charge_Release_Animation() =>
            Animator.CrossFade(AnimatorStates.RangedAttackChargeRelease, 0.5f);

        public bool ApplyMeleeAttack(string animationName)
        {
            int targetAnimationHash = Animator.StringToHash(animationName);

            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
            bool isInTransition = Animator.IsInTransition(0);

            if (currentState.shortNameHash == targetAnimationHash || isInTransition)
            {
                return false;
            }

            Animator.CrossFade(targetAnimationHash, 0.25f);
            return true;
        }

        public void ApplyRiseAnimation()
        {
        }
        

        public void ApplySummonAnimation()
        {
        }

        public void ApplyHitAnimation()
        {
            float xHit = Vector3.Dot(Model.ImpactHit, transform.right);
            float zHit = Vector3.Dot(Model.ImpactHit, transform.forward);

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

        public void ApplyDieAnimation()
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != AnimatorStates.Die)
            {
                Animator.Play(AnimatorStates.Die);
            }
        }

        public void SwitchAnimator(WeaponType weaponType)
        {
            if (_animatorMatchWithWeapons.TryGetValue(weaponType,
                    out RuntimeAnimatorController runtimeAnimatorController))
            {
                Animator.runtimeAnimatorController = runtimeAnimatorController;
            }
        }

        //  Event Handlers --------------------------------
    }
}