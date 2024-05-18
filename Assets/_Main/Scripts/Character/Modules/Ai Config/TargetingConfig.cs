using UnityEngine;

namespace Atomic.Character
{
    [CreateAssetMenu(menuName = "Ai Module Config /Targeting")]
    public class TargetingConfig : ScriptableObject
    {
        public string TargetLayer;

        public float MemorySpan = 3.0f;

        public float DistanceWeight = 1.0f;

        public float AngleWeight = 1.0f;

        public float AgeWeight = 1.0f;

        public int MaxNumberTarget = 16;
        
        

        public void Assign(ITargetingController targetingController)
        {
            targetingController.TargetLayer = TargetLayer;
            targetingController.MemorySpan = MemorySpan;
            targetingController.DistanceWeight = DistanceWeight;
            targetingController.AngleWeight = AngleWeight;
            targetingController.AgeWeight = AgeWeight;
            targetingController.MaxNumberTarget = MaxNumberTarget;
        }
    }

}
