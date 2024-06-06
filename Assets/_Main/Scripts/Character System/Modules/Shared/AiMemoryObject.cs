using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [System.Serializable]
    public class AiMemoryObject
    {
        public float Age
        {
            get { return Time.time - lastTime; }
        }
        public GameObject gameObject;
        public Vector3 position;
        public Vector3 direction;
        public int layerIndex;
        public float distance;
        public float angle;
        public float lastTime;
        public float score;
    }
}
