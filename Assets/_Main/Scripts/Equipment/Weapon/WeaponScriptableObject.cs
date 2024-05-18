using System;
using System.Collections.Generic;
using Atomic.Character;
using Atomic.Core;
using Atomic.Damage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;

namespace Atomic.Equipment
{
    public class WeaponScriptableObject : SerializedScriptableObject, ICloneable
    {
        public event Action<CombatMode> OnActivated; 
        
        [HideInInspector] public bool isAttach;
        [HideInInspector] public bool isActivated; 
        
        [Header("ENUM", order = 0)]
        public CombatMode combatMode; 
        public WeaponType weaponType;
        
        [Header("WEAPON", order = 1)]
        public string Name;
        public GameObject weaponPrefab;
        public Vector3 weaponSpawnPoint;
        public Vector3 weaponSpawnRotation;
        
        [Header("PARAMETER", order = 2)]
        public float range;

        [Header("DAMAGE HIT EFFECT", order = 3)]
        public List<DamagePassiveEffectSo> effectBuilders = new(8);
        
        [Header("DAMAGE HIT FORCE", order = 4)]
        public DamageForce damageForce;
        
        [Header("METABOLISM WEIGHT", order = 5)]
        public MinMaxFloat damageWeight;
        public MinMaxFloat chanceCriticalWeight;
        public MinMaxFloat speedWeight;
        public MinMaxFloat distanceWeight;
        public MinMaxFloat radiusWeight;
        public MinMaxFloat areaOfEffectDistance;
        public MinMaxFloat gravityDownAcceleration;
        public MinMaxFloat forceWeight;
        
        protected BaseAgent Owner;
        protected GameObject Model;
        private ObjectPool<EffectPopupAnimation> _effectPopupPool; 
        
        public virtual void Attach(Transform parent, BaseAgent owner)
        {
            isAttach = true;
            Owner = owner;
            Model = Instantiate(weaponPrefab, parent, true);
            Model.transform.localPosition = weaponSpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(weaponSpawnRotation);
            CreatePopupEffect();
            DeActivate();
        }

        public virtual void Detach()
        {
            isAttach = false;
            isActivated = false;
            Owner = null;
            Destroy(Model);
        }

        private void CreatePopupEffect()
        {
            foreach (var builder in effectBuilders)
            {
                builder.CreatePopupPool();
            }
        }

        public void Activate()
        {
            isActivated = true;
            Model.SetActive(true);
            OnActivated?.Invoke(combatMode);
        }

        public void DeActivate()
        {
            isActivated = false;
            Model.SetActive(false);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
    
}
