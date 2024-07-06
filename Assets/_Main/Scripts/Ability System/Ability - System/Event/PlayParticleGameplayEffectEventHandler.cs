using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Ability Event Handler/Play Particles")]
    public class PlayParticleGameplayEffectEventHandler : AbstractApplyGameplayEffectEventHandler
    {
        public override void Initialize()
        {
        }

        public override void PreApplyEffectSpec(GameplayEffectSpec effectSpec)
        {
            Debug.Log("play Particle");
        }
    }
}


