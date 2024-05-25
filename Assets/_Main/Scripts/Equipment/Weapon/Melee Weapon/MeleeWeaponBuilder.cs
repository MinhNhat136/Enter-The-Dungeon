using System.Collections.Generic;
using Atomic.Character;
using Atomic.Core;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [CreateAssetMenu(fileName = "Melee Weapon", menuName = "Weapons/Melee/Config/Default", order = 0)]
    public class MeleeWeaponBuilder : WeaponBuilder
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public int CurrentCombo { get; set; }
        
        //  Fields ----------------------------------------
        public List<MeleeAttackData> attackData;
        private MeleeWeaponObject _meleeWeaponObject;

        //  Initialization  -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            
            Model.transform.SetParent(parent);

            _meleeWeaponObject = Model.GetComponent<MeleeWeaponObject>();
            _meleeWeaponObject.Owner = Owner;
            _meleeWeaponObject.SetForceWeight(forceWeight);
        }

        public override void Detach()
        {
            base.Detach();
            _meleeWeaponObject = null;
            CurrentCombo = 0; 
        }
        
        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------
        public void BeginAttack()
        {
            _meleeWeaponObject.EnergyValue = attackData[CurrentCombo].EnergyValue;
            foreach (var effect in effectBuilders)
            {
                _meleeWeaponObject.PassiveEffect.Add(effect.CreatePassiveEffect());
            }
        }
        
        public void BeginAttackMove()
        {
            _meleeWeaponObject.BeginHit();
        }
        
        public void EndAttackMove()
        {
            _meleeWeaponObject.EndHit();
        }
        
        public void EndAttack()
        {
            _meleeWeaponObject.PassiveEffect.Clear();
        }
        
        
        //  Event Handlers --------------------------------
    }
}