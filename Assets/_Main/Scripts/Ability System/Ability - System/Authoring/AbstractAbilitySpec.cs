using System.Collections;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public struct AbilityCooldownTime
    {
        public float TimeRemaining;
        public float TotalDuration;
    }

    public abstract class AbstractAbilitySpec
    {
        public delegate void ApplyGameplayEffectEvent(GameplayEffectSpec effectSpec);

        public ApplyGameplayEffectEvent OnApplyGameplayEffect;

        public readonly AbstractAbilityScriptableObject Ability;
        protected readonly AbilitySystemController Owner;
        public float Level;
        public bool isActive;

        public AbstractAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner)
        {
            Ability = ability;
            Owner = owner;
        }

        public IEnumerator TryActivateAbility()
        {
            if (!CanActivateAbility()) yield break;

            isActive = true;
            yield return PreActivate();
            yield return ActivateAbility();
            EndAbility();
        }

        protected virtual bool CanActivateAbility()
        {
            return !isActive
                   && CheckGameplayTags()
                   && CheckCost()
                   && CheckCooldown().TimeRemaining <= 0;
        }

        public abstract void CancelAbility();

        protected abstract bool CheckGameplayTags();

        protected virtual AbilityCooldownTime CheckCooldown()
        {
            float maxDuration = 0;
            if (Ability.cooldown == null) return new AbilityCooldownTime();
            var cooldownTags = Ability.cooldown.gameplayEffectTags.grantedTagsToAbilitySystem;

            float longestCooldown = 0f;

            // Check if the cooldown tag is granted to the player, and if so, capture the remaining duration for that tag
            foreach (var appliedGameplayEffect in Owner.appliedGameplayEffects)
            {
                var grantedTags = appliedGameplayEffect.spec.GameplayEffectScriptableObject.gameplayEffectTags
                    .grantedTagsToAbilitySystem;
                foreach (var grantedTag in grantedTags)
                {
                    foreach (var coolDownTag in cooldownTags)
                    {
                        if (grantedTag == coolDownTag)
                        {
                            // If this is an infinite GE, then return null to signify this is on CD
                            if (appliedGameplayEffect.spec.GameplayEffectScriptableObject.gameplayEffect
                                    .durationPolicy == EDurationPolicy.Infinite)
                                return new AbilityCooldownTime()
                                {
                                    TimeRemaining = float.MaxValue,
                                    TotalDuration = 0
                                };

                            var durationRemaining = appliedGameplayEffect.spec.DurationRemaining;

                            if (durationRemaining > longestCooldown)
                            {
                                longestCooldown = durationRemaining;
                                maxDuration = appliedGameplayEffect.spec.TotalDuration;
                            }
                        }
                    }
                }
            }

            return new AbilityCooldownTime()
            {
                TimeRemaining = longestCooldown,
                TotalDuration = maxDuration
            };
        }
        
        protected abstract IEnumerator PreActivate();
        
        protected abstract IEnumerator ActivateAbility();
        
        protected virtual void EndAbility()
        {
            isActive = false;
        }
        
        protected virtual bool CheckCost()
        {
            if (this.Ability.cost == null) return true;
            var geSpec = this.Owner.MakeOutgoingSpec(this.Ability.cost, this.Level);
            // If this isn't an instant cost, then assume it passes cooldown check
            if (geSpec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy !=
                EDurationPolicy.Instant) return true;

            for (var i = 0; i < geSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers.Length; i++)
            {
                var modifier = geSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modifier.modifierOperator != EAttributeModifier.Add) continue;
                var costValue = (modifier.modifierMagnitude.CalculateMagnitude(geSpec) * modifier.multiplier)
                    .GetValueOrDefault();

                this.Owner.AttributeSystemComponent.GetAttributeValue(modifier.attribute, out var attributeValue);

                // The total attribute after accounting for cost should be >= 0 for the cost check to succeed
                if (attributeValue.currentValue + costValue < 0) return false;
            }

            return true;
        }
        
        protected virtual bool AscHasAllTags(AbilitySystemController asc, TagScriptableObject[] tags)
        {
            // If the input ASC is not valid, assume check passed
            if (!asc) return true;
            
            for (var iAbilityTag = 0; iAbilityTag < tags.Length; iAbilityTag++)
            {
                var abilityTag = tags[iAbilityTag];

                bool requirementPassed = false;
                for (var iAsc = 0; iAsc < asc.appliedGameplayEffects.Count; iAsc++)
                {
                    TagScriptableObject[] ascGrantedTags = asc.appliedGameplayEffects[iAsc].spec
                        .GameplayEffectScriptableObject.gameplayEffectTags.grantedTagsToAbilitySystem;
                    for (var iAscTag = 0; iAscTag < ascGrantedTags.Length; iAscTag++)
                    {
                        if (ascGrantedTags[iAscTag] == abilityTag)
                        {
                            requirementPassed = true;
                        }
                    }
                }

                // If any ability tag wasn't found, requirements failed
                if (!requirementPassed) return false;
            }

            return true;
        }
        
        protected virtual bool AscHasNoneTags(AbilitySystemController asc, TagScriptableObject[] tags)
        {
            // If the input ASC is not valid, assume check passed
            if (!asc) return true;

            for (var iAbilityTag = 0; iAbilityTag < tags.Length; iAbilityTag++)
            {
                var abilityTag = tags[iAbilityTag];

                bool requirementPassed = true;
                for (var iAsc = 0; iAsc < asc.appliedGameplayEffects.Count; iAsc++)
                {
                    TagScriptableObject[] ascGrantedTags = asc.appliedGameplayEffects[iAsc].spec
                        .GameplayEffectScriptableObject.gameplayEffectTags.grantedTagsToAbilitySystem;
                    for (var iAscTag = 0; iAscTag < ascGrantedTags.Length; iAscTag++)
                    {
                        if (ascGrantedTags[iAscTag] == abilityTag)
                        {
                            requirementPassed = false;
                        }
                    }
                }

                // If any ability tag wasn't found, requirements failed
                if (!requirementPassed) return false;
            }

            return true;
        }
    }
}