using System.Collections;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Stat Initialisation")]
    public class InitialiseStatsAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        public GameplayEffectScriptableObject[] initialisationGameplayEffect;

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new InitialiseStatsAbility(this, owner)
            {
                Level = owner.Level
            };
            return spec;
        }

        public class InitialiseStatsAbility : AbstractAbilitySpec
        {
            public InitialiseStatsAbility(AbstractAbilityScriptableObject abilitySO, AbilitySystemController owner) : base(abilitySO, owner)
            {
            }

            public override void CancelAbility()
            {
            }

            protected override bool CheckGameplayTags()
            {
                return AscHasAllTags(Owner, Ability.abilityTags.OwnerTags.requireTags)
                        && AscHasNoneTags(Owner, Ability.abilityTags.OwnerTags.ignoreTags);
            }

            protected override IEnumerator ActivateAbility()
            {
                // Apply cost and cooldown (if any)
                if (Ability.cooldown)
                {
                    var cdSpec = Owner.MakeOutgoingSpec(Ability.cooldown);
                    Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
                }

                if (Ability.cost)
                {
                    var costSpec = Owner.MakeOutgoingSpec(Ability.cost);
                    Owner.ApplyGameplayEffectSpecToSelf(costSpec);
                }

                InitialiseStatsAbilityScriptableObject abilitySO = Ability as InitialiseStatsAbilityScriptableObject;
                Owner.AttributeSystemComponent.UpdateAttributeCurrentValues();

                for (var i = 0; i < abilitySO.initialisationGameplayEffect.Length; i++)
                {
                    var effectSpec = Owner.MakeOutgoingSpec(abilitySO.initialisationGameplayEffect[i]);
                    Owner.ApplyGameplayEffectSpecToSelf(effectSpec);
                    Owner.AttributeSystemComponent.UpdateAttributeCurrentValues();
                }

                yield break;
            }

            protected override IEnumerator PreActivate()
            {
                yield return null;
            }
        }
    }

}