using Atomic.Core;


namespace Atomic.AbilitySystem
{
    public abstract class AbstractApplyGameplayEffectEventHandler : BaseSo
    {
        public abstract void PreApplyEffectSpec(GameplayEffectSpec effectSpec);

    }
    
}
