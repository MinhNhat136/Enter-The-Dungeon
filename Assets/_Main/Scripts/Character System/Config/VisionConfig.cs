using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character
{
    [CreateAssetMenu(menuName = "Ai Module Config/Vision/Default")]
    public class VisionConfig : ScriptableObject
    {
        public float distance = 10f;

        public float angle = 30f;

        public float height = 1.0f;

        public int scanFrequency = 30;

        public int maxObjectRemember = 10;

        public LayerMask visionLayer;

        public LayerMask occlusionLayer;
    }
}

