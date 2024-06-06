using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Attribute Change Notify")]
    public class NotifyAttributeChangeEventHandler : AbstractAttributeEventHandler
    {
        private enum EComparisionType
        {
            Greater, 
            Less,
            Different,
        }
        
        
        [SerializeField]
        private AttributeScriptableObject attribute;

        [SerializeField] 
        private EComparisionType comparisionType;
        

        public override void Initialize()
        {
            base.Initialize();
            Reset();
        }

        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.MAttributeIndexCache;
            if (attributeCacheDict.TryGetValue(attribute, out var primaryAttributeIndex))
            {
                var prevValue = prevAttributeValues[primaryAttributeIndex].currentValue;
                var currentValue = currentAttributeValues[primaryAttributeIndex].currentValue;

                bool shouldInvoke = false;

                switch (comparisionType)
                {
                    case EComparisionType.Different:
                        shouldInvoke = Math.Abs(prevValue - currentValue) > 0.001f;
                        break;
                    case EComparisionType.Less:
                        shouldInvoke = currentValue < prevValue;
                        break;
                    case EComparisionType.Greater:
                        shouldInvoke = currentValue > prevValue;
                        break;
                }

                if (shouldInvoke)
                {
                    attributeSystem.onAttributeChanged?.Invoke(attribute);
                }
            }
        }

        public override void Reset()
        {
            
        }
    }
}