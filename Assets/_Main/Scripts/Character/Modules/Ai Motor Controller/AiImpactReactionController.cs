using System;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public class AiImpactReactionController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        public float hitThreshold;
        public float breakThreshold;
        public float stunThreshold;
        public float knockdownThreshold;

        public float hitReactionTime;
        public float breakReactionTime;
        public float stunReactionTime;
        public float knockdownReactionTime;

        public bool ResetImpact { get; set; } = false;
        private float _timeImpact;
        private float _lastTimeImpact;

        private AiImpactSensorController _impactSensor;

        private void ApplyImpact(Vector3 impactForce)
        {
            _timeImpact = Time.time;
            Model.ForceHit = impactForce;
            if (impactForce.magnitude > hitThreshold && impactForce.magnitude <= breakThreshold)
            {
                HitReaction();
                return;
            }

            if (impactForce.magnitude > breakThreshold && impactForce.magnitude <= stunThreshold)
            {
                BreakReaction();
                return;
            }

            if (impactForce.magnitude > stunThreshold && impactForce.magnitude <= knockdownThreshold)
            {
                StunReaction();
                return;
            }

            if (impactForce.magnitude > knockdownThreshold)
            {
                KnockdownReaction();
            }
        }


        private void NormalReaction()
        {
            Model.AgentCondition = AgentCondition.Normal;
        }

        private void HitReaction()
        {
            if (Math.Abs(_lastTimeImpact - _timeImpact) > 0.05f && Model.AgentCondition == AgentCondition.Hit)
            {
                ResetImpact = true;
                CancelInvoke(nameof(NormalReaction));
            }
            else Model.AgentCondition = AgentCondition.Hit;
            Invoke(nameof(NormalReaction), hitReactionTime);
        }

        private void StunReaction()
        {
            if (Math.Abs(_lastTimeImpact - _timeImpact) > 0.05f && Model.AgentCondition == AgentCondition.Stun)
            {
                ResetImpact = true;
                CancelInvoke(nameof(NormalReaction));
            }
            else Model.AgentCondition = AgentCondition.Stun;
            Invoke(nameof(NormalReaction), hitReactionTime);
        }

        private void KnockdownReaction()
        {
            Model.AgentCondition = AgentCondition.Knockdown;
            Invoke(nameof(NormalReaction), knockdownReactionTime);
        }

        private void BreakReaction()
        {
            Model.AgentCondition = AgentCondition.Break;
            Invoke(nameof(NormalReaction), breakReactionTime);
        }

        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }

        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;

                _impactSensor = Model.ImpactSensorController;
                _impactSensor.OnImpact += ApplyImpact;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
    }
}