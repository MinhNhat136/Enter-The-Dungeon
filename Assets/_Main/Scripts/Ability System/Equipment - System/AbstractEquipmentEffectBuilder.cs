using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public abstract class AbstractEquipmentEffectBuilder : ScriptableObject
    {
        

        public abstract GameplayEffectModifier[] Build(int numberEffect = 1, int level = 0);
    }
}
