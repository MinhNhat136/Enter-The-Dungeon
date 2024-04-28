using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Ranged/Config", order = 0)]
    public class RangedWeaponScriptableObject : WeaponScriptableObject, System.ICloneable
    {
        //  Events ----------------------------------------

        
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        [Header("CONFIG", order = 2)]
        public IndicatorBuilderScriptableObject indicatorBuilder; 
        public ShootBuilderScriptableObject shootBuilder;
        public DamageConfigScriptableObject damageConfig;
        public HitEffectConfigScriptableObject hitConfig;
        
        private ObjectPool<ProjectileBase> _projectilePool;
        private ParticleSystem _shootSystem;
        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            _shootSystem = Model.GetComponentInChildren<ParticleSystem>();
            _projectilePool = new ObjectPool<ProjectileBase>(CreateProjectile);
            Model.transform.SetParent(parent);

            indicatorBuilder.Initialize(owner);
            shootBuilder.Initialize(owner);
        }

        public override void Detach()
        {
            base.Detach();
            _shootSystem = null;
            if (_projectilePool != null)
            {
                _projectilePool.Clear();
            }

            indicatorBuilder.Destroy();
            shootBuilder.Destroy();
        }

        public void BeginCharge()
        {
            indicatorBuilder.trajectoryIndicator.Activate();
        }
        
        public void UpdateCharge()
        {
            indicatorBuilder.trajectoryIndicator.SetLaunchPosition(_shootSystem.transform.position);
            indicatorBuilder.trajectoryIndicator.SetTarget(Owner.TargetAgent ? Owner.TargetAgent.transform : null);
            indicatorBuilder.trajectoryIndicator.Indicate();
        }

        public void EndCharge()
        {
        }

        public void CancelCharge()
        {
            indicatorBuilder.trajectoryIndicator.DeActivate();
        }

        public void DoProjectileShoot()
        {
            indicatorBuilder.trajectoryIndicator.DeActivate();
            
            _shootSystem.Play();
            Vector3 shootDirection = _shootSystem.transform.forward;
            shootDirection.Normalize();

            ProjectileBase bullet = _projectilePool.Get();
            bullet.gameObject.SetActive(true);
            bullet.Load(_shootSystem.transform.position, new Vector3(Owner.transform.forward.x, _shootSystem.transform.forward.y, Owner.transform.forward.z), shootBuilder.shootVelocity);
            
            shootBuilder.TrajectoryController.Shoot(bullet);
        }

        private void HandleBulletCollision(ProjectileBase projectile, Collision collision)
        {
            if (collision == null)
            {
                DisableProjectile(projectile);
                return;
            };
            if (collision.gameObject.TryGetComponent(out BaseAgent agent) && collision.gameObject.layer == shootBuilder.HitMask)
            {
                agent.HitBoxController.ApplyDamage(new DamageMessage()
                {
                    Damager = this.Owner,
                    Amount = 10,
                });
            }
        }
        
        private void DisableProjectile(ProjectileBase Bullet)
        {
            Debug.Log("disable");
            Bullet.gameObject.SetActive(false);
            _projectilePool.Release(Bullet);
        }
        
        private void HandleBulletImpact(float DistanceTravelled, Vector3 HitLocation, Vector3 HitNormal, Collider HitCollider,int ObjectsPenetrated = 0)
        {
           
        }

        private ProjectileBase CreateProjectile()
        {
            ProjectileBase instance = Instantiate(shootBuilder.bulletPrefab);
            instance.gameObject.SetActive(false);
            instance.Spawn(owner: Owner, delayedDisableTime: indicatorBuilder.maxDistance/shootBuilder.shootVelocity);
            instance.OnCollision += HandleBulletCollision;

            return instance;
        }

        public object Clone()
        {
            RangedWeaponScriptableObject config = CreateInstance<RangedWeaponScriptableObject>();

            config.WeaponType = WeaponType;
            config.name = name;
            
            config.indicatorBuilder = indicatorBuilder.Clone() as IndicatorBuilderScriptableObject;
            config.shootBuilder = shootBuilder.Clone() as ShootBuilderScriptableObject;
            config.damageConfig = damageConfig.Clone() as DamageConfigScriptableObject;
            
            config.WeaponPrefab = WeaponPrefab;
            config.WeaponSpawnPoint = WeaponSpawnPoint;
            config.WeaponSpawnRotation = WeaponSpawnRotation;

            return config;
        }

        //  Event Handlers --------------------------------
    }
}