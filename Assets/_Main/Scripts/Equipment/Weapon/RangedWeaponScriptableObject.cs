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
    /// TODO: Replace with comments...
    /// </summary>
    [CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Ranged/Config", order = 0)]
    public class RangedWeaponScriptableObject : WeaponScriptableObject, System.ICloneable
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        [Header("INDICATOR", order = 0)] 
        public GameObject indicatorPrefab;
        public float delayActivateTime;
        
        [Header("BULLET", order = 1)]
        public ProjectileBase bulletPrefab;
        public LayerMask bulletHitMask;
        
        [Header("CONFIG", order = 2)] 
        public DamageConfigScriptableObject damageConfig;
        public HitEffectConfigScriptableObject hitConfig;

        [Header("METABOLISM WEIGHT", order = 3)]
        public float speedCharge;
        public MinMaxFloat damageWeight;
        public MinMaxFloat speedWeight;
        public MinMaxFloat distanceWeight;
        public MinMaxFloat radiusWeight;
        public MinMaxFloat areaOfEffectDistance;
        public MinMaxFloat gravityDownAcceleration;
        
        private ITrajectoryIndicator _trajectoryIndicator;
        private ObjectPool<ProjectileBase> _projectilePool;

        private float _energyValue;
        private Vector3 _targetPosition;
        private ParticleSystem _shootSystem;
        private GameObject _indicator;

        //  Initialization  -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            _shootSystem = Model.GetComponentInChildren<ParticleSystem>();
            Model.transform.SetParent(parent);
            
            CreatePool();
            CreateIndicator();
        }

        private void CreatePool()
        {
            _projectilePool = new ObjectPool<ProjectileBase>(CreateProjectile);
        }
        
        private ProjectileBase CreateProjectile()
        {
            ProjectileBase projectileInstance = Instantiate(bulletPrefab);
            projectileInstance.gameObject.SetActive(false);
            projectileInstance.Spawn(owner: Owner);
            projectileInstance
                .SetDistanceWeight(distanceWeight)
                .SetVelocityWeight(speedWeight);
            projectileInstance.OnTrigger += HandleBulletTrigger;

            return projectileInstance;
        }

        private void CreateIndicator()
        {
            _indicator = Instantiate(indicatorPrefab, Owner.transform, false);

            _trajectoryIndicator = _indicator.GetComponent<ITrajectoryIndicator>();
            _trajectoryIndicator.DelayActivateTime = delayActivateTime;
            _trajectoryIndicator
                .SetPosition(Owner.transform.position)
                .SetLaunchTransform(_shootSystem.transform)
                .SetForwardDirection(_shootSystem.transform)
                .SetDistanceWeight(distanceWeight)
                .SetScaleWeight(areaOfEffectDistance)
                .SetVelocityWeight(speedWeight);
            _trajectoryIndicator.Set();
            _trajectoryIndicator.DeActivate();
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void Detach()
        {
            base.Detach();
            _shootSystem = null;
            if (_projectilePool != null)
            {
                _projectilePool.Clear();
            }

            _indicator = null;
            _trajectoryIndicator = null;
        }

        public void BeginCharge()
        {
            _trajectoryIndicator.Activate();
        }

        public void UpdateCharge()
        {
            // _energyValue = Mathf.Lerp(_energyValue, maxEnergy, speedCharge * Time.deltaTime);
            if (Owner.TargetAgent != null)
            {
                _targetPosition = Owner.TargetAgent.transform.position;
                _targetPosition.y = 0;
            }
            else _targetPosition = Vector3.zero;
            _trajectoryIndicator.SetTarget(_targetPosition);
            _trajectoryIndicator.EnergyValue = _energyValue;
            _trajectoryIndicator.Indicate();
        }

        public void CancelCharge()
        {
            _trajectoryIndicator.DeActivate();
        }

        public void DoProjectileShoot()
        {
            _shootSystem.Play();
            Vector3 shootDirection = _shootSystem.transform.forward;
            shootDirection.Normalize();

            ProjectileBase bullet = _projectilePool.Get();
            bullet.gameObject.SetActive(true);
            bullet.Shoot(_shootSystem.transform.position, _shootSystem.transform.forward, _targetPosition, _energyValue);

            // _energyValue = minEnergy;
            _trajectoryIndicator.EnergyValue = _energyValue;
            _trajectoryIndicator.Indicate();
            _trajectoryIndicator.DeActivate();
        }

        private void HandleBulletTrigger(ProjectileBase projectile, Collider collider)
        {
            // if (collider == null)
            // {
            //     DisableProjectile(projectile);
            //     return;
            // }
            //
            // ;
            // if (collider.gameObject.TryGetComponent(out BaseAgent agent) &&
            //     collider.gameObject.layer == shootBuilder.HitMask)
            // {
            //     agent.HitBoxController.ApplyDamage(new DamageMessage()
            //     {
            //         Damager = this.Owner,
            //         Amount = 10,
            //     });
            // }
        }

        private void DisableProjectile(ProjectileBase Bullet)
        {
            Bullet.gameObject.SetActive(false);
            _projectilePool.Release(Bullet);
        }

        private void HandleBulletImpact(float DistanceTravelled, Vector3 HitLocation, Vector3 HitNormal, Collider HitCollider, int ObjectsPenetrated = 0)
        {
        }

        public object Clone()
        {
            RangedWeaponScriptableObject config = CreateInstance<RangedWeaponScriptableObject>();

            config.WeaponType = WeaponType;
            config.name = name;

            config.damageConfig = damageConfig.Clone() as DamageConfigScriptableObject;

            config.WeaponPrefab = WeaponPrefab;
            config.WeaponSpawnPoint = WeaponSpawnPoint;
            config.WeaponSpawnRotation = WeaponSpawnRotation;

            return config;
        }

        //  Event Handlers --------------------------------
    }
}