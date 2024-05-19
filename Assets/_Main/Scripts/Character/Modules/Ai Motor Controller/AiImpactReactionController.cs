using System;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The AiImpactReactionController class handles the reaction of an AI character to various levels of impact forces.
    /// It classifies the impacts into different categories (Hit, Break, Stun, Knockdown) based on their magnitudes 
    /// and applies corresponding reactions, adjusting the character's condition and invoking appropriate response times.
    /// </summary>
    public class AiImpactReactionController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }

        //  Fields ----------------------------------------
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
        
        //  Initialization  -------------------------------
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
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        private void ApplyImpact(Vector3 impactForce)
        {
            _timeImpact = Time.time;
            Model.ForceHit = impactForce;
            if (impactForce.magnitude > hitThreshold && impactForce.magnitude <= breakThreshold)
            {
                Reaction(AgentCondition.Hit, hitReactionTime);
                return;
            }

            if (impactForce.magnitude > breakThreshold && impactForce.magnitude <= stunThreshold)
            {
                Reaction(AgentCondition.Break, breakReactionTime);

                return;
            }

            if (impactForce.magnitude > stunThreshold && impactForce.magnitude <= knockdownThreshold)
            {
                Reaction(AgentCondition.Stun, stunReactionTime);
                return;
            }

            if (impactForce.magnitude > knockdownThreshold)
            {
                Reaction(AgentCondition.Knockdown, knockdownReactionTime);
            }
        }
        
        private void NormalReaction()
        {
            Model.AgentCondition = AgentCondition.Normal;
        }

        private void Reaction(AgentCondition condition, float time)
        {
            if (ShouldResetImpact(Model.AgentCondition, condition))
            {
                ResetImpact = true;
                Model.AgentCondition = condition;
                CancelInvoke(nameof(NormalReaction));
                Invoke(nameof(NormalReaction), time);
            }
        }

        private bool ShouldResetImpact(AgentCondition currentCondition, AgentCondition nextCondition)
        {
            if (Math.Abs(_lastTimeImpact - _timeImpact) > 0.05f && (int)nextCondition >= (int)currentCondition)
            {
                return true;
            }
            return false;
        }
        //  Event Handlers --------------------------------
        
    }
}