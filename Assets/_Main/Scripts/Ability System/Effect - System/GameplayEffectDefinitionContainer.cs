using System;
using UnityEngine.Serialization;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct GameplayEffectDefinitionContainer
    {
        public EDurationPolicy durationPolicy;

        public ModifierMagnitudeScriptableObject durationModifier;
        
        public float durationMultiplier;
        
        public GameplayEffectModifier[] modifiers;
    }

}
