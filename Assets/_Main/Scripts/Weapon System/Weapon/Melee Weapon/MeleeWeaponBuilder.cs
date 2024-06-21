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
        public AbstractMeleeAttackAbilityScriptableObject[] attackData;

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
            CancelInvoke(nameof(ResetCombo));
            
        }

        private AbstractMeleeAttackAbilityScriptableObject.AbstractMeleeAttackSpec _effectSpec;

        public void BeginHit()
        {
            _effectSpec = attackData[CurrentCombo]
                .CreateSpec(Owner.AiAbilityController.abilitySystemController, peak.transform);
            StartCoroutine(_effectSpec.TryActivateAbility());
        }

        public void EndHit()
        {
            if (_effectSpec == null) return;
            _effectSpec.CancelAbility();
        }
        
        public void EndAttack()
        {
            Invoke(nameof(ResetCombo), attackData[CurrentCombo].AnimationClip.length * Owner.SpeedRatio);
            if (CurrentCombo < attackData.Length - 1)
            {
                CurrentCombo++;
            }
            else ResetCombo();
        }
        
        public void ResetCombo()
        {
            CurrentCombo = 0;
        }
        
        //  Event Handlers --------------------------------
    }
}