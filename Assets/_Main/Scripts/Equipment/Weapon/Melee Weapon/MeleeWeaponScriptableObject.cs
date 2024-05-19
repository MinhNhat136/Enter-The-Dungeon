using System.Collections.Generic;
using Atomic.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [CreateAssetMenu(fileName = "Melee Weapon", menuName = "Weapons/Melee/Config/Default", order = 0)]
    public class MeleeWeaponScriptableObject : WeaponScriptableObject
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------
        public List<MeleeAttackData> attackData;
        private MeleeWeaponObject _meleeWeaponObject;
        
        private readonly List<BaseAgent> _hitAgent = new(8);

        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            Model.transform.SetParent(parent);

            _meleeWeaponObject = Model.GetComponent<MeleeWeaponObject>();
            _meleeWeaponObject.OnHitObject += OnWeaponTrigger;
        }

        //  Other Methods ---------------------------------
        public void BeginAttackMove()
        {
            _meleeWeaponObject.Collider.enabled = true;
        }

        public void EndAttackMove()
        {
        }

        public void BeginHitDamage()
        {
            
        }

        public void EndHitDamage()
        {
            
        }

        public void BeginAttack()
        {
        }

        public void EndAttack()
        {
            foreach (var agent in _hitAgent)
            {
                foreach (var effectBuilder in effectBuilders)
                {
                    var effect = effectBuilder.CreatePassiveEffect();
                    effect.Target = agent;
                    effect.Source = Owner;
                    
                    effect.Apply();
                }
            }
            _meleeWeaponObject.Collider.enabled = false;
            _hitAgent.Clear();
        }

        private void OnWeaponTrigger(Collider collider)
        {
            var agent = collider.GetComponentInParent<BaseAgent>();
            if (agent && !_hitAgent.Contains(agent))
            {
                _hitAgent.Add(agent);
            }
        }
        
        //  Event Handlers --------------------------------
    }
}