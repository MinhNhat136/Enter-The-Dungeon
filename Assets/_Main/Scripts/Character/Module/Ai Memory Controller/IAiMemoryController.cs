using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IAiMemoryController
    {
        public List<AiMemoryObject> Memories { get; }
        public Predicate<AiMemoryObject> ForgetCondition { get; set; }
        public void UpdateSenses(IVisionController sensor, string layerName, GameObject[] objects);
        public void RefreshMemory(GameObject target);
        public AiMemoryObject FetchMemory(GameObject gameObject);
        public void ForgetMemory();
    }
}

