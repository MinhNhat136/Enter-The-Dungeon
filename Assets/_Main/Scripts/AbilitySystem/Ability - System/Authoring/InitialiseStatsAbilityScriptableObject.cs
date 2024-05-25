using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Stat Initialisation")]
    public class InitialiseStatsAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        public GameplayEffectScriptableObject[] initialisationGameplayEffect;

        public override AbstractAbilitySpec CreateSpec(AbilitySystemCharacter owner)
        {
            var spec = new InitialiseStatsAbility(this, owner)
            {
                Level = owner.Level
            };
            return spec;
        }

        public class InitialiseStatsAbility : AbstractAbilitySpec
        {
            public InitialiseStatsAbility(AbstractAbilityScriptableObject abilitySO, AbilitySystemCharacter owner) : base(abilitySO, owner)
            {
            }

            public override void CancelAbility()
            {
            }

            public override bool CheckGameplayTags()
            {
                return AscHasAllTags(Owner, Ability.AbilityTags.OwnerTags.requireTags)
                        && AscHasNoneTags(Owner, Ability.AbilityTags.OwnerTags.ignoreTags);
            }

            protected override IEnumerator ActivateAbility()
            {
                // Apply cost and cooldown (if any)
                if (Ability.Cooldown)
                {
                    var cdSpec = Owner.MakeOutgoingSpec(Ability.Cooldown);
                    Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
                }

                if (Ability.Cost)
                {
                    var costSpec = Owner.MakeOutgoingSpec(Ability.Cost);
                    Owner.ApplyGameplayEffectSpecToSelf(costSpec);
                }

                InitialiseStatsAbilityScriptableObject abilitySO = Ability as InitialiseStatsAbilityScriptableObject;
                Owner.AttributeSystem.UpdateAttributeCurrentValues();

                for (var i = 0; i < abilitySO.initialisationGameplayEffect.Length; i++)
                {
                    var effectSpec = Owner.MakeOutgoingSpec(abilitySO.initialisationGameplayEffect[i]);
                    Owner.ApplyGameplayEffectSpecToSelf(effectSpec);
                    Owner.AttributeSystem.UpdateAttributeCurrentValues();
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