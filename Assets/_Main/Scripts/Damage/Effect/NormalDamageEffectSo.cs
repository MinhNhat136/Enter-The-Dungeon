using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/Normal",fileName = "Normal Damage", order = 0)]
    public class NormalDamageEffectSo : DamagePassiveEffectSo
    {
        public override PassiveEffect CreatePassiveEffect()
        {
            return new NormalDamage()
            {
                Source = Damager,
                effectPool = popupPool,
                EffectType = StatusEffectType.Normal,
                Damage = damagePerTick,
            };
        }
    }
    
}

