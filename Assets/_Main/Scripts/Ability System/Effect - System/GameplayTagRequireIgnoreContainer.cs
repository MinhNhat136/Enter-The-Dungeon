using System;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public struct GameplayTagRequireIgnoreContainer
    {
        /// <summary>
        /// All of these tags must be present
        /// </summary>
        public TagScriptableObject[] requireTags;

        /// <summary>
        /// None of these tags can be present
        /// </summary>
        public TagScriptableObject[] ignoreTags;
    }

}
