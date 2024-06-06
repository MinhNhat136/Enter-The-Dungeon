using System;
using Atomic.AbilitySystem;
using Atomic.Character;
using Atomic.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.Equipment
{
    public class WeaponBuilder : SerializedMonoBehaviour
    {
        public event Action<CombatMode> OnActivated;

        [Header("ENUM", order = 0)]
        public CombatMode combatMode; 
        public WeaponType weaponType;
        
        [Header("PARAMETER", order = 2)]
        public float range;

        [Header("METABOLISM WEIGHT", order = 5)]
        public MinMaxFloat damageWeight;
        public MinMaxFloat speedWeight;
        public MinMaxFloat distanceWeight;
        public MinMaxFloat radiusWeight;
        public MinMaxFloat areaOfEffectDistance;
        public MinMaxFloat forceWeight;
        
        
        protected BaseAgent Owner { get; private set; }
        
        public virtual void Attach(BaseAgent owner)
        {
            Owner = owner;
            DeActivate();
        }

        public virtual void Detach()
        {
            Owner = null;
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated?.Invoke(combatMode);
        }

        public void DeActivate()
        {
            gameObject.SetActive(false);
        }
    }
}
