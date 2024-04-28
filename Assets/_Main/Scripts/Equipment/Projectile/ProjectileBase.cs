using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileBase : MonoBehaviour
    {
        public delegate void TriggerEvent(ProjectileBase projectile, Collider collider);

        public event TriggerEvent OnTrigger;
        public BaseAgent Owner { get; private set; }
        public Vector3 ShootPosition { get; private set; }
        public Vector3 ShootVelocity { get; set; }
        public Rigidbody Rigidbody {get; private set; }
        
        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Spawn(BaseAgent owner)
        {
            Owner = owner;
            gameObject.SetActive(false);
        }
        
        public void Load(Vector3 shootPosition, Vector3 shootDirection, float velocity, float delayedDisableTime)
        {
            ShootPosition = shootPosition;
            transform.position = shootPosition;
            transform.forward = shootDirection;
            ShootVelocity = transform.forward * velocity;
            Invoke(nameof(DelayedDisable), delayedDisableTime);
        }

        public void DelayedDisable()
        {
            OnTriggerEnter(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger?.Invoke(this, other);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(DelayedDisable));
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}

