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
    public class PlayerAnimatorController : MonoBehaviour, IAnimatorController, IInitializableWithBaseModel<PlayerAgent>, IAnimatorStateInfoController
    {
        //  Statics ---------------------------------------
        #region Animator Parameter
        public static int Horizontal        = Animator.StringToHash("Horizontal");
        public static int Vertical          = Animator.StringToHash("Vertical");
        public static int Hit_Horizontal    = Animator.StringToHash("Hit_Horizontal");
        public static int Hit_Vertical      = Animator.StringToHash("Hit_Vertical");
        public static int Dodge_Horizontal  = Animator.StringToHash("Dodge_Horizontal");
        public static int Dodge_Vertical    = Animator.StringToHash("Dodge_Vertical");
        #endregion

        #region Animation State Name
        public static int Summon            = Animator.StringToHash("summon");
        public static int Locomotion        = Animator.StringToHash("locomotion");
        public static int Roll              = Animator.StringToHash("roll");
        public static int Break             = Animator.StringToHash("break");
        public static int Knock_Down        = Animator.StringToHash("knock_down");
        #endregion

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public bool IsSummon { get; private set; }
        public bool IsDied { get; private set; }
        public bool IsRolling { get; private set; }
        public bool IsKnockDown { get; private set; }
        public bool IsStunned { get; private set; }



        //  Fields ----------------------------------------
        private PlayerAgent _model;
        private Animator _animator;
        public PlayerAgent Model
        {
            get { return _model; }
        }

        public AnimatorStateInfos animatorStateInfos => throw new System.NotImplementedException();

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

            _animator.SetFloat(Horizontal, xVelocity, .1f, Time.deltaTime);
            _animator.SetFloat(Vertical, zVelocity, .1f, Time.deltaTime);

        }

        public void ApplyRoll()
        {
            _animator.Play("roll");
        }
       
        public void ApplySummon()
        {

        }

        public void ApplyBreak()
        {

        }

        public void ApplyKnock_Down()
        {

        }


        //  Event Handlers --------------------------------

    }
}

