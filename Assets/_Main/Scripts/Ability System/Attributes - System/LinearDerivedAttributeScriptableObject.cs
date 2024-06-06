using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute/Linear")]
    public class LinearDerivedAttributeScriptableObject : AttributeScriptableObject
    {
        public AttributeScriptableObject attribute;
        [SerializeField] private float gradient;
        [SerializeField] private float offset;

        public override AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue, List<AttributeValue> otherAttributeValues)
        {
            // Find desired attribute in list
            var baseAttributeValue = otherAttributeValues.Find(x => x.attributeScriptableObject == attribute);

            // Calculate new value
            attributeValue.baseValue = (baseAttributeValue.currentValue * gradient) + offset;

            attributeValue.currentValue = (attributeValue.baseValue + attributeValue.modifier.addValue) * (attributeValue.modifier.multiplyValue + 1);

            if (attributeValue.modifier.overrideValue != 0)
            {
                attributeValue.currentValue = attributeValue.modifier.overrideValue;
            }
            return attributeValue;
        }
    }
    
}
