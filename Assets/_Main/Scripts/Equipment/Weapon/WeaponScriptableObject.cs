using System;
using System.Collections.Generic;
using Atomic.Character;
using Atomic.Damage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    public class WeaponScriptableObject : ScriptableObject, ICloneable
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

        [Header("DAMAGE HIT EFFECT", order = 2)]
        public List<DamagePassiveEffect> damagePassiveEffects;

        [Header("DAMAGE HIT FORCE", order = 3)]
        public DamageForce damageForce;
        
        protected BaseAgent Owner;
        protected GameObject Model;
        
        public virtual void Attach(Transform parent, BaseAgent owner)
        {
            isAttach = true;
            Owner = owner;
            Model = Instantiate(weaponPrefab, parent, true);
            Model.transform.localPosition = weaponSpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(weaponSpawnRotation);
            DeActivate();
        }

        public virtual void Detach()
        {
            isAttach = false;
            isActivated = false;
            Owner = null;
            Destroy(Model);
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
