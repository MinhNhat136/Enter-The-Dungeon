using System;
using Atomic.Character;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    public class WeaponScriptableObject : ScriptableObject, ICloneable
    {

        public event Action<CombatMode> OnActivated; 
        
        [HideInInspector] public bool IsAttach;
        [HideInInspector] public bool IsActivated; 
        
        [Header("ENUM", order = 0)]
        public CombatMode CombatMode; 
        public WeaponType WeaponType;
        
        [Header("WEAPON", order = 1)]
        public string Name;
        public GameObject WeaponPrefab;
        public Vector3 WeaponSpawnPoint;
        public Vector3 WeaponSpawnRotation;
        
        protected BaseAgent Owner;
        protected GameObject Model;


        public virtual void Attach(Transform parent, BaseAgent owner)
        {
            IsAttach = true;
            Owner = owner;
            Model = Instantiate(WeaponPrefab, parent, true);
            Model.transform.localPosition = WeaponSpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(WeaponSpawnRotation);
            DeActivate();
        }

        public virtual void Detach()
        {
            IsAttach = false;
            IsActivated = false;
            Owner = null;
            Destroy(Model);
        }

        public virtual void Activate()
        {
            IsActivated = true;
            Model.SetActive(true);
            OnActivated?.Invoke(CombatMode);
        }

        public virtual void DeActivate()
        {
            IsActivated = false;
            Model.SetActive(false);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
    
}
