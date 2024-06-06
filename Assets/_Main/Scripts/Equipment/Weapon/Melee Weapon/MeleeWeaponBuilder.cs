using System.Collections.Generic;
using Atomic.AbilitySystem;
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
    public class MeleeWeaponBuilder : WeaponBuilder
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public int CurrentCombo { get; set; }
        
        //  Fields ----------------------------------------
        public GameObject peak;
        public List<MeleeAttackAbilityScriptableObject> attackData;

        //  Initialization  -------------------------------
        public override void Attach(BaseAgent owner)
        {
            base.Attach( owner);
        }

        public override void Detach()
        {
            base.Detach();
            CurrentCombo = 0; 
        }
        
        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------
        public void BeginAttack()
        {
           
            
        }

        private MeleeAttackAbilityScriptableObject.MeleeAttackSpec effectSpec;
        
        public void BeginAttackMove()
        {
            effectSpec = attackData[CurrentCombo].CreateMeleeAttackSpecValue(Owner.AiAbilityController.abilitySystemController, peak.transform);
            effectSpec.isAttack = true;
            StartCoroutine(effectSpec.TryActivateAbility());   
        }
        
        public void EndAttackMove()
        {
            effectSpec.isAttack = false;
        }
        
        public void EndAttack()
        {
        }
        
        
        //  Event Handlers --------------------------------
    }
}