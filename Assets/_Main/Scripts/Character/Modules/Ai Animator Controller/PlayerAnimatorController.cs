using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAnimatorController : MonoBehaviour, IAgentAnimator
    {

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }
        
        //  Fields ----------------------------------------
        private BaseAgent _model;
        private Animator _animator;
        public BaseAgent Model
        {
            get { return _model; }
        }

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

        protected virtual void OnDisable()
        {

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
        
        public void ApplyRollAnimation()
        {
            _animator.Play("roll");
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

    }
}

