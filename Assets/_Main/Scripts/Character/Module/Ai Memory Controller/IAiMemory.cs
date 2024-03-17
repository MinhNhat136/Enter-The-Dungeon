using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IAiMemory
    {
        public void UpdateSense(AiVisionSensorController sensor, string layerName, GameObject[] objects);

        public void RefreshMemory(GameObject agent, GameObject target);

        public AiMemory FetchMemory(GameObject gameObject);

        public void ForgetMemory();
    }
}

