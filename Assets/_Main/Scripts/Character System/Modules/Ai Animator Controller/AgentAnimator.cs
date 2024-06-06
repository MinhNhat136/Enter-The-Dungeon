using System;
using Atomic.AbilitySystem;
using Atomic.Core.Interface;
using Sirenix.OdinInspector;
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
        public static readonly int Attack1 = Animator.StringToHash("attack_1");
        public static readonly int Attack2 = Animator.StringToHash("attack_2");
        public static readonly int Attack3 = Animator.StringToHash("attack_3");
        public static readonly int Die = Animator.StringToHash("die");
        public static readonly int RangedAttackChargeStart = Animator.StringToHash("rangeAttack_charge_start");
        public static readonly int RangedAttackChargeRelease = Animator.StringToHash("rangeAttack_charge_release");
    }

    public abstract class AgentAnimator : SerializedMonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        public Animator Animator { get; protected set; }

        public bool IsCurrentAnimationComplete(int layerIndex = 0, float percentage = 1)
        {
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (stateInfo.normalizedTime >= percentage)
            {
                return true;
            }

            return false;
        }

        public bool IsNextAnimationComplete(int layerIndex = 0, float percentage = 1f)
        {
            AnimatorStateInfo nextStateInfo = Animator.GetNextAnimatorStateInfo(layerIndex);

            if (Animator.IsInTransition(layerIndex))
            {
                if (nextStateInfo.normalizedTime >= percentage)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCurrentAnimationComplete(string animationName, int layerIndex = 0, float percentage = 1)
        {
            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);

            if (currentState.IsName(animationName))
            {
                if (currentState.normalizedTime >= percentage)
                {
                    return true;
                }
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

        public virtual void ApplyMovementAnimation()
        {
            Animator.SetFloat(AnimatorParameters.InputHorizontal, Model.MotorController.MoveSpeed,
                .1f, Time.deltaTime);
            Animator.SetFloat(AnimatorParameters.InputVertical, Model.MotorController.MoveSpeed,
                .1f, Time.deltaTime);
        }

        public virtual void StopMovementAnimation()
        {
            Animator.SetFloat(AnimatorParameters.InputHorizontal, 0, .1f, Time.deltaTime);
            Animator.SetFloat(AnimatorParameters.InputVertical, 0, .1f, Time.deltaTime);
        }

        public virtual void ApplyAppear()
        {
            Animator.Play(AnimatorStates.Appear);
        }

        public virtual void ApplyRise()
        {
            Animator.CrossFade(AnimatorStates.Rise, 0.25f);
        }

        public virtual void ApplyLocomotionAnimation()
        {
            Animator.CrossFade("locomotion", 0.25f);
        }

        public virtual void ApplyHitAnimation()
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

        public virtual void ApplyStunAnimation()
        {
            Animator.CrossFade("stun", 0.2f);
        }

        public virtual void ApplyBreakAnimation()
        {
            Animator.CrossFade("break", 0.2f);
        }

        public virtual void ApplyKnockDownAnimation()
        {
            Animator.CrossFade("knock_down", 0.2f);
        }
    }
}