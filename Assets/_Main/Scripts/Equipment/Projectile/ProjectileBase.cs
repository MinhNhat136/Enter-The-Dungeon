using System;
using System.Collections;
using Atomic.Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileBase : MonoBehaviour
    {
        public delegate void CollisionEvent(ProjectileBase projectile, Collision collision);

        public event CollisionEvent OnCollision;
        public BaseAgent Owner { get; private set; }
        public Vector3 ShootPosition { get; private set; }
        public Vector3 ShootVelocity { get; set; }
        public Rigidbody Rigidbody {get; private set; }

        private WaitForSeconds _waitForSeconds;

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Spawn(BaseAgent owner, float delayedDisableTime)
        {
            Owner = owner;
            _waitForSeconds = new WaitForSeconds(delayedDisableTime);
            gameObject.SetActive(false);
        }
        
        public void Load(Vector3 shootPosition, Vector3 shootDirection, float velocity)
        {
            ShootPosition = shootPosition;
            transform.position = shootPosition;
            transform.forward = shootDirection;
            ShootVelocity = transform.forward * velocity;
            StartCoroutine(DelayedDisable());
        }

        public IEnumerator DelayedDisable()
        {
            Debug.Log(gameObject.GetInstanceID() + " turnoff");
            yield return _waitForSeconds;
            OnCollisionEnter(null);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollision?.Invoke(this, collision);
        }

        private void OnDisable()
        {
            StopCoroutine(DelayedDisable());
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}

