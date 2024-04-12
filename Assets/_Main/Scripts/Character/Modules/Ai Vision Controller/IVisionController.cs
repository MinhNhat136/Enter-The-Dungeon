using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IVisionController : IInitializableWithBaseModel<BaseAgent>
    {
        public float VisionDistance { get; set; }
        public float VisionAngle { get; set; }
        public float VisionHeight { get; set; }
        public float ScanFrequency { get; set; }
        public int MaxObjectRemember { get; set; }
        public LayerMask VisionLayer { get; set; }
        public LayerMask OcclusionLayer { get; set; }
        public Color MeshVisionColor { get; set; }

        public void Scan();
        public int Filter(GameObject[] buffer, string layerName);
    }
}
