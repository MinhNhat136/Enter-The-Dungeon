using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public static partial class AnimatorParameters
    {
        public static int InputHorizontal    = Animator.StringToHash("Horizontal");
        public static int InputVertical      = Animator.StringToHash("Vertical");
        public static int Hit_Horizontal     = Animator.StringToHash("Hit_Horizontal");
        public static int Hit_Vertical       = Animator.StringToHash("Hit_Vertical");
        public static int Dodge_Horizontal   = Animator.StringToHash("Dodge_Horizontal");
        public static int Dodge_Vertical     = Animator.StringToHash("Dodge_Vertical");
    }

    public static partial class AnimatorStates
    {
        public static int Summon             = Animator.StringToHash("summon");
        public static int Locomotion         = Animator.StringToHash("locomotion");
        public static int Roll               = Animator.StringToHash("roll");
        public static int Break              = Animator.StringToHash("break");
        public static int Knock_Down         = Animator.StringToHash("knock_down");
    }

    public interface IAgentAnimator : IInitializableWithBaseModel<BaseAgent>, ITickable
    {
        public void ApplyAnimator();
    }
}
