using Atomic.Character;
using Atomic.Core.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiHitBoxController : MonoBehaviour, IDamageable, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------
        public event Action _onReceiveDamage;
        public event Action _onHitWhileInvulnerable;
        public event Action _onBecomeVulnerable;
        public event Action _onResetDamage;

        //  Properties ------------------------------------
        public Collider[] Colliders { get; set; }

        public bool IsInvulnerable { get; set; }

        public float InvulnerableTime { get; set; }
        public float HitAngle { get; set; }

        public float HitForwardRotation { get; set; }

        public bool IsInitialized => _isInitialized;

        public BaseAgent Model => _model;

        public Dictionary<PassiveEffect, IPassiveEffect> PassiveEffects { get; } = new();

        public Dictionary<CharacterActionType, Action> ActionTriggers { get; } = new();

        public event Action OnReceiveDamage
        {
            add => _onReceiveDamage += value;
            remove => _onReceiveDamage -= value;
        }

        public event Action OnHitWhileInvulnerable
        {
            add => _onHitWhileInvulnerable += value;
            remove => _onHitWhileInvulnerable -= value;
        }

        public event Action OnBecomeVulnerable
        {
            add => _onBecomeVulnerable += value;
            remove => _onBecomeVulnerable -= value;
        }

        public event Action OnResetDamage
        {
            add => _onResetDamage += value;
            remove => _onResetDamage -= value;
        }

        //  Fields ----------------------------------------
        [FormerlySerializedAs("_hitBoxConfig")] [SerializeField]
        private HitBoxConfig hitBoxConfig;

        private AiHealth _health;
        private BaseAgent _model;
        private bool _isInitialized;

        private float _timeSinceLastHit;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _model = model;
                _health = model.HealthController;
                Colliders = GetComponentsInChildren<Collider>();
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

        public void AssignHitBoxController()
        {
            hitBoxConfig.Assign(this);
        }

        //  Unity Methods   -------------------------------
        public void Tick()
        {
            if (IsInvulnerable)
            {
                _timeSinceLastHit += Time.deltaTime;
                if (_timeSinceLastHit > InvulnerableTime)
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
            if (_health.CurrentHealth <= 0)
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

            Vector3 posDamageSource = data.DamageSource - transform.position;
            posDamageSource -= transform.up * Vector3.Dot(transform.up, posDamageSource);

            if (Vector3.Angle(forward, posDamageSource) > HitAngle * 0.5f)
                return;

            IsInvulnerable = true;
            _health.Decrease(data.Amount);

            _onReceiveDamage.Invoke();
        }

        public void ApplyPassiveEffect(PassiveEffect effect)
        {
            if (!PassiveEffects.ContainsKey(effect))
                return;

            PassiveEffects[effect].ApplyEffect();
        }

        public void ResetDamage()
        {
            IsInvulnerable = false;
            _timeSinceLastHit = 0.0f;
            _onResetDamage?.Invoke();
        }

        public void SetTriggerState(bool enable)
        {
            foreach (var collider in Colliders)
            {
                collider.isTrigger = enable;
            }
        }

        private void SetColliderState(bool enable)
        {
            foreach (var collider in Colliders)
            {
                collider.enabled = enable;
            }
        }
        //  Event Handlers --------------------------------
    }
}