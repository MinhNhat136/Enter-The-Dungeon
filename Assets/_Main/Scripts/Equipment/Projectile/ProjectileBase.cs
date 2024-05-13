using System;
using System.Collections.Generic;
using Atomic.Character;
using Atomic.Core;
using Atomic.Damage;
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
        public delegate void ReleaseProjectile(ProjectileBase projectile);
        public ReleaseProjectile Release;

        
        //  Properties ------------------------------------
        public float EnergyValue { get; set; }

        protected BaseAgent Owner { get; set; }
        protected Vector3 ShootPosition { get; set; }
        protected Vector3 ShootTarget { get; set; }
        protected LayerMask HitMask { get; set; }
        public List<PassiveEffect> PassiveEffect { get; set; } = new(8);

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
        public abstract void OnHit(Vector3 point, Vector3 normal);
        protected abstract void ReleaseAfterDelay();

        //  Event Handlers --------------------------------
        
    }
}

