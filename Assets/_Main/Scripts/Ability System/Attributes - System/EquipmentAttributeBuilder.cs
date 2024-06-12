using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Equipment Attribute Builder")]
    public class EquipmentAttributeBuilder : ScriptableObject
    {
        public AnimationCurve scalingValue;
        public float offset;
        public List<GameplayEffectModifier> modifiers;

        public GameplayEffectModifier[] Build(int level = 0)
        {
            Debug.Log("build");
            var numberOfModifiers = Mathf.Clamp(level, 1, modifiers.Count);

            var selectedModifiers = new GameplayEffectModifier[numberOfModifiers];

            for (var i = 0; i < numberOfModifiers; i++)
            {
                var modifier = modifiers[i];

                modifier.multiplier = scalingValue.Evaluate(level) + (int)Random.Range(0, offset);

                selectedModifiers[i] = modifier;
            }

            return selectedModifiers;
        }
    }
}