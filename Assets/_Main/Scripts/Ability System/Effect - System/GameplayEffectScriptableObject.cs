using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect Definition")]
    public class GameplayEffectScriptableObject : ScriptableObject
    {
        [SerializeField]
        public GameplayEffectDefinitionContainer gameplayEffect;

        [SerializeField]
        public GameplayEffectTags gameplayEffectTags;

        [SerializeField]
        public GameplayEffectPeriod gameplayEffectPeriod;
    }

}
