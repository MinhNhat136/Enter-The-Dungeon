using Atomic.Core.Interface;
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character
{
    public class AiHealth : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public float CurrentHealth { get; set; }
        public bool IsInitialized { get; private set; }
        public bool IsInvincible { get; set; }
        public bool IsDead { get; set; }

        public BaseAgent Model { get; private set; }


        //  Fields ----------------------------------------
        public float maxHealth;


        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
                CurrentHealth = maxHealth;
                IsDead = false;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void Increase(float healAmount)
        {
            if (CurrentHealth >= maxHealth)
            {
                CurrentHealth = maxHealth;
                return;
            }

            CurrentHealth += healAmount;
        }

        public void Decrease(float damage)
        {
            if (CurrentHealth <= 0)
            {
                return;
            }

            CurrentHealth -= damage;
        }

        public void Revive()
        {
            CurrentHealth = maxHealth;
        }

        public void Kill()
        {
            CurrentHealth = 0f;
            HandleDeath();
        }

        void HandleDeath()
        {
            if (IsDead)
                return;

            if (CurrentHealth <= 0f)
            {
                IsDead = true;
            }
        }


        //  Event Handlers --------------------------------
    }
}