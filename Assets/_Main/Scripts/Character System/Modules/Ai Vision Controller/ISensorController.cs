using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public interface ISensorController : IInitializableWithBaseModel<BaseAgent>
    {
        public void Scan();
        public int Filter(GameObject[] buffer, string layerName);
    }
}
