using System;
using System.Linq;
using UnityEngine;

namespace Atomic.Equipment
{
    public class RangedWeaponAssembler : MonoBehaviour
    {
        public WeaponComponent[] components;

        public Transform GetAttachTransform(WeaponComponentEnum componentEnum)
        {
            return components.First(component => component.componentEnum == componentEnum).componentPosition;
        }
    }
    
    public enum WeaponComponentEnum
    {
        Indicator,
        Barrel,
        Magazine, 
    }

    [Serializable]
    public struct WeaponComponent
    {
        public WeaponComponentEnum componentEnum;
        public Transform componentPosition;
    }
}
