using System.Collections.Generic;
using UnityEngine;


namespace Atomic.AbilitySystem
{
    public class AbilitySystemCharacter : MonoBehaviour
    {
        [SerializeField]
        protected AttributeSystemComponent _attributeSystem;
        public AttributeSystemComponent AttributeSystem { get { return _attributeSystem; } set { _attributeSystem = value; } }
        public List<GameplayEffectContainer> AppliedGameplayEffects = new List<GameplayEffectContainer>();
        public List<AbstractAbilitySpec> GrantedAbilities = new List<AbstractAbilitySpec>();
        public float Level;

        public void GrantAbility(AbstractAbilitySpec spec)
        {
            this.GrantedAbilities.Add(spec);
        }

        public void RemoveAbilitiesWithTag(GameplayTagScriptableObject tag)
        {
            for (var i = GrantedAbilities.Count - 1; i >= 0; i--)
            {
                if (GrantedAbilities[i].Ability.AbilityTags.AssetTag == tag)
                {
                    GrantedAbilities.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// Applies the gameplay effect spec to self
        /// </summary>
        /// <param name="geSpec">GameplayEffectSpec to apply</param>
        public bool ApplyGameplayEffectSpecToSelf(GameplayEffectSpec geSpec)
        {
            if (geSpec == null) return true;
            bool tagRequirementsOK = CheckTagRequirementsMet(geSpec);

            if (tagRequirementsOK == false) return false;


            switch (geSpec.GameplayEffect.gameplayEffect.durationPolicy)
            {
                case EDurationPolicy.HasDuration:
                case EDurationPolicy.Infinite:
                    ApplyDurationalGameplayEffect(geSpec);
                    break;
                case EDurationPolicy.Instant:
                    ApplyInstantGameplayEffect(geSpec);
                    return true;
            }

            return true;
        }
        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectScriptableObject GameplayEffect, float? level = 1f)
        {
            level = level ?? this.Level;
            return GameplayEffectSpec.CreateNew(
                GameplayEffect: GameplayEffect,
                Source: this,
                Level: level.GetValueOrDefault(1));
        }

        bool CheckTagRequirementsMet(GameplayEffectSpec geSpec)
        {
            /// Build temporary list of all gametags currently applied
            var appliedTags = new List<GameplayTagScriptableObject>();
            for (var i = 0; i < AppliedGameplayEffects.Count; i++)
            {
                appliedTags.AddRange(AppliedGameplayEffects[i].spec.GameplayEffect.gameplayEffectTags.grantedTags);
            }

            // Every tag in the ApplicationTagRequirements.RequireTags needs to be in the character tags list
            // In other words, if any tag in ApplicationTagRequirements.RequireTags is not present, requirement is not met
            for (var i = 0; i < geSpec.GameplayEffect.gameplayEffectTags.applicationTagRequirements.requireTags.Length; i++)
            {
                if (!appliedTags.Contains(geSpec.GameplayEffect.gameplayEffectTags.applicationTagRequirements.requireTags[i]))
                {
                    return false;
                }
            }

            // No tag in the ApplicationTagRequirements.IgnoreTags must in the character tags list
            // In other words, if any tag in ApplicationTagRequirements.IgnoreTags is present, requirement is not met
            for (var i = 0; i < geSpec.GameplayEffect.gameplayEffectTags.applicationTagRequirements.ignoreTags.Length; i++)
            {
                if (appliedTags.Contains(geSpec.GameplayEffect.gameplayEffectTags.applicationTagRequirements.ignoreTags[i]))
                {
                    return false;
                }
            }

            return true;
        }

        void ApplyInstantGameplayEffect(GameplayEffectSpec spec)
        {
            for (var i = 0; i < spec.GameplayEffect.gameplayEffect.modifiers.Length; i++)
            {
                var modifier = spec.GameplayEffect.gameplayEffect.modifiers[i];
                var magnitude = (modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier).GetValueOrDefault();
                var attribute = modifier.attribute;
                this.AttributeSystem.GetAttributeValue(attribute, out var attributeValue);

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
                }
                this.AttributeSystem.SetAttributeBaseValue(attribute, attributeValue.baseValue);
            }
        }
        void ApplyDurationalGameplayEffect(GameplayEffectSpec spec)
        {
            var modifiersToApply = new List<GameplayEffectContainer.ModifierContainer>();
            for (var i = 0; i < spec.GameplayEffect.gameplayEffect.modifiers.Length; i++)
            {
                var modifier = spec.GameplayEffect.gameplayEffect.modifiers[i];
                var magnitude = (modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier).GetValueOrDefault();
                var attributeModifier = new AttributeModifier();
                switch (modifier.modifierOperator)
                {
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
            AppliedGameplayEffects.Add(new GameplayEffectContainer() { spec = spec, modifiers = modifiersToApply.ToArray() });
        }

        void UpdateAttributeSystem()
        {
            // Set Current Value to Base Value (default position if there are no GE affecting that atribute)


            for (var i = 0; i < this.AppliedGameplayEffects.Count; i++)
            {
                var modifiers = this.AppliedGameplayEffects[i].modifiers;
                for (var m = 0; m < modifiers.Length; m++)
                {
                    var modifier = modifiers[m];
                    AttributeSystem.UpdateAttributeModifiers(modifier.Attribute, modifier.Modifier, out _);
                }
            }
        }

        void TickGameplayEffects()
        {
            for (var i = 0; i < this.AppliedGameplayEffects.Count; i++)
            {
                var ge = this.AppliedGameplayEffects[i].spec;

                // Can't tick instant GE
                if (ge.GameplayEffect.gameplayEffect.durationPolicy == EDurationPolicy.Instant) continue;

                // Update time remaining.  Stritly, it's only really valid for durational GE, but calculating for infinite GE isn't harmful
                ge.UpdateRemainingDuration(Time.deltaTime);

                // Tick the periodic component
                ge.TickPeriodic(Time.deltaTime, out var executePeriodicTick);
                if (executePeriodicTick)
                {
                    ApplyInstantGameplayEffect(ge);
                }
            }
        }

        void CleanGameplayEffects()
        {
            this.AppliedGameplayEffects.RemoveAll(x => x.spec.GameplayEffect.gameplayEffect.durationPolicy == EDurationPolicy.HasDuration && x.spec.DurationRemaining <= 0);
        }

        void Update()
        {
            // Reset all attributes to 0
            this.AttributeSystem.ResetAttributeModifiers();
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