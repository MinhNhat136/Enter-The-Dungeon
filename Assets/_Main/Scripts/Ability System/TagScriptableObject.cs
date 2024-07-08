using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Tag")]
    public class TagScriptableObject : ScriptableObject
    {
        [SerializeField]
        private TagScriptableObject parent;
        
        public bool IsDescendantOf(TagScriptableObject other, int nSearchLimit = 4)
        {
            int i = 0;
            TagScriptableObject tag = parent;
            while (nSearchLimit > i++)
            {
                if (!tag) return false;
                if (tag == other) return true;
                
                tag = tag.parent;
            }

            return false;
        }
    }
    
}
