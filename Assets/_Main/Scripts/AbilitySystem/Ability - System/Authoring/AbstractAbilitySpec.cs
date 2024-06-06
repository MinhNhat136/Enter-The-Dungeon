using System.Collections;

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
        
        public virtual IEnumerator TryActivateAbility()
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
            var cooldownTags = Ability.cooldown.gameplayEffectTags.grantedTags;

            float longestCooldown = 0f;

            // Check if the cooldown tag is granted to the player, and if so, capture the remaining duration for that tag
            for (var i = 0; i < this.Owner.appliedGameplayEffects.Count; i++)
            {
                var grantedTags = this.Owner.appliedGameplayEffects[i].spec.GameplayEffectScriptableObject.gameplayEffectTags.grantedTags;
                for (var iTag = 0; iTag < grantedTags.Length; iTag++)
                {
                    for (var iCooldownTag = 0; iCooldownTag < cooldownTags.Length; iCooldownTag++)
                    {
                        if (grantedTags[iTag] == cooldownTags[iCooldownTag])
                        {
                            // If this is an infinite GE, then return null to signify this is on CD
                            if (this.Owner.appliedGameplayEffects[i].spec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy == EDurationPolicy.Infinite) return new AbilityCooldownTime()
                            {
                                TimeRemaining = float.MaxValue,
                                TotalDuration = 0
                            };

                            var durationRemaining = this.Owner.appliedGameplayEffects[i].spec.DurationRemaining;

                            if (durationRemaining > longestCooldown)
                            {
                                longestCooldown = durationRemaining;
                                maxDuration = this.Owner.appliedGameplayEffects[i].spec.TotalDuration;
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

        /// <summary>
        /// Method to activate before activating this ability.  This method is run after activation checks.
        /// </summary>
        protected abstract IEnumerator PreActivate();

        /// <summary>
        /// The logic that dictates what the ability does.  Targeting logic should be placed here.
        /// Gameplay Effects are applied in this method.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator ActivateAbility();

        /// <summary>
        /// Method to run once the ability ends
        /// </summary>
        protected virtual void EndAbility()
        {
            isActive = false;
        }

        /// <summary>
        /// Checks whether the activating character has enough resources to activate this ability
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckCost()
        {
            if (this.Ability.cost == null) return true;
            var geSpec = this.Owner.MakeOutgoingSpec(this.Ability.cost, this.Level);
            // If this isn't an instant cost, then assume it passes cooldown check
            if (geSpec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy != EDurationPolicy.Instant) return true;

            for (var i = 0; i < geSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers.Length; i++)
            {
                var modifier = geSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modifier.modifierOperator != EAttributeModifier.Add) continue;
                var costValue = (modifier.modifierMagnitude.CalculateMagnitude(geSpec) * modifier.multiplier).GetValueOrDefault();

                this.Owner.AttributeSystemComponent.GetAttributeValue(modifier.attribute, out var attributeValue);

                // The total attribute after accounting for cost should be >= 0 for the cost check to succeed
                if (attributeValue.currentValue + costValue < 0) return false;

            }
            return true;
        }

        /// <summary>
        /// Checks if an Ability System Character has all the listed tags
        /// </summary>
        /// <param name="asc">Ability System Character</param>
        /// <param name="tags">List of tags to check</param>
        /// <returns>True, if the Ability System Character has all tags</returns>
        protected virtual bool AscHasAllTags(AbilitySystemController asc, GameplayTagScriptableObject[] tags)
        {
            // If the input ASC is not valid, assume check passed
            if (!asc) return true;

            for (var iAbilityTag = 0; iAbilityTag < tags.Length; iAbilityTag++)
            {
                var abilityTag = tags[iAbilityTag];

                bool requirementPassed = false;
                for (var iAsc = 0; iAsc < asc.appliedGameplayEffects.Count; iAsc++)
                {
                    GameplayTagScriptableObject[] ascGrantedTags = asc.appliedGameplayEffects[iAsc].spec.GameplayEffectScriptableObject.gameplayEffectTags.grantedTags;
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

        /// <summary>
        /// Checks if an Ability System Character has none of the listed tags
        /// </summary>
        /// <param name="asc">Ability System Character</param>
        /// <param name="tags">List of tags to check</param>
        /// <returns>True, if the Ability System Character has none of the tags</returns>
        protected virtual bool AscHasNoneTags(AbilitySystemController asc, GameplayTagScriptableObject[] tags)
        {
            // If the input ASC is not valid, assume check passed
            if (!asc) return true;

            for (var iAbilityTag = 0; iAbilityTag < tags.Length; iAbilityTag++)
            {
                var abilityTag = tags[iAbilityTag];

                bool requirementPassed = true;
                for (var iAsc = 0; iAsc < asc.appliedGameplayEffects.Count; iAsc++)
                {
                    GameplayTagScriptableObject[] ascGrantedTags = asc.appliedGameplayEffects[iAsc].spec.GameplayEffectScriptableObject.gameplayEffectTags.grantedTags;
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