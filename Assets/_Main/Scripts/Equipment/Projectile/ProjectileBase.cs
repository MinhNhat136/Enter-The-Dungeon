using System.Collections.Generic;
using Atomic.Character;
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
    public abstract class ProjectileBase : MonoBehaviour, IEnergyConsumer<ProjectileBase>
    {
        //  Events ----------------------------------------
        public delegate void ReleaseProjectile(ProjectileBase projectile);
        public ReleaseProjectile Release;

        
        //  Properties ------------------------------------
        public float EnergyValue { get; set; }

        public BaseAgent Owner { get; set; }
        protected Vector3 ShootPosition { get; set; }
        protected Vector3 ShootTarget { get; set; }
        protected LayerMask HitMask { get; set; }
        public List<PassiveEffect> PassiveEffect { get; set; } = new(8);
        [HideInInspector] public ObjectPool<ParticleSystem> HitVfx;
        [SerializeField] protected ParticleSystem hitVfxPrefab;

        //  Fields ----------------------------------------
        protected MinMaxFloat DistanceWeight { get; private set; }
        protected MinMaxFloat SpeedWeight { get; private set; }
        protected ActionOnHit actionOnHit;
        
        protected Transform MyTransform;
        
        //  Initialization  -------------------------------
        public virtual ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            MyTransform = transform;
            Owner = owner;
            HitMask = hitMask;
            actionOnHit = GetComponent<ActionOnHit>();
            actionOnHit.projectile = this;
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
        
    }
}

