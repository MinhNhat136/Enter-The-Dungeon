using Atomic.Character.Model;
using Atomic.Character.Module;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Player
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAnimatorController : MonoBehaviour, IAnimatorController, IInitializableWithBaseModel<PlayerAgent>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }


        //  Fields ----------------------------------------
        private PlayerAgent _model;
        private Animator _animator;
        public PlayerAgent Model
        {
            get { return _model; }
        }
        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize(PlayerAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;
                _animator = _model.BaseAnimator;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        //  Unity Methods   -------------------------------
        public void Tick()
        {
            ApplyAnimator();
        }

        //  Other Methods ---------------------------------
        public void ApplyAnimator()
        {
            float xVelocity = Vector3.Dot(_model.MoveDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(_model.MoveDirection.normalized, transform.forward);

            _animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
            _animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

            bool playRunAnimation = _model.IsRunning & _model.MoveDirection.magnitude > 0;
            _animator.SetBool("isRunning", playRunAnimation);
        }

        //  Event Handlers --------------------------------

    }
}

