using Atomic.Character;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    
    /// <summary>
    /// This class is responsible for managing the properties and behaviors of a ranged weapon,
    /// including its indicator, barrel, magazine, and various weight properties for metabolism.
    /// </summary>
    [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapons/Ranged/Config/Default", order = 0)]
    public abstract class RangedWeaponScriptableObject : WeaponScriptableObject, System.ICloneable
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        [Header("INDICATOR", order = 0)] 
        public GameObject indicatorPrefab;
        public float delayActivateTime;
        
        [Header("BULLET", order = 1)] 
        public ProjectileBase bulletPrefab;
        public LayerMask projectileHitMask;
        public MinMaxFloat projectileContains;

        [FormerlySerializedAs("barrel")] [Header("BARREL", order = 2)] 
        public BarrelController barrelPrefab;
        
        [Header("METABOLISM WEIGHT", order = 5)]
        public float speedCharge;
        public MinMaxFloat damageWeight;
        public MinMaxFloat speedWeight;
        public MinMaxFloat distanceWeight;
        public MinMaxFloat radiusWeight;
        public MinMaxFloat areaOfEffectDistance;
        public MinMaxFloat gravityDownAcceleration;
        
        protected float energyValue;
        protected ObjectPool<ProjectileBase> projectilePool;
        protected GameObject indicator;
        protected ITrajectoryIndicator trajectoryIndicator;
        protected BarrelController barrelController;
        
        private RangedWeaponAssembler _weaponAssembler; 

        //  Initialization  -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            _weaponAssembler = Model.GetComponent<RangedWeaponAssembler>();
            Model.transform.SetParent(parent);
            
            CreatePool();
            CreateBarrel();
            CreateIndicator();
        }

        private void CreateIndicator()
        {
            indicator = Instantiate(indicatorPrefab, Owner.transform, false);
            
            trajectoryIndicator = indicator.GetComponent<ITrajectoryIndicator>();
            trajectoryIndicator.DelayActivateTime = delayActivateTime;
            trajectoryIndicator
                .SetPosition(Owner.transform.position)
                .SetDistanceWeight(distanceWeight)
                .SetRadiusWeight(radiusWeight)
                .SetAoEWeight(areaOfEffectDistance)
                .SetGravityDownAcceleration(gravityDownAcceleration)
                .SetDamageWeight(damageWeight)
                .SetSpeedWeight(speedWeight)
                .Initialize();
            trajectoryIndicator.DeActivate();
        }

        private void CreateBarrel()
        {
            barrelController = Instantiate(barrelPrefab,
                _weaponAssembler.GetAttachTransform(WeaponComponentEnum.Barrel), false);
        }

        private void CreatePool()
        {
            projectilePool = new ObjectPool<ProjectileBase>
            (
                CreateProjectile, 
                OnGetProjectile, 
                OnReclaimProjectile, 
                OnDestroyProjectile, 
                true, 
                (int)projectileContains.min, 
                (int)projectileContains.max);
        }
        
        private ProjectileBase CreateProjectile()
        {
            ProjectileBase projectileInstance = Instantiate(bulletPrefab);
            projectileInstance.gameObject.SetActive(false);
            projectileInstance
                .Spawn(owner: Owner, hitMask: projectileHitMask)
                .SetDistanceWeight(distanceWeight)
                .SetSpeedWeight(speedWeight);
            
            return projectileInstance;
        }
        
        public override void Detach()
        {
            base.Detach();
            indicator = null;
            trajectoryIndicator = null;
            projectilePool?.Clear();

        }
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public abstract void BeginCharge();

        public abstract void UpdateCharge();

        public abstract void CancelCharge();
        
        public abstract void Shoot();

        public abstract void EndShoot();
        
        protected Vector3 FindTarget()
        {
            if (Owner.TargetAgent == null 
                || (Owner.TargetAgent != null && 
                    Vector3.Distance(Owner.transform.position, Owner.TargetAgent.transform.position) > distanceWeight.max))
            {
                var forwardTarget = Quaternion.LookRotation(Owner.transform.forward, Vector3.up);
                return Owner.transform.position + 
                               forwardTarget * Vector3.forward 
                                             * distanceWeight.GetValueFromRatio(energyValue);
            }
            return Owner.TargetAgent.transform.position;
        }
        
        protected void AimTarget(Vector3 target)
        {
            trajectoryIndicator
                .SetStartPosition(barrelController.transform)
                .SetEndPosition(target);
            trajectoryIndicator.EnergyValue = energyValue;
            trajectoryIndicator.Indicate();
        }
        
        public new object Clone()
        {
            base.Clone();
            RangedWeaponScriptableObject config = CreateInstance<RangedWeaponScriptableObject>();

            config.weaponType = weaponType;
            config.name = name;
            
            config.weaponPrefab = weaponPrefab;
            config.weaponSpawnPoint = weaponSpawnPoint;
            config.weaponSpawnRotation = weaponSpawnRotation;

            return config;
        }

        //  Event Handlers --------------------------------
        protected abstract void OnProjectileCollide(ProjectileBase projectile, Collider other);

        protected abstract void OnGetProjectile(ProjectileBase projectile);

        protected abstract void OnReclaimProjectile(ProjectileBase projectile);

        protected abstract void OnDestroyProjectile(ProjectileBase projectile);
        
    }
}