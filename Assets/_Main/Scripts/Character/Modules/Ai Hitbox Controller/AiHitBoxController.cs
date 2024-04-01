using Atomic.Character.Model;
using Atomic.Core.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiHitBoxController : MonoBehaviour, IDamageable, IInitializableWithBaseModel<BaseAgent>, ITickable
    {
        //  Events ----------------------------------------
        public event Action _onReceiveDamage;
        public event Action _onHitWhileInvulnerable;
        public event Action _onBecomeVulnerable;
        public event Action _onResetDamage;

        //  Properties ------------------------------------
        public Collider[] Colliders
        {
            get; set;
        }

        public bool IsInvulnerable
        {
            get; set;
        }

        public float InvulnerabiltyTime 
        { 
            get; set; 
        }
        public float HitAngle
        {
            get; set; 
        }

        public float HitForwardRotation
        {
            get; set;
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public BaseAgent Model 
        { 
            get { return _model; } 
        }

        public Dictionary<PassiveEffect, IPassiveEffect> PassiveEffects => _passiveEffects; 

        public event Action OnReceiveDamage
        {
            add { _onReceiveDamage += value; }
            remove { _onReceiveDamage -= value; }
        }

        public event Action OnHitWhileInvulnerable
        {
            add { _onHitWhileInvulnerable += value; }
            remove { _onHitWhileInvulnerable -= value; }
        }

        public event Action OnBecomeVulnerable
        {
            add { _onBecomeVulnerable += value; }
            remove { _onBecomeVulnerable -= value; }
        }

        public event Action OnResetDamage
        {
            add { _onResetDamage += value; }
            remove { _onResetDamage -= value; }
        }

        //  Fields ----------------------------------------
        private AiHealth health;
        private BaseAgent _model;
        private bool _isInitialized;
        protected float _timeSinceLastHit = 0.0f;
        private Dictionary<PassiveEffect, IPassiveEffect> _passiveEffects = new();
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                this.health = model.HealthController;
                ResetDamage();
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new System.Exception("AiHit Box Controller not initialized");
            }
        }


        //  Unity Methods   -------------------------------
        public void Tick()
        {
            if (IsInvulnerable)
            {
                _timeSinceLastHit += Time.deltaTime;
                if (_timeSinceLastHit > InvulnerabiltyTime)
                {
                    _timeSinceLastHit = 0.0f;
                    IsInvulnerable = false;
                    _onBecomeVulnerable.Invoke();
                }
            }
        }

        //  Other Methods ---------------------------------
        public void ApplyDamage(DamageMessage data)
        {
            if (health.CurrentHealth <= 0)
            {
                return;
            }

            if (IsInvulnerable)
            {
                _onHitWhileInvulnerable.Invoke();
                return;
            }

            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(HitForwardRotation, transform.up) * forward;

            Vector3 positionToDamager = data.damageSource - transform.position;
            positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

            if (Vector3.Angle(forward, positionToDamager) > HitAngle * 0.5f)
                return;

            IsInvulnerable = true;
            health.Decrease(data.amount);

            _onReceiveDamage.Invoke();
        }

        public void ApplyPassiveEffect(PassiveEffect effect)
        {
            if (!_passiveEffects.ContainsKey(effect)) 
                return;

            _passiveEffects[effect].ApplyEffect();
        }

        public void ResetDamage()
        {
            IsInvulnerable = false;
            _timeSinceLastHit = 0.0f;
            _onResetDamage.Invoke();
        }

        public void SetTriggerState(bool enabled)
        {
            for (int index = 0; index <= Colliders.Length; index++)
            {
                Colliders[index].isTrigger = enabled;
            }
        }

        public void SetColliderState(bool enabled)
        {
            for (int index = 0; index <= Colliders.Length; index++)
            {
                Colliders[index].enabled = enabled;
            }
        }
        //  Event Handlers --------------------------------

    }

}
