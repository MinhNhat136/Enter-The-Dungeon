using System.Collections.Generic;
using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public abstract class AbstractAttributeEventHandler : ScriptableObject, IInitializable
    {
        public bool IsInitialized { get; set; }

        public virtual void Initialize()
        {
            if (IsInitialized) 
                return;
        }
        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        public abstract void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues);
        public abstract void Reset();
    }
}
