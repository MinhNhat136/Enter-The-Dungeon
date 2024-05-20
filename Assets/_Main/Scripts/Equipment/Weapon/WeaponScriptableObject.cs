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
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The WeaponScriptableObject class defines a weapon's configuration and behavior in the game.
    /// It includes properties for weapon attributes, methods for attaching/detaching the weapon to/from a character,
    /// and handles activation/deactivation of the weapon. It also manages damage effects and pop-up animations.
    /// </summary>
    public class WeaponScriptableObject : SerializedScriptableObject, ICloneable
    {
        //  Events ----------------------------------------
        public event Action<CombatMode> OnActivated; 


        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------
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
        public GameObject Model;
        private ObjectPool<EffectPopupAnimation> _effectPopupPool; 
        
        [HideInInspector] public bool isAttach;
        [HideInInspector] public bool isActivated; 

        
        //  Initialization  -------------------------------
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

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        private void CreatePopupEffect()
        {
            foreach (var builder in effectBuilders)
            {
                builder.CreatePopupPool();
            }
        }
        
        public object Clone()
        {
            throw new NotImplementedException();
        }
        //  Event Handlers --------------------------------

    }
    
}
