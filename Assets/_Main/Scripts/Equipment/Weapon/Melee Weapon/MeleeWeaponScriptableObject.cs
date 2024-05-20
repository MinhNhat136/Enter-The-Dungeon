using System.Collections.Generic;
using System.Linq;
using Atomic.Character;
using UnityEngine;
using UnityEngine.AI;

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
        public int CurrentCombo { get; set; }
       

        //  Fields ----------------------------------------
        public List<MeleeAttackData> attackData;
        
        private MeleeWeaponObject _meleeWeaponObject;
        private readonly List<BaseAgent> _hitAgent = new(8);
        private NavMeshHit _navMeshHit;
        private RaycastHit _rayCastHit;

        //  Initialization  -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            
            Model.transform.SetParent(parent);

            _meleeWeaponObject = Model.GetComponent<MeleeWeaponObject>();
            _meleeWeaponObject.OnHitObject += OnWeaponTrigger;
        }

        public override void Detach()
        {
            base.Detach();
            _hitAgent.Clear();
            _meleeWeaponObject = null;
            CurrentCombo = 0; 
        }
        
        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------
        public void BeginAttackMove()
        {
            _meleeWeaponObject.Collider.enabled = true;
        }
        
        public void EndAttackMove()
        {
            foreach (var agent in _hitAgent)
            {
                foreach (var effect in effectBuilders.Select(effectBuilder => effectBuilder.CreatePassiveEffect()))
                {
                    effect.Target = agent;
                    effect.Source = Owner;
                    
                    effect.Apply();
                }

                agent.ImpactSensorController.ImpactValue =
                    forceWeight.GetValueFromRatio(attackData[CurrentCombo].EnergyValue);
                agent.ImpactSensorController.ImpactDirection = (agent.transform.position - Owner.transform.position).normalized;
                agent.ImpactSensorController.Impact();
            }
            _meleeWeaponObject.Collider.enabled = false;
            _hitAgent.Clear();
        }
        
        public void EndAttack()
        {
           
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