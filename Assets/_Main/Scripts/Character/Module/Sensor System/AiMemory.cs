using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [System.Serializable]
    public class AiMemory
    {
        public float Age
        {
            get { return Time.time - lastTime; }
        }
        public GameObject gameObject;
        public Vector3 position;
        public Vector3 direction;
        public float distance;
        public float angle;
        public float damage;
        public float lastTime;
        public float score;
    }
}
