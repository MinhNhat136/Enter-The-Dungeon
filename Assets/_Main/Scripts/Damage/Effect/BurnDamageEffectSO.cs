using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/Burn",fileName = "Burn Damage", order = 1)]
    public class BurnDamageEffectSo : DamagePassiveEffectSo
    {
        public override PassiveEffect CreatePassiveEffect()
        {
            return new Burn()
            {
                EffectType = StatusEffectType.Burn,
                Tick = tick + tickBonus,
                Interval = interval + intervalBonus,
                Damage = damagePerTick + damagePerTickBonus,
            };
        }
    }
}
