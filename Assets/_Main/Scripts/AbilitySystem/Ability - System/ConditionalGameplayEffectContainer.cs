using System;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct ConditionalGameplayEffectContainer
    {
        public GameplayEffectScriptableObject gameplayEffect;
        public GameplayTagScriptableObject[] requiredSourceTags;
    }

}
