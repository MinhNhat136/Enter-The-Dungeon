using System;
using Atomic.AbilitySystem;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [RequireComponent(
        typeof(AbilitySystemController),
        typeof(AttributeSystemComponent))]
    public class AiAbilityController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        //  Fields ----------------------------------------
        public AbstractAbilityScriptableObject[] abilities;
        public AbstractAbilityScriptableObject[] initialisationAbilities;
        [FormerlySerializedAs("abilitySystemCharacter")] [FormerlySerializedAs("_abilitySystemCharacter")] public AbilitySystemController abilitySystemController;
        private AbstractAbilitySpec[] _abilitySpecs;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model; 
                
                abilitySystemController = GetComponent<AbilitySystemController>();
                ActivateInitialisationAbilities();
                GrantCastableAbilities();
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        void ActivateInitialisationAbilities()
        {
            for (var i = 0; i < initialisationAbilities.Length; i++)
            {
                var spec = initialisationAbilities[i].CreateSpec(abilitySystemController);
                abilitySystemController.GrantAbility(spec);
                StartCoroutine(spec.TryActivateAbility());
            }
        }

        void GrantCastableAbilities()
        {
            _abilitySpecs = new AbstractAbilitySpec[abilities.Length];
            for (var i = 0; i < abilities.Length; i++)
            {
                var spec = abilities[i].CreateSpec(abilitySystemController);
                abilitySystemController.GrantAbility(spec);
                _abilitySpecs[i] = spec;
            }
        }
        
        public void UseAbility(int i)
        {
            var spec = _abilitySpecs[i];
            StartCoroutine(spec.TryActivateAbility());
        }
        //  Event Handlers --------------------------------
        
    }
}