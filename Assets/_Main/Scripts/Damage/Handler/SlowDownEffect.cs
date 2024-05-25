using UnityEngine;

namespace Atomic.Damage
{
    public class SlowDownEffect : PassiveEffect
    {
        public float SlowDownPercentage { get; set; }
        public override void Apply()
        {
            if (Target.HealthController.IsDead)
            {
                return;
            }

            if (Target.PassiveStateController.CurrentEffects.HasFlag(EffectType))
            {
                return;
            }

            Target.PassiveStateController.CurrentEffects |= EffectType;
            Target.PassiveStateController.EffectHandlers.Add(this);

            Target.MovementSpeed *= SlowDownPercentage;
            Target.PassiveStateController.StartCoroutine(DelayRemoveCoroutine());
        }


        public override void Handle()
        {
            
        }
        
        public override void Remove()
        {
            Target.MovementSpeed *= 1/SlowDownPercentage;
            Target.PassiveStateController.CurrentEffects &= ~EffectType;
            Target.PassiveStateController.EffectHandlers.Remove(this);
        }

        public override PassiveEffect Clone()
        {
            SlowDownEffect cloneEffect = new();
            cloneEffect.EffectType = EffectType;
            cloneEffect.Interval = Interval;
            cloneEffect.Tick = Tick;
            cloneEffect.Damage = Damage;
            cloneEffect.effectPool = effectPool;
            cloneEffect.SlowDownPercentage = SlowDownPercentage;
            
            return cloneEffect;
        }
    }
    
}
