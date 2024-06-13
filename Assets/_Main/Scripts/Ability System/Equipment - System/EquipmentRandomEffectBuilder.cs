using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public class EquipmentRandomEffectBuilder: AbstractEquipmentEffectBuilder
    {
        public AnimationCurve scalingValue;
        public float offset;
        public List<GameplayEffectModifier> modifiers;
        
        public override GameplayEffectModifier[] Build(int numberEffect = 1, int level = 0)
        {
            var numberOfModifiers = Mathf.Clamp(numberEffect, 1, modifiers.Count);

            var selectedModifiers = new GameplayEffectModifier[numberOfModifiers];

            for (var i = 0; i < numberOfModifiers; i++)
            {
                var modifier = modifiers[i];
                modifier.multiplier = scalingValue.Evaluate(level) + Random.Range(0, offset);
                selectedModifiers[i] = modifier;
            }

            return selectedModifiers;
        }
    }
}