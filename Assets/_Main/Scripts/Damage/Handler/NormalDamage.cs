
using Doozy.Runtime.Reactor.Ticker;
using UnityEngine;

namespace Atomic.Damage
{
    public class NormalDamage : PassiveEffect
    {
        public override void Apply()
        {
            if (Target.HealthController.IsDead)
            {
                return;
            }
            
            if (Target.HealthController.IsInvincible)
            {
                return; 
            }
            
            if (Target.PassiveStateController.Effects.HasFlag(EffectType))
            {
                Target.PassiveStateController.CurrentEffects |= EffectType;
                Target.PassiveStateController.EffectHandlers.Add(this);
            }
        }

        public override void Handle()
        {
            Target.HealthController.Decrease(Damage);
            var popup = effectPool.Get();
            var direction = (Target.transform.position - Source.transform.position).normalized;
            if (Vector3.Dot(direction, Vector3.right) > 0)
            {
                popup.horizontalDirection = 1;
            }
            else popup.horizontalDirection = -1;
            popup.transform.position = Target.transform.position;
            popup.value = Damage;
            popup.StartAnimation();
            popup.GetComponent<SelfPoolRelease>().Release(effectPool, popup);
            Remove();
        }

        public override void Remove()
        {
            Target.PassiveStateController.CurrentEffects &= ~EffectType;
            Target.PassiveStateController.EffectHandlers.Remove(this);
        }

        public override PassiveEffect Clone()
        {
            NormalDamage cloneEffect = new();
            cloneEffect.EffectType = EffectType;
            cloneEffect.Interval = Interval;
            cloneEffect.Tick = Tick;
            cloneEffect.Damage = Damage;
            cloneEffect.effectPool = effectPool;
            
            return cloneEffect;
        }
    }
}