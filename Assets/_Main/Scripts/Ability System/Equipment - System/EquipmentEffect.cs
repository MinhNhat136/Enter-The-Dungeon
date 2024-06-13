using System.Collections.Generic;
using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public enum ERarity
    {
        Common = 0, 
        Great = 1,
        Rare = 2,
        Epic = 3, 
        Legendary = 4,
    }
    
    [CreateAssetMenu(menuName = "Gameplay Ability System/Equipment Effect")]
    public class EquipmentEffect : ScriptableObject
    {
        public ERarity rarity;
        public MinMaxFloat level;
        public GameplayEffectModifier[] defaultModifiers;
        public GameplayEffectModifier[] additionalModifiers;
    }
}