using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/Toxic",fileName = "Toxic Damage", order = 2)]
    public class ToxicDamageEffectSo : DamagePassiveEffect
    {
        public override bool CanApplyDamage()
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyEffect(BaseAgent target)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveEffect(BaseAgent target)
        {
            throw new System.NotImplementedException();
        }
    }
}