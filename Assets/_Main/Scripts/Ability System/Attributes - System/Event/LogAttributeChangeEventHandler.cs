using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Attribute Change Log")]
    public class LogAttributeChangeEventHandler : AbstractAttributeEventHandler
    {
        [SerializeField]
        private AttributeScriptableObject PrimaryAttribute;

        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.MAttributeIndexCache;
            if (attributeCacheDict.TryGetValue(PrimaryAttribute, out var primaryAttributeIndex))
            {
                var prevValue = prevAttributeValues[primaryAttributeIndex].currentValue;
                var currentValue = currentAttributeValues[primaryAttributeIndex].currentValue;

                if (prevValue != currentValue)
                {
                    // If value has changed, log a message to console
                    Debug.Log($"{attributeSystem.gameObject.name}: {currentAttributeValues[primaryAttributeIndex].attributeScriptableObject.name} modified.  Old Value: {prevValue}.  New Value: {currentValue}.");
                }
            }
        }

        public override void Reset()
        {
            
        }
        
        
    }
    
}
