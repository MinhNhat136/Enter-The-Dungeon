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
        public float DelayedDisableTime = 2f; 
        public delegate void CollisionEvent(ProjectileBase projectile, Collision collision);

        public event CollisionEvent OnCollision;
        public BaseAgent Owner { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialDirection { get; private set; }
        public Vector3 InitialVelocity { get; set;  }
        public IProjectileTrajectoryController TrajectoryController { get; set; }
        public Rigidbody Rigidbody {get; private set; }

        private WaitForSeconds _waitForSeconds;
        public UnityAction OnShoot ;

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Shoot(BaseAgent owner, Vector3 SpawnForce)
        {
            Owner = owner;
            transform.position = InitialPosition;
            InitialDirection = SpawnForce.normalized;
            transform.forward = SpawnForce.normalized;

            OnShoot?.Invoke();
        }

        public void Update()
        {
            TrajectoryController.ApplyTrajectory(this);
        }

        public void Spawn(Vector3 spawnPoint, IProjectileTrajectoryController trajectoryController)
        {
            // Init something here
            InitialPosition = spawnPoint;
            TrajectoryController = trajectoryController;
            _waitForSeconds = new WaitForSeconds(DelayedDisableTime);
            StartCoroutine(DelayedDisable(DelayedDisableTime));
        }

        public IEnumerator DelayedDisable(float time)
        {
            yield return _waitForSeconds;
            OnCollisionEnter(null);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollision?.Invoke(this, collision);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            OnCollision = null;
        }
    }
}

