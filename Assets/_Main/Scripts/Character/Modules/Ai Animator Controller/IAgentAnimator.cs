using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public static partial class AnimatorParameters
    {
        public static int InputHorizontal       = Animator.StringToHash("Horizontal");
        public static int InputVertical         = Animator.StringToHash("Vertical");
        
        public static int Hit_Horizontal        = Animator.StringToHash("Hit_Horizontal");
        public static int Hit_Vertical          = Animator.StringToHash("Hit_Vertical");
        
        public static int Dodge_Horizontal      = Animator.StringToHash("Dodge_Horizontal");
        public static int Dodge_Vertical        = Animator.StringToHash("Dodge_Vertical");

        public static int IsRangedAttack        = Animator.StringToHash("IsRangedAttack");
        public static int IsMeleeAttack         = Animator.StringToHash("IsMeleeAttack");
    }

    public static partial class AnimatorStates
    {
        public static int Summon             = Animator.StringToHash("summon");
        public static int Locomotion         = Animator.StringToHash("locomotion");
        public static int Roll               = Animator.StringToHash("roll");
        public static int Break              = Animator.StringToHash("break");
        public static int Knock_Down         = Animator.StringToHash("knock_down");
        
        public static int RangedAttack_Charge_Start   = Animator.StringToHash("rangeAttack_charge_start");
        public static int RangedAttack_Charge_Release = Animator.StringToHash("rangeAttack_charge_release");
    }

    public interface IAgentAnimator : IInitializableWithBaseModel<BaseAgent>
    {
        
        
        public virtual void ApplyMovementAnimation()
        {
            
        }

        public virtual void StopMovementAnimation()
        {
            
        }
        
        public virtual void ApplyRangedAttack_Charge_Start_Animation()
        {
            
        }
        
        public virtual void ApplyRangedAttack_Charge_Release_Animation()
        {
            
        }

        public virtual void ApplyRollAnimation()
        {
            
        }

        public virtual void ApplyJumpAnimation()
        {
            
        }

        public virtual void ApplySummonAnimation()
        {
            
        }

        public virtual void ApplyBreakAnimation()
        {
            
        }

        public virtual void ApplyKnockDownAnimation()
        {
            
        }

    }
}
