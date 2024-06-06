using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Ability Event Handler/Ability Change Log")]
    public class LogApplyGameplayEffectEventHandler : AbstractApplyGameplayEffectEventHandler
    {
        public override void PreApplyEffectSpec(GameplayEffectSpec effectSpec)
        {
            Debug.Log(effectSpec.Source);
            Debug.Log(effectSpec.Target);
        }

        public override void Reset()
        {
        }
    }
}