using UnityEngine;
using System;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct AbilityTags
    {
        /// <summary>
        /// This tag describes the Gameplay Ability
        /// </summary>
        [SerializeField] public TagScriptableObject AssetTag;

        /// <summary>
        /// Active Gameplay Abilities (on the same character) that have these tags will be cancelled
        /// </summary>
        [SerializeField] public TagScriptableObject[] CancelAbilitiesWithTags;

        /// <summary>
        /// Gameplay Abilities that have these tags will be blocked from activating on the same character
        /// </summary>
        [SerializeField] public TagScriptableObject[] BlockAbilitiesWithTags;

        /// <summary>
        /// These tags are granted to the character while the ability is active
        /// </summary>
        [SerializeField] public TagScriptableObject[] ActivationOwnedTags;

        /// <summary>
        /// This ability can only be activated if the owner character has all of the Required tags
        /// and none of the Ignore tags.  Usually, the owner is the source as well.
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer OwnerTags;

        /// <summary>
        /// This ability can only be activated if the source character has all of the Required tags
        /// and none of the Ignore tags
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer SourceTags;

        /// <summary>
        /// This ability can only be activated if the target character has all of the Required tags
        /// and none of the Ignore tags
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer TargetTags;

    }

}