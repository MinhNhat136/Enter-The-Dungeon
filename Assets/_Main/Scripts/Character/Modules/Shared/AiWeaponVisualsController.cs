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
        public List<WeaponScriptableObject> WeaponSlots;
        private List<WeaponScriptableObject> CurrentAttachedSlot => WeaponSlots.Where(attachSlot => attachSlot.IsAttach).ToList();
        public AttachParent[] AttachParents;
       
        //  Collections -----------------------------------
        
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;

                foreach (var weaponSlot in WeaponSlots)
                {
                    weaponSlot.OnActivated += (combatMode) =>
                    {
                        Model.CurrentWeapon = WeaponSlots.First(slot => slot.IsActivated);
                        Model.CurrentCombatMode = combatMode;
                    };
                }    
            }
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
        public void AttachDefaultWeapons()
        {
            AttachWeapon(WeaponType.Bow);
            AttachWeapon(WeaponType.Cannon);
            
            ActivateOtherWeapon();
        }

        public void DetachAllWeapons()
        {
            foreach (var weapon in WeaponSlots)
            {
                weapon.Detach();
            }
        }
        
        public WeaponScriptableObject GetAttachSlot(WeaponType weaponType)
        {
            return WeaponSlots.Find(attachSlots => attachSlots.WeaponType == weaponType);
        }

        public void AttachWeapon(WeaponType weaponType)
        {
            var weapon = GetAttachSlot(weaponType);
            if (!weapon) return; 
            weapon.Attach(AttachParents.First(attachPoint => attachPoint.WeaponType == weapon.WeaponType).ParentTransform,
                Model);
        }

        public void ActivateOtherWeapon()
        {
            if (CurrentAttachedSlot.Count <= 1)
            {
                return;
            }
            var activatedSlot = WeaponSlots.FirstOrDefault(slot => slot.IsActivated);
    
            if (activatedSlot != null)
            {
                int indexUsedSlot = CurrentAttachedSlot.IndexOf(activatedSlot);
                activatedSlot.DeActivate();
                indexUsedSlot++;
                if (indexUsedSlot == CurrentAttachedSlot.Count) indexUsedSlot = 0;
                CurrentAttachedSlot[indexUsedSlot]?.Activate();
            }
            else
            {
                CurrentAttachedSlot[0]?.Activate();
            }
        }
        
        //  Event Handlers --------------------------------
    }

}
