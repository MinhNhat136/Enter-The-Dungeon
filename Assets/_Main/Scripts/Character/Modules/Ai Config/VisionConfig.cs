using UnityEngine;

namespace Atomic.Character
{
    [CreateAssetMenu(menuName = "Ai Module Config/Vision/Default")]
    public class VisionConfig : ScriptableObject
    {
        [SerializeField]
        public float Distance = 10f;

        [SerializeField]
        public float Angle = 30f;

        [SerializeField]
        public float Height = 1.0f;

        [SerializeField]
        public int ScanFrequency = 30;

        [SerializeField]
        public int MaxObjectRemember = 10;

        [SerializeField]
        public LayerMask VisionLayer;


        [SerializeField]
        public LayerMask OcclusionLayer;

        [SerializeField]
        public Color MeshVisionColor = Color.red;

        public void Assign(IVisionController visionController)
        {
            visionController.VisionDistance = Distance;
            visionController.VisionHeight = Height;
            visionController.VisionAngle = Angle;
            visionController.ScanFrequency = ScanFrequency;
            visionController.OcclusionLayer = OcclusionLayer;
            visionController.VisionLayer = VisionLayer;
            visionController.MaxObjectRemember = MaxObjectRemember;
        }
    }
}

