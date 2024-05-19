using Atomic.Core.Interface;
using Atomic.Equipment;
using NodeCanvas.Tasks.Actions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Atomic.Character
{
    public static partial class AnimatorParameters
    {
        public static readonly int InputHorizontal = Animator.StringToHash("Horizontal");
        public static readonly int InputVertical = Animator.StringToHash("Vertical");

        public static readonly int HitHorizontal = Animator.StringToHash("Hit_Horizontal");
        public static readonly int HitVertical = Animator.StringToHash("Hit_Vertical");

        public static readonly int DodgeHorizontal = Animator.StringToHash("Dodge_Horizontal");
        public static readonly int DodgeVertical = Animator.StringToHash("Dodge_Vertical");
    }
    

    public static partial class AnimatorStates
    {
        public static readonly int Appear = Animator.StringToHash("appear");
        public static readonly int Rise = Animator.StringToHash("rise");
        public static readonly int Summon = Animator.StringToHash("summon");
        public static readonly int Locomotion = Animator.StringToHash("locomotion");
        public static readonly int Roll = Animator.StringToHash("roll");
        public static readonly int Break = Animator.StringToHash("break");
        public static readonly int Stun = Animator.StringToHash("stun");
        public static readonly int HitReact = Animator.StringToHash("hit_react");
        public static readonly int HitLoop = Animator.StringToHash("hit_loop");
        public static readonly int KnockDown = Animator.StringToHash("knock_down");
        public static readonly int MeleeAttack1 = Animator.StringToHash("meleeAttack_1");
        public static readonly int MeleeAttack2 = Animator.StringToHash("meleeAttack_2");
        public static readonly int MeleeAttack3 = Animator.StringToHash("MeleeAttack_3");
        public static readonly int Die = Animator.StringToHash("die");
        public static readonly int RangedAttackChargeStart = Animator.StringToHash("rangeAttack_charge_start");
        public static readonly int RangedAttackChargeRelease = Animator.StringToHash("rangeAttack_charge_release");
    }

    public abstract class AgentAnimator : SerializedMonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        protected Animator Animator { get; set; }
        
        public bool IsCurrentAnimationComplete( int layerIndex = 0)
        {
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                return true;
            }
            return false;
        }
        
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;

                Animator = GetComponentInChildren<Animator>();
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
    }
}