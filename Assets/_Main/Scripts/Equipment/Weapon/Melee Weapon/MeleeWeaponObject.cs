using System;
using System.Collections.Generic;
using Atomic.Character;
using Atomic.Core;
using Atomic.Damage;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class MeleeWeaponObject : MonoBehaviour, IEnergyConsumer<MeleeWeaponObject>
    {
        public float EnergyValue { get; set; }

        public BaseAgent Owner { get; set; }
        public List<PassiveEffect> PassiveEffect { get; set; } = new(8);
        private readonly List<BaseAgent> _hitAgent = new(8);
        
        private MinMaxFloat ForceWeight { get; set; }
        
        //  Events ----------------------------------------
        public GameObject pivot; 
        
        //  Properties ------------------------------------
        public Collider Collider { get; private set; }
        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public MeleeWeaponObject SetForceWeight(MinMaxFloat forceWeight)
        {
            ForceWeight = forceWeight;
            return this; 
        }
        
        //  Unity Methods   -------------------------------
        public void Awake()
        {
            Collider = GetComponent<Collider>();
            Collider.enabled = false;
        }

        public void BeginHit()
        {
            Collider.enabled = true;
        }

        public void EndHit()
        {
            Collider.enabled = false;
            _hitAgent.Clear();
        }
       
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void OnTriggerEnter(Collider other)
        {
            var agent = other.GetComponentInParent<BaseAgent>();
            if (agent && !_hitAgent.Contains(agent))
            {
                _hitAgent.Add(agent);
                foreach (var effect in PassiveEffect)
                {
                    effect.Target = agent;
                    effect.Source = Owner;
                    
                    effect.Apply();
                }

                agent.ImpactSensorController.ImpactValue = ForceWeight.GetValueFromRatio(EnergyValue);
                agent.ImpactSensorController.ImpactDirection = (agent.transform.position - Owner.transform.position).normalized;
                agent.ImpactSensorController.Impact();
            }
        }
    }
    
}
