using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public class AbilitySystemController : MonoBehaviour
    {
        public delegate void NotifyGameplayEffectApply(GameplayEffectSpec gameplayEffectSpec);

        public NotifyGameplayEffectApply onApplyGameplayEffect;

        [SerializeField] protected AttributeSystemComponent attributeSystemComponent;

        public AttributeSystemComponent AttributeSystemComponent
        {
            get => attributeSystemComponent;
            set => attributeSystemComponent = value;
        }

        public readonly List<GameplayEffectContainer> appliedGameplayEffects = new(16);
        private readonly List<AbstractAbilitySpec> _grantedAbilities = new(16);
        public AbstractApplyGameplayEffectEventHandler[] gameplayEffectEventHandlers;
        public float Level;

        public void GrantAbility(AbstractAbilitySpec spec)
        {
            _grantedAbilities.Add(spec);
        }

        public void RemoveAbilitiesWithTag(TagScriptableObject tag)
        {
            for (var i = _grantedAbilities.Count - 1; i >= 0; i--)
            {
                if (_grantedAbilities[i].Ability.abilityTags.AssetTag == tag)
                {
                    _grantedAbilities.RemoveAt(i);
                }
            }
        }

        private void RemoveGameplayEffectsWithTag(TagScriptableObject[] tagsToRemove)
        {
            for (int i = appliedGameplayEffects.Count - 1; i >= 0; i--)
            {
                var appliedEffect = appliedGameplayEffects[i];
                var assetTag = appliedEffect.spec.GameplayEffectScriptableObject.gameplayEffectTags.assetTag;
                
                if (tagsToRemove.Contains(assetTag))
                {
                    appliedGameplayEffects.RemoveAt(i);
                }
            }
        }
        
        public bool ApplyGameplayEffectSpecToSelf(GameplayEffectSpec gameplayEffectSpec)
        {
            if (gameplayEffectSpec == null) return true;
            
            bool canApplyEffect = CheckTag(gameplayEffectSpec.GameplayEffectScriptableObject.gameplayEffectTags.applicationTagRequirements);
            
            if (canApplyEffect == false) return false;

            var tagsToRemove = gameplayEffectSpec.GameplayEffectScriptableObject.gameplayEffectTags.removeGameplayEffectsWithTag;

            if (tagsToRemove is { Length: > 0 })
            {
                RemoveGameplayEffectsWithTag(tagsToRemove);
            }
            
            switch (gameplayEffectSpec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy)
            {
                case EDurationPolicy.HasDuration:
                case EDurationPolicy.Infinite:
                    ApplyInfiniteGameplayEffect(gameplayEffectSpec);
                    break;
                case EDurationPolicy.Instant:
                    ApplyInstantGameplayEffect(gameplayEffectSpec);
                    return true;
            }

            return true;
        }

        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectScriptableObject gameplayEffect, float? level = 1f)
        {
            level = level ?? Level;
            return GameplayEffectSpec.CreateNew(
                gameplayEffect: gameplayEffect,
                source: this,
                level: level.GetValueOrDefault(1));
        }

        bool CheckTag(GameplayTagRequireIgnoreContainer tags)
        {
            var appliedTags = new List<TagScriptableObject>();
            foreach (var appliedGameplayEffect in appliedGameplayEffects)
            {
                appliedTags.AddRange(appliedGameplayEffect.spec.GameplayEffectScriptableObject.gameplayEffectTags.grantedTagsToAbilitySystem);
            }

            foreach (var requireTag in tags.requireTags)
            {
                if (!appliedTags.Contains(requireTag))
                {
                    return false;
                }
            }

            foreach (var ignoreTag in tags.ignoreTags)
            {
                if (appliedTags.Contains(ignoreTag))
                {
                    return false;
                }
            }

            return true;
        }

        private void ApplyInstantGameplayEffect(GameplayEffectSpec spec)
        {
            foreach (var modifier in spec.GameplayEffectScriptableObject.gameplayEffect.modifiers)
            {
                var magnitude = (modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier)
                    .GetValueOrDefault();
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

                AttributeSystemComponent.SetAttributeBaseValue(attribute, attributeValue.baseValue);
            }
        }
        
        private void ApplyInfiniteGameplayEffect(GameplayEffectSpec spec)
        {
            var modifiersToApply = new List<GameplayEffectContainer.ModifierContainer>();

            foreach (var modifier in spec.GameplayEffectScriptableObject.gameplayEffect.modifiers)
            {
                var magnitude = (modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier)
                    .GetValueOrDefault();
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

                modifiersToApply.Add(new GameplayEffectContainer.ModifierContainer()
                    { Attribute = modifier.attribute, Modifier = attributeModifier });
                appliedGameplayEffects.Add(new GameplayEffectContainer()
                {
                    spec = spec, modifiers = modifiersToApply.ToArray()
                });
            }
        }

        private void UpdateAttributeSystem()
        {
            foreach (var appliedGameplayEffect in appliedGameplayEffects)
            {
                var modifiers = appliedGameplayEffect.modifiers;
                foreach (var modifier in modifiers)
                {
                    AttributeSystemComponent.UpdateAttributeModifiers(modifier.Attribute, modifier.Modifier, out _);
                }
            }
        }

        private void TickGameplayEffects()
        {
            foreach (var appliedGameplayEffect in appliedGameplayEffects)
            {
                var gameplayEffect = appliedGameplayEffect.spec;

                if (gameplayEffect.GameplayEffectScriptableObject.gameplayEffect.durationPolicy ==
                    EDurationPolicy.Instant) continue;

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
            appliedGameplayEffects.RemoveAll(x =>
                x.spec.GameplayEffectScriptableObject.gameplayEffect.durationPolicy == EDurationPolicy.HasDuration &&
                x.spec.DurationRemaining <= 0);
        }

        [Button]
        private void ShowTag()
        {
            foreach (var appliedGameplayEffect in appliedGameplayEffects)
            {
                Debug.Log(appliedGameplayEffect.spec.GameplayEffectScriptableObject.gameplayEffectTags
                    .assetTag);
            }
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