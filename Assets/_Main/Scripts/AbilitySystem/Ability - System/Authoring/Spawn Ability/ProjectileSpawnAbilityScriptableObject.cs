using System.Collections;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Projectile")]
    public class ProjectileSpawnAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        [HideInInspector] public ProjectileBase projectile;
        public GameplayEffectScriptableObject gameplayEffect;
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new ProjectileAbilitySpec(this, owner)
            {
                Level = owner.Level,
                projectile = projectile,
            };
            return spec;
        }
        
        private class ProjectileAbilitySpec : AbstractAbilitySpec
        {
            public ProjectileBase projectile;
            private GameplayEffectSpec _effectSpec;

            public ProjectileAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
            }

            protected override bool CheckGameplayTags()
            {
                return AscHasAllTags(Owner, Ability.abilityTags.OwnerTags.requireTags)
                       && AscHasNoneTags(Owner, Ability.abilityTags.OwnerTags.ignoreTags)
                       && AscHasAllTags(Owner, Ability.abilityTags.SourceTags.requireTags)
                       && AscHasNoneTags(Owner, Ability.abilityTags.SourceTags.ignoreTags);
            }
            
            protected override IEnumerator PreActivate()
            {
                yield return null;
            }

            protected override IEnumerator ActivateAbility()
            {
                var cdSpec = Owner.MakeOutgoingSpec(Ability.cooldown);
                var costSpec = Owner.MakeOutgoingSpec(Ability.cost);
                
                Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
                Owner.ApplyGameplayEffectSpecToSelf(costSpec);

                projectile.Shoot();
                _effectSpec = Owner.MakeOutgoingSpec(((ProjectileSpawnAbilityScriptableObject)Ability).gameplayEffect);
                Owner.ApplyGameplayEffectSpecToSelf(_effectSpec);
                yield return null;
            }
            
            public override void CancelAbility()
            {
            }
        }
    }
}