using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{

    public abstract class AbstractAbilityScriptableObject : BaseSo
    {
        /// <summary>
        /// Name of this ability
        /// </summary>
        [SerializeField] private string abilityName;

        /// <summary>
        /// Tags for this ability
        /// </summary>
        [SerializeField] public AbilityTags abilityTags;
        
        /// <summary>
        /// The GameplayEffect that defines the cost associated with activating the ability
        /// </summary>
        /// <returns></returns>
        [SerializeField] public GameplayEffectScriptableObject cost;

        /// <summary>
        /// The GameplayEffect that defines the cooldown associated with this ability
        /// </summary>
        /// <returns></returns>
        [SerializeField] public GameplayEffectScriptableObject cooldown;
        
        /// <summary>
        /// The Event that execute when this ability activate
        /// </summary>
        /// <returns></returns>
        [SerializeField] public AbstractApplyGameplayEffectEventHandler[] eventOnActivates; 

        /// <summary>
        /// Creates the Ability Spec (the instantiation of the ability)
        /// </summary>
        /// <param name="owner">Usually the character casting this ability</param>
        /// <returns>Ability Spec</returns>
        public abstract AbstractAbilitySpec CreateSpec(AbilitySystemController owner);
    }
}