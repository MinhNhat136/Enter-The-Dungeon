using UnityEngine;

namespace Atomic.Equipment
{
    public class KeepFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        

        [SerializeField]
        private bool flatRotation;

        public void LateUpdate()
        {
            Follow();
        }

        public void Follow()
        {
            if (target == null)
                return;

            transform.position = target.position;

            if (flatRotation)
            {
                transform.rotation = target.rotation;
            }
        }
    }
}