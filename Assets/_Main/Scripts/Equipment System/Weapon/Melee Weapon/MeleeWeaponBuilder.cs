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
        public List<AbstractMeleeAttackAbilityScriptableObject> attackData;

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

        private AbstractMeleeAttackAbilityScriptableObject.AbstractMeleeAttackSpec effectSpec;
        
        public void BeginAttackMove()
        {
            effectSpec = attackData[CurrentCombo].CreateSpec(Owner.AiAbilityController.abilitySystemController, peak.transform);
            StartCoroutine(effectSpec.TryActivateAbility());   
        }
        
        public void EndAttackMove()
        {
            effectSpec.CancelAbility();
        }
        
        public void EndAttack()
        {
        }
        
        
        //  Event Handlers --------------------------------
    }
}