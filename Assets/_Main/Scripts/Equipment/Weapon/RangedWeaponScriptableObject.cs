using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;

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
        [Header("CONFIG", order = 0)]
        public ShootBuilderScriptableObject shootBuilder;
        public DamageConfigScriptableObject damageConfig;
        public HitEffectConfigScriptableObject hitConfig;
        
        [Header("PARAMETER", order = 1)] 
        public float minEnergy;
        public float maxEnergy;
        public float speedCharge;
        
        public float timeWeight;
        public float distanceWeight;
        public float radiusWeight;
        public float scaleWeight; 
        
        [Header("INDICATOR", order = 2)] 
        public GameObject indicatorPrefab;
        public Vector3 indicatorPosition;
        public float delayActivateTime;
        
        private float _energyValue;
        private ParticleSystem _shootSystem;
        private GameObject _indicator;
        public  ITrajectoryIndicator trajectoryIndicator;
        private ObjectPool<ProjectileBase> _projectilePool;

        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            _shootSystem = Model.GetComponentInChildren<ParticleSystem>();
            _projectilePool = new ObjectPool<ProjectileBase>(CreateProjectile);
            Model.transform.SetParent(parent);
            
            shootBuilder.Initialize(owner);
            
            _indicator = Instantiate(indicatorPrefab, owner.transform, false);

            trajectoryIndicator = _indicator.GetComponent<ITrajectoryIndicator>();
            trajectoryIndicator.DelayActivateTime = delayActivateTime;
            trajectoryIndicator
                .SetPosition(Owner.transform.position)
                .SetLaunchTransform(_shootSystem.transform)
                .SetForwardDirection(owner.transform.forward)
                .SetDistanceWeight(distanceWeight)
                .SetRadiusWeight(radiusWeight)
                .SetScaleWeight(scaleWeight)
                .SetTimeWeight(timeWeight)
                .Set();
            trajectoryIndicator.DeActivate();
        }

        public override void Detach()
        {
            base.Detach();
            _shootSystem = null;
            if (_projectilePool != null)
            {
                _projectilePool.Clear();
            }

            _indicator = null;
            trajectoryIndicator = null; 
            
            shootBuilder.Destroy();
        }

        public void BeginCharge()
        {
            trajectoryIndicator.Activate();
            _energyValue = minEnergy;

        }
        
        public void UpdateCharge()
        {
            _energyValue = Mathf.Lerp(_energyValue, maxEnergy, speedCharge * Time.deltaTime);
            Vector3 targetPosition = Vector3.zero;
            if (Owner.TargetAgent != null)
            {
                targetPosition = Owner.TargetAgent.transform.position;
            }
            trajectoryIndicator.SetTarget(targetPosition);
            trajectoryIndicator.IndicateValue = _energyValue;
            trajectoryIndicator.Indicate();
        }

        public void CancelCharge()
        {
            trajectoryIndicator.DeActivate();
        }

        public void DoProjectileShoot()
        {
            trajectoryIndicator.DeActivate();
            
            _shootSystem.Play();
            Vector3 shootDirection = _shootSystem.transform.forward;
            shootDirection.Normalize();

            ProjectileBase bullet = _projectilePool.Get();
            bullet.gameObject.SetActive(true);
            bullet.Load(_shootSystem.transform.position, new Vector3(Owner.transform.forward.x, _shootSystem.transform.forward.y, Owner.transform.forward.z), shootBuilder.shootVelocity, delayedDisableTime: _energyValue/shootBuilder.shootVelocity);
            
            shootBuilder.TrajectoryController.Shoot(bullet);
        }

        private void HandleBulletTrigger(ProjectileBase projectile, Collider collider)
        {
            if (collider == null)
            {
                DisableProjectile(projectile);
                return;
            };
            if (collider.gameObject.TryGetComponent(out BaseAgent agent) && collider.gameObject.layer == shootBuilder.HitMask)
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
            instance.Spawn(owner: Owner);
            instance.OnTrigger += HandleBulletTrigger;

            return instance;
        }

        public object Clone()
        {
            RangedWeaponScriptableObject config = CreateInstance<RangedWeaponScriptableObject>();

            config.WeaponType = WeaponType;
            config.name = name;
            
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