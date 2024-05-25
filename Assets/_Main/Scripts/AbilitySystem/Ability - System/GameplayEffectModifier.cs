using System;
using UnityEngine.Serialization;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct GameplayEffectModifier
    {
        public AttributeScriptableObject attribute;
        public EAttributeModifier modifierOperator;
        public ModifierMagnitudeScriptableObject modifierMagnitude;
        public float multiplier;
    }
    
    public enum EAttributeModifier
    {
        Add, 
        Multiply, 
        Override
    }
}
