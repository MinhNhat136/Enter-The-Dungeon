using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Character.Module
{
    public class AiHealth : MonoBehaviour
    {
        public float MaxHealth = 10f;

        public UnityAction<float, GameObject> OnDecreased;
        public UnityAction<float> OnIncreased;
        public UnityAction OnZero;

        public float CurrentHealth { get; set; }
        public bool Invincible { get; set; }

        bool m_IsDead;

        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void Increase(float healAmount)
        {
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnIncreased?.Invoke(trueHealAmount);
            }
        }

        public void Decrease(float damage, GameObject damageSource)
        {
            if (Invincible)
                return;

            float healthBefore = CurrentHealth;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            float trueDamageAmount = healthBefore - CurrentHealth;
            if (trueDamageAmount > 0f)
            {
                OnDecreased?.Invoke(trueDamageAmount, damageSource);
            }

            HandleDeath();
        }

        public void Kill()
        {
            CurrentHealth = 0f;

            OnDecreased?.Invoke(MaxHealth, null);

            HandleDeath();
        }

        void HandleDeath()
        {
            if (m_IsDead)
                return;

            if (CurrentHealth <= 0f)
            {
                m_IsDead = true;
                OnZero?.Invoke();
            }
        }
    }
}