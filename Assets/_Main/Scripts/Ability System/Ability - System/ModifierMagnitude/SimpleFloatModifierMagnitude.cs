using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Modifier Magnitude/Simple Float")]
    public class SimpleFloatModifierMagnitude : ModifierMagnitudeScriptableObject
    {
        [FormerlySerializedAs("ScalingFunction")] [SerializeField]
        private AnimationCurve scalingFunction;

        public override void Initialise(GameplayEffectSpec spec)
        {
        }
        public override float? CalculateMagnitude(GameplayEffectSpec spec)
        {
            return scalingFunction.Evaluate(spec.Level);
        }
    }
}
