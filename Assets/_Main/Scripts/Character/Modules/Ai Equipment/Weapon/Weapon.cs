using System;
using System.Collections.Generic;
using Atomic.Character.Model;
using Atomic.Character.Module;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [Serializable]
    public abstract class Weapon : MonoBehaviour, IInitializable, ICharacterActionTrigger, IActionEffectTrigger
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public BaseAgent Owner { get; set; }
        public string WeaponName { get; set; }
        public Sprite WeaponIcon { get; set; }
        public GameObject WeaponRoot => weaponRoot;
        public bool IsWeaponActive { get; set; }
        public bool IsInitialized { get; private set; }
        
        protected Dictionary<CharacterActionType, Action> ActionTriggers { get; set; } 
        protected Dictionary<ActionEffectType, Action> EffectTriggers { get; set; }
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private GameObject weaponRoot;
        
        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Weapon not initialized");
            }
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            IsWeaponActive = show;
        }
        
        public void RegisterCharacterActionTrigger()
        {
            
        }

        public void RegisterEffectTrigger()
        {
            
        }
        
        //  Event Handlers --------------------------------
        public void OnCharacterActionTrigger(CharacterActionType actionType)
        {
            
        }

        public void OnActionEffectTrigger(ActionEffectType effectType)
        {
        }
    }
}

