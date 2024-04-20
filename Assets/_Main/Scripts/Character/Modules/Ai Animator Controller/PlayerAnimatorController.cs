using Atomic.Character.Model;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAnimatorController : MonoBehaviour, IAgentAnimator, ICharacterActionTrigger, IActionEffectTrigger
    {

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized => _isInitialized;
        public BaseAgent Model => _model;

        //  Fields ----------------------------------------
        private BaseAgent _model;
        private Animator _animator;

        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;
                _animator = GetComponentInChildren<Animator>();
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
        

        //  Unity Methods   -------------------------------

        //  Other Methods ---------------------------------
        public void ApplyMovementAnimation()
        {
            float xVelocity = Vector3.Dot(_model.MotorController.MoveDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(_model.MotorController.MoveDirection.normalized, transform.forward);

            _animator.SetFloat(AnimatorParameters.InputHorizontal, xVelocity, .1f, Time.deltaTime);
            _animator.SetFloat(AnimatorParameters.InputVertical, zVelocity, .1f, Time.deltaTime);

        }

        public void StopMovementAnimation()
        {
            _animator.SetFloat(AnimatorParameters.InputHorizontal, 0, .1f, Time.deltaTime);
            _animator.SetFloat(AnimatorParameters.InputVertical, 0, .1f, Time.deltaTime);
        }

        public void ApplyRollAnimation()
        {
            _animator.CrossFade(AnimatorStates.Roll, 0.05f);
        }


        public void ApplyRangedAttack_Charge_Start_Animation()
        {
            _animator.CrossFade(AnimatorStates.RangedAttack_Charge_Start, 0.05f); }

        public void ApplyRangedAttack_Charge_Release_Animation()
        {
            _animator.Play(AnimatorStates.RangedAttack_Charge_Release);
            _animator.SetBool(AnimatorParameters.IsRangedAttack, false);
        }

        public void ApplySummonAnimation()
        {
        }

        public void ApplyBreakAnimation()
        {
            
        }

        public void ApplyKnockDownAnimation()
        {

        }
        
        //  Event Handlers --------------------------------

        public void OnCharacterActionTrigger(CharacterActionType actionType)
        {
        }

        public void OnActionEffectTrigger(ActionEffectType effectType)
        {
        }
    }
}

