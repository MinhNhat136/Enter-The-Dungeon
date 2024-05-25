using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Clamp Attribute")]
    public class ClampAttributeEventHandler : AbstractAttributeEventHandler
    {

        [SerializeField]
        private AttributeScriptableObject primaryAttribute;
        [SerializeField]
        private AttributeScriptableObject maxAttribute;
        
        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.MAttributeIndexCache;
            ClampAttributeToMax(primaryAttribute, maxAttribute, currentAttributeValues, attributeCacheDict);
        }

        private void ClampAttributeToMax(AttributeScriptableObject attribute1, AttributeScriptableObject attribute2, List<AttributeValue> attributeValues, Dictionary<AttributeScriptableObject, int> attributeCacheDict)
        {
            if (attributeCacheDict.TryGetValue(attribute1, out var primaryAttributeIndex)
                && attributeCacheDict.TryGetValue(attribute2, out var maxAttributeIndex))
            {
                var newPrimaryAttribute = attributeValues[primaryAttributeIndex];
                var newMaxAttribute = attributeValues[maxAttributeIndex];

                // Clamp current and base values
                if (newPrimaryAttribute.currentValue > newMaxAttribute.currentValue) newPrimaryAttribute.currentValue = newMaxAttribute.currentValue;
                if (newPrimaryAttribute.baseValue > newMaxAttribute.baseValue) newPrimaryAttribute.baseValue = newMaxAttribute.baseValue;
                attributeValues[primaryAttributeIndex] = newPrimaryAttribute;
            }
        }
    }

}
