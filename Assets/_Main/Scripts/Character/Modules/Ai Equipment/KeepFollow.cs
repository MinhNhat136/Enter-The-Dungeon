using UnityEngine;

namespace Atomic.Character.Module
{
    public class KeepFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private bool flatRotation;

        public void Update()
        {
            Follow();
        }

        private void Follow()
        {
        }

    }

}
