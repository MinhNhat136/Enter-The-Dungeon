using UnityEngine;

namespace Atomic.Character
{
    [CreateAssetMenu(menuName = "Ai Module Config/Agent/Ai", fileName ="Ai Config")]
    public class AiConfig : ScriptableObject
    {
        [Header("THRESHOLD")]
        public float normalReactionThreshold;
        public float stunThreshold;
        public float breakThreshold;
        public float knockdownThreshold;
    }
}