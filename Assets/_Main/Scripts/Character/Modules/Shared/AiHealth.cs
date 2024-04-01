using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Character.Module
{
    public class AiHealth : MonoBehaviour, IInitializable
    {
        public float MaxHealth = 10f;

        public UnityAction OnZero;

        public float CurrentHealth { get; set; }

        public bool IsInitialized
        {
            get {  return _isInitialized; }
        }

        bool m_IsDead;
        bool _isInitialized; 
        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
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