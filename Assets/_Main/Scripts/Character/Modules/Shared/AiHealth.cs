using Atomic.Core.Interface;
using System;
using UnityEngine.Events;

namespace Atomic.Character
{
    [Serializable]
    public class AiHealth : IInitializableWithBaseModel<BaseAgent>
    {
        public float MaxHealth;

        public UnityAction OnZero;

        public float CurrentHealth { get; set; }
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        bool m_IsDead;

        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
                CurrentHealth = MaxHealth;
                m_IsDead = false;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        public void Increase(float healAmount)
        {
            CurrentHealth += healAmount;
        }

        public void Decrease(float damage)
        {
            CurrentHealth -= damage;
        }

        public void Kill()
        {
            CurrentHealth = 0f;
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