using System;
using System.Collections.Generic;
using System.Linq;
using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character
{
   
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    [Serializable]
    public struct AttachParent
    {
        public WeaponType WeaponType;
        public Transform ParentTransform; 
    }
    /// <summary>
    /// Handles visual representation of AI weapons.
    /// </summary>
    public class AiWeaponVisualsController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }
        public List<WeaponBuilder> weaponSlots;
       
        //  Collections -----------------------------------
        
        
        //  Fields ----------------------------------------
        private int _currentWeaponIndex;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (IsInitialized) return;
            IsInitialized = true;
            Model = model;

            _currentWeaponIndex = 0;

            foreach (var weaponSlot in weaponSlots)
            {
                weaponSlot.Attach(weaponSlot.transform, Model);
                weaponSlot.OnActivated += (combatMode) =>
                {
                    Model.CurrentWeapon = weaponSlots[_currentWeaponIndex];
                    Model.CurrentCombatMode = combatMode;
                };
            }

            ActivateOtherWeapon();
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("Ai WeaponVisualController ...... ");
            }
        }

        
        //  Unity Methods   -------------------------------
        public void OnDestroy()
        {
            DetachAllWeapons();
        }

        //  Other Methods ---------------------------------
        private void DetachAllWeapons()
        {
            foreach (var weapon in weaponSlots)
            {
                weapon.Detach();
            }
        }
        
        public void ActivateOtherWeapon()
        {
            weaponSlots[_currentWeaponIndex].DeActivate();
            if (_currentWeaponIndex == weaponSlots.Count - 1) _currentWeaponIndex = 0;
            else _currentWeaponIndex++;
            weaponSlots[_currentWeaponIndex]?.Activate();
        }
        //  Event Handlers --------------------------------
    }

}
