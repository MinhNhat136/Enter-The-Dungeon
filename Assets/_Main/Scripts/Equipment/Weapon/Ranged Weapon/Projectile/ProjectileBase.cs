using System;
using System.Collections.Generic;
using Atomic.Character;
using Atomic.Collection;
using Atomic.Core;
using Atomic.Damage;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ProjectileBase : MonoBehaviour, IEnergyConsumer<ProjectileBase>, IObjectPoolable
    {
        //  Events ----------------------------------------
        public delegate void ReleaseProjectile(ProjectileBase projectile);
        public ReleaseProjectile Release;

        
        //  Properties ------------------------------------
        public float EnergyValue { get; set; }

        public BaseAgent Owner { get; set; }
        public Vector3 ShootPosition { get; protected set; }
        protected Vector3 ShootTarget { get; set;}
        public List<PassiveEffect> PassiveEffect { get; set; } = new(8);
        [HideInInspector] public ObjectPool<ParticleSystem> HitVfx;
        [SerializeField] protected ParticleSystem hitVfxPrefab;
        [SerializeField] protected ParticleSystem trail;

        //  Fields ----------------------------------------
        protected MinMaxFloat DistanceWeight { get; private set; }
        protected MinMaxFloat SpeedWeight { get; private set; }
        public MinMaxFloat ForceWeight { get; private set; }
        
        protected ActionOnHit actionOnHit;
        protected ActionOnShoot actionOnShoot;
        
        public Transform MyTransform;
        
        //  Initialization  -------------------------------
        public virtual ProjectileBase Spawn(BaseAgent owner)
        {
            MyTransform = transform;
            Owner = owner;
            actionOnHit = GetComponent<ActionOnHit>();
            actionOnShoot = GetComponent<ActionOnShoot>();
            if (actionOnHit != null) actionOnHit.projectile = this;
            if (actionOnShoot != null) actionOnShoot.projectile = this;
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

        public ProjectileBase SetForceWeight(MinMaxFloat forceWeight)
        {
            ForceWeight = forceWeight;
            return this; 
        }
        // Unity Methods ----------------------------------
        private void Awake()
        {
            if (trail)
            {
                trail.Stop();
            }
        }

        private void OnDisable()
        {
            if (trail)
            {
                trail.Stop();
            }
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
            if (trail)
            {
                trail.Play();
            }
        }
        
        protected ParticleSystem CreateVFX()
        {
            ParticleSystem vfxInstance = Instantiate(hitVfxPrefab);
            vfxInstance.gameObject.SetActive(false);
            return vfxInstance;
        }

        protected void OnGetVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(true);
            vfx.Play();

            var autoRelease = vfx.GetComponent<AutoReleaseParticleSystem>();
            autoRelease.myPool = HitVfx;
            autoRelease.AutoRelease();
        }
        
        protected void OnReleaseVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(false);
            vfx.Stop();
        }
        
        protected void OnDestroyVFX(ParticleSystem vfx)
        {
            Destroy(vfx.gameObject);
        }

        public abstract void Shoot();
        protected abstract void ReleaseAfterDelay();

        //  Event Handlers --------------------------------

        public BasePoolSO<IObjectPoolable> PoolParent { get; set; }
        public void ReturnToPool()
        {
            throw new System.NotImplementedException();
        }
    }
}

