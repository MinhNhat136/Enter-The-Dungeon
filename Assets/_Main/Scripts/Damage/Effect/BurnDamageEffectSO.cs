using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    [CreateAssetMenu(menuName = "Damage/Effect/Burn",fileName = "Burn Damage", order = 1)]
    public class BurnDamageEffectSo : DamagePassiveEffect
    {
        public override bool CanApplyDamage()
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyEffect(BaseAgent target)
        {
            Debug.Log("Burn");
        }

        public override void RemoveEffect(BaseAgent target)
        {
            Debug.Log("stop burn");
        }
    }
    
}
