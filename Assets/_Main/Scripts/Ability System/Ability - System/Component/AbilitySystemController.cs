using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public class AbilitySystemController : MonoBehaviour
    {
        [SerializeField]
        protected AttributeSystemComponent attributeSystemComponent;
        public AttributeSystemComponent AttributeSystemComponent 
        { 
            get => attributeSystemComponent;
            set => attributeSystemComponent = value;
        }
        public readonly List<GameplayEffectContainer> appliedGameplayEffects = new();
        public readonly List<AbstractAbilitySpec> grantedAbilities = new();
        public float Level;

        public void GrantAbility(AbstractAbilitySpec spec)
        {
            grantedAbilities.Add(spec);
        }

        public void RemoveAbilitiesWithTag(GameplayTagScriptableObject tag)
        {
            for (var i = grantedAbilities.Count - 1; i >= 0; i--)
            {
                if (grantedAbilities[i].Ability.abilityTags.AssetTag == tag)
                {
                    grantedAbilities.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// Applies the gameplay effect spec to self
        /// </summary>
        /// <param name="gameplayEffectSpec">GameplayEffectSpec to apply</param>
        public bool ApplyGameplayEffectSpecToSelf(GameplayEffectSpec gameplayEffectSpec)
        {
            if (gameplayEffectSpec == null) return true;
            bool tagRequirementsOK = CheckTagRequirementsMet(gameplayEffectSpec);

            if (tagRequirementsOK == false) return false;
            
            switch (gameplayEffectSpec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy)
            {
                case EDurationPolicy.HasDuration:
                case EDurationPolicy.Infinite:
                    ApplyDurationalGameplayEffect(gameplayEffectSpec);
                    break;
                case EDurationPolicy.Instant:
                    ApplyInstantGameplayEffect(gameplayEffectSpec);
                    return true;
            }

            return true;
        }
        
        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectScriptableObject GameplayEffect, float? level = 1f)
        {
            level = level ?? this.Level;
            return GameplayEffectSpec.CreateNew(
                gameplayEffect: GameplayEffect,
                source: this,
                level: level.GetValueOrDefault(1));
        }

        bool CheckTagRequirementsMet(GameplayEffectSpec geSpec)
        {
            // Build temporary list of all game tags currently applied
            var appliedTags = new List<GameplayTagScriptableObject>();
            for (var i = 0; i < appliedGameplayEffects.Count; i++)
            {
                appliedTags.AddRange(appliedGameplayEffects[i].spec.GameplayEffectScriptableObject.gameplayEffectTags.grantedTags);
            }

            // Every tag in the ApplicationTagRequirements.RequireTags needs to be in the character tags list
            // In other words, if any tag in ApplicationTagRequirements.RequireTags is not present, requirement is not met
            for (var i = 0; i < geSpec.GameplayEffectScriptableObject.gameplayEffectTags.applicationTagRequirements.requireTags.Length; i++)
            {
                if (!appliedTags.Contains(geSpec.GameplayEffectScriptableObject.gameplayEffectTags.applicationTagRequirements.requireTags[i]))
                {
                    return false;
                }
            }

            // No tag in the ApplicationTagRequirements.IgnoreTags must in the character tags list
            // In other words, if any tag in ApplicationTagRequirements.IgnoreTags is present, requirement is not met
            for (var i = 0; i < geSpec.GameplayEffectScriptableObject.gameplayEffectTags.applicationTagRequirements.ignoreTags.Length; i++)
            {
                if (appliedTags.Contains(geSpec.GameplayEffectScriptableObject.gameplayEffectTags.applicationTagRequirements.ignoreTags[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private void ApplyInstantGameplayEffect(GameplayEffectSpec spec)
        {
            for (var i = 0; i < spec.GameplayEffectScriptableObject.gameplayEffect.modifiers.Length; i++)
            {
                var modifier = spec.GameplayEffectScriptableObject.gameplayEffect.modifiers[i];
                var magnitude = (modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier).GetValueOrDefault();
                var attribute = modifier.attribute;
                AttributeSystemComponent.GetAttributeValue(attribute, out var attributeValue);

                switch (modifier.modifierOperator)
                {
                    case EAttributeModifier.Add:
                        attributeValue.baseValue += magnitude;
                        break;
                    case EAttributeModifier.Multiply:
                        attributeValue.baseValue *= magnitude;
                        break;
                    case EAttributeModifier.Override:
                        attributeValue.baseValue = magnitude;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                this.AttributeSystemComponent.SetAttributeBaseValue(attribute, attributeValue.baseValue);
            }
        }

        private void ApplyDurationalGameplayEffect(GameplayEffectSpec spec)
        {
            var modifiersToApply = new List<GameplayEffectContainer.ModifierContainer>();
            for (var i = 0; i < spec.GameplayEffectScriptableObject.gameplayEffect.modifiers.Length; i++) 
            {
                var modifier = spec.GameplayEffectScriptableObject.gameplayEffect.modifiers[i];
                var magnitude = (modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier).GetValueOrDefault();
                var attributeModifier = new AttributeModifier();
                switch (modifier.modifierOperator) {
                    case EAttributeModifier.Add:
                        attributeModifier.addValue = magnitude;
                        break;
                    case EAttributeModifier.Multiply:
                        attributeModifier.multiplyValue = magnitude;
                        break;
                    case EAttributeModifier.Override:
                        attributeModifier.overrideValue = magnitude;
                        break;
                }
                modifiersToApply.Add(new GameplayEffectContainer.ModifierContainer() { Attribute = modifier.attribute, Modifier = attributeModifier });
            }
            appliedGameplayEffects.Add(new GameplayEffectContainer() { spec = spec, modifiers = modifiersToApply.ToArray() });
        }

        private void UpdateAttributeSystem()
        {
            // Set Current Value to Base Value (default position if there are no GE affecting that attribute)
            for (var i = 0; i < appliedGameplayEffects.Count; i++)
            {
                var modifiers = appliedGameplayEffects[i].modifiers;
                for (var m = 0; m < modifiers.Length; m++)
                {
                    var modifier = modifiers[m];
                    AttributeSystemComponent.UpdateAttributeModifiers(modifier.Attribute, modifier.Modifier, out _);
                }
            }
        }

        private void TickGameplayEffects()
        {
            for (var i = 0; i < appliedGameplayEffects.Count; i++)
            {
                var gameplayEffect = appliedGameplayEffects[i].spec;

                // Can't tick instant GE
                if (gameplayEffect.GameplayEffectScriptableObject.gameplayEffect.durationPolicy == EDurationPolicy.Instant) continue;

                // Update time remaining.  Strictly, it's only really valid for durational GE, but calculating for infinite GE isn't harmful
                gameplayEffect.UpdateRemainingDuration(Time.deltaTime);

                // Tick the periodic component
                gameplayEffect.TickPeriodic(Time.deltaTime, out var executePeriodicTick);
                if (executePeriodicTick)
                {
                    ApplyInstantGameplayEffect(gameplayEffect);
                }
            }
        }

        private void CleanGameplayEffects()
        {
            appliedGameplayEffects.RemoveAll(x => x.spec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy == EDurationPolicy.HasDuration && x.spec.DurationRemaining <= 0);
        }

        private void Update()
        {
            // Reset all attributes to 0
            AttributeSystemComponent.ResetAttributeModifiers();
            UpdateAttributeSystem();

            TickGameplayEffects();
            CleanGameplayEffects();
        }
    }
}

namespace Atomic.AbilitySystem
{
    public class GameplayEffectContainer
    {
        public GameplayEffectSpec spec;
        public ModifierContainer[] modifiers;

        public class ModifierContainer
        {
            public AttributeScriptableObject Attribute;
            public AttributeModifier Modifier;
        }
    }
}