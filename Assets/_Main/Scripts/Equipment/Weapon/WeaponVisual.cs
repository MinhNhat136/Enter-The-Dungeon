using System;
using UnityEngine;

namespace  Atomic.Equipment
{
    [Serializable]
    public class WeaponVisual
    {
        private event Action onChangedEvent;
        
        [SerializeField]
        private GameObject[] visuals;

        public bool IsShow => false;

        public event Action OnChangedEvent
        {
            add => onChangedEvent += value;
            remove => onChangedEvent -= value;
        }
        
    }
    
}
