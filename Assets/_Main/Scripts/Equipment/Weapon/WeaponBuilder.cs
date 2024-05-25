using System;
using System.Collections.Generic;
using Atomic.Character;
using Atomic.Core;
using Atomic.Damage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    public class WeaponBuilder : SerializedMonoBehaviour
    {
        public event Action<CombatMode> OnActivated;

        [Header("ENUM", order = 0)]
        public CombatMode combatMode; 
        public WeaponType weaponType;
        
        [Header("WEAPON", order = 1)]
        public GameObject weaponPrefab;
        public Vector3 weaponSpawnPoint;
        public Vector3 weaponSpawnRotation;
        
        [Header("PARAMETER", order = 2)]
        public float range;

        [Header("DAMAGE HIT EFFECT", order = 3)]
        public List<DamagePassiveEffectSo> effectBuilders = new(8);
        
        [Header("METABOLISM WEIGHT", order = 5)]
        public MinMaxFloat damageWeight;
        public MinMaxFloat speedWeight;
        public MinMaxFloat distanceWeight;
        public MinMaxFloat radiusWeight;
        public MinMaxFloat areaOfEffectDistance;
        public MinMaxFloat forceWeight;
        
        protected BaseAgent Owner { get; private set; }
        public GameObject Model { get; private set; }
        private ObjectPool<EffectPopupAnimation> _effectPopupPool; 
        
        public virtual void Attach(Transform parent, BaseAgent owner)
        {
            Owner = owner;
            Model = Instantiate(weaponPrefab, parent, true);
            Model.transform.localPosition = weaponSpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(weaponSpawnRotation);
            CreatePopupEffect();
            DeActivate();
        }

        public virtual void Detach()
        {
            Owner = null;
            Destroy(Model);
        }
        
        public void Activate()
        {
            Model.SetActive(true);
            OnActivated?.Invoke(combatMode);
        }

        public void DeActivate()
        {
            Model.SetActive(false);
        }

        private void CreatePopupEffect()
        {
            foreach (var builder in effectBuilders)
            {
                builder.CreatePopupPool();
            }
        }
    }
}
