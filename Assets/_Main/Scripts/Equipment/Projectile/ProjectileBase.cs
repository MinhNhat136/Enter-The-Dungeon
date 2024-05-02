using System;
using Atomic.Character;
using Atomic.Core;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileBase : MonoBehaviour, IEnergyConsumer<ProjectileBase>
    {
        //  Events ----------------------------------------
        public delegate void TriggerEvent(ProjectileBase projectile, Collider collider);
        public event TriggerEvent OnTrigger;

        //  Properties ------------------------------------
        public float EnergyValue { get; set; }
        public float MinEnergyValue { get; set; }
        public float MaxEnergyValue { get; set; }

        public BaseAgent Owner { get; private set; }
        public Vector3 ShootPosition { get; private set; }
        public Vector3 ShootTarget { get; set; }
        public Rigidbody RigidBody {get; private set; }

        //  Fields ----------------------------------------
        private MinMaxFloat _distanceWeight;
        private MinMaxFloat _velocityWeight;
            
        //  Initialization  -------------------------------
        public void Spawn(BaseAgent owner)
        {
            Owner = owner;
            gameObject.SetActive(false);
        }

        public ProjectileBase SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            _distanceWeight = distanceWeight;
            return this;
        }

        public ProjectileBase SetSpeedWeight(MinMaxFloat speedWeight)
        {
            _velocityWeight = speedWeight;
            return this;
        }
        
        //  Unity Methods   -------------------------------
        public void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }
        
        private void OnDisable()
        {
            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
        }

        //  Other Methods ---------------------------------
        public void Shoot(Vector3 shootPosition, Vector3 shootDirection, Vector3 shootTarget, float energyValue)
        {
            ShootPosition = shootPosition;
            ShootTarget = shootTarget;
            EnergyValue = energyValue;
            
            transform.position = shootPosition;
            transform.forward = shootDirection;
        }

        public void Update()
        {
            /*transform.position += transform.forward * (MaxEnergyValue * _velocityWeight) * Time.deltaTime;
            if (Vector3.Distance(transform.position, ShootPosition) >= _distanceWeight * EnergyValue)
            {
                OnTriggerEnter(null);
            }*/
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger?.Invoke(this, other);
        }
        
        //  Event Handlers --------------------------------
        
    }
}

