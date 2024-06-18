using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character
{
    [CreateAssetMenu(menuName = "Ai Module Config /Targeting")]
    public class TargetingConfig : ScriptableObject
    {
        [FormerlySerializedAs("TargetLayer")] public string targetLayer;

        [FormerlySerializedAs("DistanceWeight")] public float distanceWeight = 1.0f;

        [FormerlySerializedAs("AngleWeight")] public float angleWeight = 1.0f;

        [FormerlySerializedAs("AgeWeight")] public float ageWeight = 1.0f;

        [FormerlySerializedAs("MaxNumberTarget")] public int maxNumberTarget = 16;
        
    }

}
