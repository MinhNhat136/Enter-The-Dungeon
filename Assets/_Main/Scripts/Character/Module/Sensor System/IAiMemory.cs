using UnityEngine.TextCore.Text;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IAiMemory
    {
        public void UpdateSense(AiVisionSensorSystem sensor);

        public void RefreshMemory(GameObject agent, GameObject target);

        public AiMemory FetchMemory(GameObject gameObject);

        public void ForgetMemory(float olderThan);
    }
}

