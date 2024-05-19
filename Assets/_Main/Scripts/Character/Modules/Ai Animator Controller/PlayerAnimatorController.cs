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
        public int currentMeleeCombo;
        
        [field: SerializeField]
        private Dictionary<WeaponType, RuntimeAnimatorController> _animatorMatchWithWeapons = new(16);

        private MeleeCombatController _meleeCombatController;
        
        //  Initialization  -------------------------------
        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
        

        //  Unity Methods   -------------------------------

        //  Other Methods ---------------------------------
        public void ApplyLocomotionAnimation()
        {
            
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

        public void ApplyRangedAttack_Charge_Start_Animation() => Animator.CrossFade(AnimatorStates.RangedAttackChargeStart, 0.05f);

        public void ApplyRangedAttack_Charge_Release_Animation() => Animator.CrossFade(AnimatorStates.RangedAttackChargeRelease, 0.5f);

        public bool ApplyMeleeAttack(int combo)
        {
            int targetAnimationHash = AnimatorStates.GetMeleeAttackComboHash(combo);

            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
            bool isInTransition = Animator.IsInTransition(0);

            if (currentState.shortNameHash == targetAnimationHash || isInTransition)
            {
                return false;
            }

            Animator.Play(targetAnimationHash);
            return true;
        }

        public void ApplyRiseAnimation()
        {
            
        }
        
        
        private void ResetCurrentMeleeCombo()
        {
            currentMeleeCombo = 0;
        }

        public void ApplySummonAnimation()
        {
            
        }

        public void ApplyHitAnimation()
        {
            
        }
        

        public void ApplyBreakAnimation()
        {
            
        }

        public void ApplyStunAnimation()
        {
            
        }

        public void ApplyKnockDownAnimation()
        {

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

