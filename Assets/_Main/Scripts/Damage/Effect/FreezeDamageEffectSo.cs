using Atomic.Character;
using UnityEngine;

namespace  Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/Freeze",fileName = "Freeze Damage", order = 0)]
    public class FreezeDamageEffectSo : DamagePassiveEffectSo
    {
        public override PassiveEffect CreatePassiveEffect()
        {
            return new Burn();
        }
    }
    
}
