using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IVisionController : IInitializable, ITickable
    {
        public float VisionDistance { get; }
        public float VisionAngle { get; }


        public void Scan();
        public int Filter(GameObject[] buffer, string layerName);
    }
}
