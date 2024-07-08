using System;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct GameplayEffectTags
    {
        public TagScriptableObject assetTag;
        
        public TagScriptableObject[] grantedTagsToAbilitySystem;
        
        public TagScriptableObject[] removeGameplayEffectsWithTag;
        
        public GameplayTagRequireIgnoreContainer applicationTagRequirements;
    }
}
