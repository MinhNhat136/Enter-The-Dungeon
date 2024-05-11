using Atomic.Character;
using UnityEngine;

namespace  Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/Freeze",fileName = "Freeze Damage", order = 0)]
    public class FreezeDamageEffectSo : DamagePassiveEffect
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
