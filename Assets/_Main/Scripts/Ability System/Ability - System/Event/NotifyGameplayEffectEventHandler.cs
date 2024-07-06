using System.Linq;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Ability Event Handler/Attribute Change Notify")]
    public class NotifyGameplayEffectEventHandler : AbstractApplyGameplayEffectEventHandler
    {

        public override void Initialize()
        {
        }
        
        public override void PreApplyEffectSpec(GameplayEffectSpec effectSpec)
        {
            if (!gameplayEffectLookup.Contains(effectSpec.GameplayEffectScriptableObject)) return;
            if (!effectSpec.Target) return;
            if (!effectSpec.Target.gameplayEffectEventHandlers.Contains(this)) return;
            effectSpec.Target.onApplyGameplayEffect?.Invoke(effectSpec);
        }
    }
}