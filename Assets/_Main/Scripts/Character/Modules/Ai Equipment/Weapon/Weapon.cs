using System;
using System.Collections.Generic;
using Atomic.Character;
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
    public abstract class Weapon : MonoBehaviour, IInitializable
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public BaseAgent Owner { get; set; }
        public AttachWeaponType WeaponType => weaponType;
        public string WeaponName { get; set; }
        public Sprite WeaponIcon { get; set; }
        
        public GameObject WeaponRoot
        {
            get => weaponRoot;
            set => weaponRoot = value;
        }

        public bool IsWeaponActive { get; set; }
        public bool IsInitialized { get; private set; }
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private GameObject weaponRoot;

        [SerializeField] 
        private AttachWeaponType weaponType;
        
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
        public void RegisterOwner(BaseAgent owner)
        {
            this.Owner = owner;
        }
        
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            IsWeaponActive = show;
        }
        
        
        //  Event Handlers --------------------------------

    }
}

