using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IVisionSystem : IInitializable, ITickable
    {
        public void Scan();
        public int Filter(GameObject[] buffer, string layerName);
    }
}
