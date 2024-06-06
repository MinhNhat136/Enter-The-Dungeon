using Atomic.AbilitySystem;
using Atomic.Character;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    public abstract class RangedWeaponBuilder : WeaponBuilder
    {
        [Header("COMPONENT TRANSFORM")] 
        public Transform barrelTransform;
        
        [Header("INDICATOR", order = 0)] 
        public GameObject indicatorPrefab;
        public float delayActivateTime;

        [Header("BULLET", order = 1)] 
        public ProjectileBase bulletPrefab;
        public MinMaxFloat projectileContains;

        [Header("BARREL", order = 2)] 
        public BarrelController barrelPrefab;

        [Header("ENERGY", order = 3)] 
        public float speedCharge;

        [FormerlySerializedAs("projectileAbilityScriptableObject")] public ProjectileSpawnAbilityScriptableObject projectileSpawnAbilityScriptableObject;

        protected float EnergyValue { get; set; }
        protected ObjectPool<ProjectileBase> ProjectilePool { get; set; }
        protected ITrajectoryIndicator TrajectoryIndicator { get; private set; }
        protected BarrelController BarrelController { get; set; }
        
        public override void Attach(BaseAgent owner)
        {
            base.Attach(owner);

            CreatePool();
            CreateBarrel();
            CreateIndicator();
        }

        private void CreateIndicator()
        {
            if (indicatorPrefab)
            {
                var indicator = Instantiate(indicatorPrefab, Owner.transform, false);
                TrajectoryIndicator = indicator.GetComponent<ITrajectoryIndicator>();
                TrajectoryIndicator.DelayActivateTime = delayActivateTime;
                TrajectoryIndicator
                    .SetPosition(Owner.transform.position)
                    .SetDistanceWeight(distanceWeight)
                    .SetRadiusWeight(radiusWeight)
                    .SetAoEWeight(areaOfEffectDistance)
                    .SetDamageWeight(damageWeight)
                    .SetSpeedWeight(speedWeight)
                    .SetForceWeight(forceWeight)
                    .Initialize();
                TrajectoryIndicator.DeActivate();
            }
        }

        private void CreateBarrel()
        {
            if (!barrelPrefab)
                return;
            BarrelController = Instantiate(barrelPrefab, barrelTransform, false);
        }

        private void CreatePool()
        {
            ProjectilePool = new ObjectPool<ProjectileBase>
            (
                CreateProjectile,
                OnGetProjectile,
                OnReleaseProjectile,
                OnDestroyProjectile,
                true,
                (int)projectileContains.min,
                (int)projectileContains.max);
        }

        private ProjectileBase CreateProjectile()
        {
            ProjectileBase projectileInstance = Instantiate(bulletPrefab);
            projectileInstance
                .Spawn(owner: Owner)
                .SetDistanceWeight(distanceWeight)
                .SetSpeedWeight(speedWeight)
                .SetForceWeight(forceWeight);
            projectileInstance.ProjectilePool = ProjectilePool;
            return projectileInstance;
        }

        public override void Detach()
        {
            base.Detach();
            TrajectoryIndicator = null;
            ProjectilePool?.Clear();
        }

        public abstract void BeginCharge();

        public abstract void UpdateCharge();

        public abstract void CancelCharge();

        public abstract void Shoot();

        public abstract void EndShoot();

        protected Vector3 FindTarget()
        {
            if (Owner.TargetAgent == null
                || (Owner.TargetAgent != null &&
                    Vector3.Distance(Owner.transform.position, Owner.TargetAgent.transform.position) >
                    distanceWeight.max))
            {
                var forwardTarget = Quaternion.LookRotation(Owner.transform.forward, Vector3.up);
                return Owner.transform.position +
                       forwardTarget * Vector3.forward
                                     * distanceWeight.GetValueFromRatio(EnergyValue);
            }

            return Owner.TargetAgent.transform.position;
        }

        protected void AimTarget(Vector3 target)
        {
            if (TrajectoryIndicator == null)
            {
                return;
            }
            TrajectoryIndicator
                .SetStartPosition(BarrelController.transform)
                .SetEndPosition(target);
            TrajectoryIndicator.EnergyValue = EnergyValue;
            TrajectoryIndicator.Indicate();
        }
        
        protected abstract void OnGetProjectile(ProjectileBase projectile);

        protected abstract void OnReleaseProjectile(ProjectileBase projectile);

        protected abstract void OnDestroyProjectile(ProjectileBase projectile);
    }
}