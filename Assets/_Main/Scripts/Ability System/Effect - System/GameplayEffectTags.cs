using System;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct GameplayEffectTags
    {
        /// <summary>
        /// The tag that defines this gameplay effect
        /// </summary>
        public GameplayTagScriptableObject assetTag;

        /// <summary>
        /// The tags this GE grants to the ability system character
        /// </summary>
        [SerializeField] public GameplayTagScriptableObject[] grantedTags;

        /// <summary>
        /// Remove GE that match these tags
        /// </summary>
        [SerializeField] public GameplayTagScriptableObject[] removeGameplayEffectsWithTag;
        
        /// <summary>
        /// These tags must be present for this GE to be applied
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer applicationTagRequirements;

        /// <summary>
        /// These tags determine if the GE is considered 'on' or 'off'
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer ongoingTagRequirements;
    }
}
