using Atomic.Character.Model;
using Atomic.Core.Interface;
using System;
using UnityEngine.Events;

namespace Atomic.Character.Module
{
    [Serializable]
    public class AiHealth : IInitializableWithBaseModel<BaseAgent>
    {
        public float MaxHealth = 10f;

        public UnityAction OnZero;

        public float CurrentHealth { get; set; }

        public bool IsInitialized
        {
            get {  return _isInitialized; }
        }



        public BaseAgent Model 
        { 
            get { return _model;  } 
        }

        bool m_IsDead;
        bool _isInitialized;
        BaseAgent _model; 

        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;
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