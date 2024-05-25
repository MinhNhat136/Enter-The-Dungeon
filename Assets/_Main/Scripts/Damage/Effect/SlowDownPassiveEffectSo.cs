using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/SlowDown", fileName = "Slow Down", order = 4)]
    public class SlowDownPassiveEffectSo : DamagePassiveEffectSo
    {
        [Range(0,1)]
        [SerializeField] private float _slowDownPercentage; 
        
        public override PassiveEffect CreatePassiveEffect()
        {
            return new SlowDownEffect()
            {
                Source = Damager,
                effectPool = popupPool,
                EffectType = StatusEffectType.Slow,
                SlowDownPercentage = _slowDownPercentage,
                Tick = tick,
                Damage = damage,
            };
        }
    }
}