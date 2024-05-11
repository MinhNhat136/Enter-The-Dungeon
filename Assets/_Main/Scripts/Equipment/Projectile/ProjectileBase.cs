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
    public abstract class ProjectileBase : MonoBehaviour, IEnergyConsumer<ProjectileBase>
    {
        //  Events ----------------------------------------
        public delegate void OnTrigger(ProjectileBase projectile, Collider other);
        public OnTrigger OnProjectileTrigger;
        
        //  Properties ------------------------------------
        public float EnergyValue { get; set; }
        
        public BaseAgent Owner { get; protected set; }
        public Vector3 ShootPosition { get; protected set; }
        public Vector3 ShootTarget { get; set; }
        public LayerMask HitMask { get; set; }

        //  Fields ----------------------------------------
        protected MinMaxFloat DistanceWeight { get; private set; }
        protected MinMaxFloat SpeedWeight { get; private set; }
            
        //  Initialization  -------------------------------
        public virtual ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            Owner = owner;
            HitMask = hitMask;

            return this;
        }

        public ProjectileBase SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            DistanceWeight = distanceWeight;
            return this;
        }

        public ProjectileBase SetSpeedWeight(MinMaxFloat speedWeight)
        {
            SpeedWeight = speedWeight;
            return this;
        }
        
        //  Other Methods ---------------------------------
        public virtual void Load(Vector3 shootPosition, Vector3 shootDirection, Vector3 shootTarget, float energyValue)
        {
            ShootPosition = shootPosition;
            ShootTarget = shootTarget;
            EnergyValue = energyValue;

            var projectileTransform = transform;
            projectileTransform.position = shootPosition;
            projectileTransform.forward = shootDirection;
        }

        public abstract void Shoot();
        public abstract void OnHit(Vector3 point, Vector3 normal, Collider collide);
        protected abstract void TriggerOnCollisionAfterDelay();

        //  Event Handlers --------------------------------
        
    }
}

