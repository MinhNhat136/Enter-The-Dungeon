using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{

    public abstract class AbstractAbilityScriptableObject : BaseSo
    {
        [SerializeField] private string abilityName;
        
        [SerializeField] public AbilityTags abilityTags;
        
        [SerializeField] public GameplayEffectScriptableObject cost;
        
        [SerializeField] public GameplayEffectScriptableObject cooldown;
        
        [SerializeField] public AbstractApplyGameplayEffectEventHandler[] eventOnActivates; 
        
        public abstract AbstractAbilitySpec CreateSpec(AbilitySystemController owner);
    }
}