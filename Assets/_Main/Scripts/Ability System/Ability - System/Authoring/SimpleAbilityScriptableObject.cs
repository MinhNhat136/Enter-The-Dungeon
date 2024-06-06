using System.Collections;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    /// <summary>
    /// Simple Ability that applies a Gameplay Effect to the activating character
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Simple Ability")]
    public class SimpleAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        /// <summary>
        /// Gameplay Effect to apply
        /// </summary>
        public GameplayEffectScriptableObject GameplayEffect;

        /// <summary>
        /// Creates the Ability Spec, which is instantiated for each character.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new SimpleAbilitySpec(this, owner)
            {
                Level = owner.Level
            };
            return spec;
        }

        /// <summary>
        /// The Ability Spec is the instantiation of the ability.  Since the Ability Spec
        /// is instantiated for each character, we can store stateful data here.
        /// </summary>
        public class SimpleAbilitySpec : AbstractAbilitySpec
        {
            public SimpleAbilitySpec(AbstractAbilityScriptableObject abilitySO, AbilitySystemController owner) : base(abilitySO, owner)
            {
            }

            /// <summary>
            /// What to do when the ability is cancelled.  We don't care about there for this example.
            /// </summary>
            public override void CancelAbility() { }

            /// <summary>
            /// What happens when we activate the ability.
            /// 
            /// In this example, we apply the cost and cooldown, and then we apply the main
            /// gameplay effect
            /// </summary>
            /// <returns></returns>
            protected override IEnumerator ActivateAbility()
            {
                // Apply cost and cooldown
                var cdSpec = Owner.MakeOutgoingSpec(Ability.cooldown);
                var costSpec = Owner.MakeOutgoingSpec(Ability.cost);
                Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
                Owner.ApplyGameplayEffectSpecToSelf(costSpec);


                // Apply primary effect
                var effectSpec = Owner.MakeOutgoingSpec(((SimpleAbilityScriptableObject)Ability).GameplayEffect);
                Owner.ApplyGameplayEffectSpecToSelf(effectSpec);

                yield return null;
            }

            /// <summary>
            /// Checks to make sure Gameplay Tags checks are met. 
            /// 
            /// Since the target is also the character activating the ability,
            /// we can just use Owner for all of them.
            /// </summary>
            /// <returns></returns>
            protected override bool CheckGameplayTags()
            {
                return AscHasAllTags(Owner, Ability.abilityTags.OwnerTags.requireTags)
                        && AscHasNoneTags(Owner, Ability.abilityTags.OwnerTags.ignoreTags)
                        && AscHasAllTags(Owner, Ability.abilityTags.SourceTags.requireTags)
                        && AscHasNoneTags(Owner, Ability.abilityTags.SourceTags.ignoreTags)
                        && AscHasAllTags(Owner, Ability.abilityTags.TargetTags.requireTags)
                        && AscHasNoneTags(Owner, Ability.abilityTags.TargetTags.ignoreTags);
            }

            /// <summary>
            /// Logic to execute before activating the ability.  We don't need to do anything here
            /// for this example.
            /// </summary>
            /// <returns></returns>

            protected override IEnumerator PreActivate()
            {
                yield return null;
            }
        }
    }

}