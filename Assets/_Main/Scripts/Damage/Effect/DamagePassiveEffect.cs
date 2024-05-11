using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    public abstract class DamagePassiveEffect : ScriptableObject
    {
        public float chance;
        public float chanceBonus; 
        
        public float duration;
        public float durationBonus;

        public float damage;
        public float damageBonus;

        public abstract bool CanApplyDamage();
        public abstract void ApplyEffect(BaseAgent target);
        public abstract void RemoveEffect(BaseAgent target);
    }
    
}
