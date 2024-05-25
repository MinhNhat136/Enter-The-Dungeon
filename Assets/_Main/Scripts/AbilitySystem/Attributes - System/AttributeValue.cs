using System;
using UnityEngine.Serialization;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct AttributeValue
    {
        public AttributeScriptableObject attributeScriptableObject;
        public float baseValue;
        public float currentValue;
        public AttributeModifier modifier;
    }

    [Serializable]
    public struct AttributeModifier
    {
        public float addValue;
        public float multiplyValue;
        public float overrideValue;

        public AttributeModifier Combine(AttributeModifier other)
        {
            other.addValue += addValue;
            other.multiplyValue += multiplyValue;
            other.overrideValue = overrideValue;
            return other;
        }
    }
}