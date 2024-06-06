using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    /// <summary>
    /// This asset defines a single player attribute
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute/Default")]
    public class AttributeScriptableObject : ScriptableObject
    {
        /// <summary>
        /// Friendly name of this attribute.  Used for display purposes only.
        /// </summary>
        public string attributeName;
        public Color color;
        
        public AttributeValue CalculateInitialValue(AttributeValue attributeValue, List<AttributeValue> otherAttributeValues)
        {
            return attributeValue;
        }

        public virtual AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue, List<AttributeValue> otherAttributeValues)
        {
            attributeValue.currentValue = (attributeValue.baseValue + attributeValue.modifier.addValue) * (attributeValue.modifier.multiplyValue + 1);

            if (attributeValue.modifier.overrideValue != 0)
            {
                attributeValue.currentValue = attributeValue.modifier.overrideValue;
            }
            return attributeValue;
        }
    }
}
