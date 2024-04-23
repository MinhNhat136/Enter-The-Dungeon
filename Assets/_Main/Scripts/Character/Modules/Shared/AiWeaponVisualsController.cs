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
    /// <summary>
    /// Handles visual representation of AI weapons.
    /// </summary>
    public class AiWeaponVisualsController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }
        public CombatMode CombatMode { get; private set; }
        public List<IAttachWeaponController> WeaponSlots { get; set; }
        public List<IAttachWeaponController> CurrentAttachedSlot => WeaponSlots.Where(attachSlot => attachSlot.IsAttach).ToList();
        public IAttachWeaponController CurrentActivatedSlot => WeaponSlots.First(slot => slot.IsActivated);
        public int CurrentActivatedSlotIndex => WeaponSlots.IndexOf(CurrentActivatedSlot);
       
        //  Collections -----------------------------------
        
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;

                WeaponSlots = GetComponentsInChildren<IAttachWeaponController>().ToList();
                foreach (var weaponSlot in WeaponSlots)
                {
                    weaponSlot.Initialize();
                    weaponSlot.OnActivate += (combatMode, weapon) =>
                    {
                        Model.CurrentWeapon = weapon;
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


        //  Other Methods ---------------------------------
        public void AttachDefaultWeapons()
        {
            var bowSlot = GetAttachSlot(AttachWeaponType.Spear);
            var shotgunSlot = GetAttachSlot(AttachWeaponType.Shotgun);

            if (bowSlot != null)
            {
                bowSlot.Attach();
                bowSlot.Activate(false);
            }

            if (shotgunSlot != null)
            {
                shotgunSlot.Attach();
                shotgunSlot.Activate(true);
            }
        }
        
        public IAttachWeaponController GetAttachSlot(AttachWeaponType weaponType) => WeaponSlots.Find(attachSlots => attachSlots.WeaponPrefab.WeaponType == weaponType);
        

        public void ActivateOtherWeapon()
        {
            if (CurrentAttachedSlot.Count <= 1)
            {
                return;
            }
            int indexUsedSlot = CurrentAttachedSlot.IndexOf(CurrentActivatedSlot);
            
            CurrentActivatedSlot.Activate(false);
            indexUsedSlot++;
            if (indexUsedSlot == CurrentAttachedSlot.Count) indexUsedSlot = 0;
            CurrentAttachedSlot[indexUsedSlot].Activate(true);
        }

        
        
        //  Event Handlers --------------------------------
    }

}
