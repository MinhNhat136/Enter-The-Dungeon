using UnityEngine;

namespace Atomic.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform cameraTransform;

        void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            var dir = transform.position - cameraTransform.position;
            dir.x = 0;
            transform.forward = dir;
        }
    }
}
