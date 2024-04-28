using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;
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
        [Header("RANGED ENUM", order = 0)]
        public ProjectileTrajectoryEnum trajectoryType;

        [Header("CONFIG", order = 2)]
        public ShootConfigScriptableObject shootConfig;
        public DamageConfigScriptableObject damageConfig;
        public HitEffectConfigScriptableObject hitConfig;
        public IndicatorConfigScriptableObject indicatorConfig; 

        [Header("PARAMETER", order = 4)] 
        public float velocity;
        public MinMaxSlider rangeAttack;
        public float criticalChance;
        
        private ObjectPool<ProjectileBase> _projectilePool;
        private IProjectileTrajectoryController _trajectoryController;
        private ParticleSystem _shootSystem;
        private GameObject _indicator;

        
        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            _shootSystem = Model.GetComponentInChildren<ParticleSystem>();
            _projectilePool = new ObjectPool<ProjectileBase>(CreateProjectile);
            Model.transform.SetParent(parent);

            SetTrajectoryController();
            indicatorConfig.Initialize(owner);
        }

        public override void Detach()
        {
            base.Detach();
            if (_projectilePool != null)
            {
                _projectilePool.Clear();
            }

            indicatorConfig.Destroy();
            
            _trajectoryController = null;
            _shootSystem = null;
            _indicator = null;
        }

        private void SetTrajectoryController()
        {
            switch (trajectoryType)
            {
                case ProjectileTrajectoryEnum.Straight:
                    _trajectoryController = new StraightTrajectory();
                    break;
                case ProjectileTrajectoryEnum.VerticalArc:
                    _trajectoryController = new VerticalArcTrajectory();
                    break;
                case ProjectileTrajectoryEnum.HorizontalArc:
                    _trajectoryController = new HorizontalArcTrajectory();
                    break;
                default:
                    _trajectoryController = null;
                    break;
            }
        }

        public void BeginCharge()
        {
            indicatorConfig.TurnOn();
        }
        
        public void UpdateCharge()
        {
            indicatorConfig.trajectoryIndicator.SetLaunchPosition(_shootSystem.transform.position);
            if(Owner.TargetAgent) 
                indicatorConfig.trajectoryIndicator.SetTarget(Owner.TargetAgent.transform);
            else
            {
                indicatorConfig.trajectoryIndicator.SetTarget(null);
            }
            indicatorConfig.Indicate();
            // Owner.StartCoroutine(indicatorConfig.Indicate());

        }

        public void EndCharge()
        {
        }

        public void CancelCharge()
        {
            indicatorConfig.TurnOff();
        }

        public void DoProjectileShoot()
        {
            indicatorConfig.trajectoryIndicator.DeActivate();
            
            _shootSystem.Play();
            Vector3 shootDirection = _shootSystem.transform.forward;
            shootDirection.Normalize();

            ProjectileBase bullet = _projectilePool.Get();
            bullet.gameObject.SetActive(true);

            bullet.Shoot(_shootSystem.transform.position, new Vector3(Owner.transform.forward.x, _shootSystem.transform.forward.y, Owner.transform.forward.z), velocity);
        }

        private void HandleBulletCollision(ProjectileBase projectile, Collision collision)
        {
            if (collision == null)
            {
                DisableProjectile(projectile);
                return;
            };
            if (collision.gameObject.TryGetComponent(out BaseAgent agent) && collision.gameObject.layer == shootConfig.HitMask)
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
        
        private void HandleBulletImpact(
            float DistanceTravelled,
            Vector3 HitLocation,
            Vector3 HitNormal,
            Collider HitCollider,
            int ObjectsPenetrated = 0)
        {
           
        }

        private ProjectileBase CreateProjectile()
        {
            ProjectileBase instance = Instantiate(shootConfig.bulletPrefab);
            instance.gameObject.SetActive(false);
            instance.Spawn(Owner, _trajectoryController.Clone(), shootConfig.delayDisableTime);
            instance.OnCollision += HandleBulletCollision;

            return instance;
        }

        public object Clone()
        {
            RangedWeaponScriptableObject config = CreateInstance<RangedWeaponScriptableObject>();

            config.WeaponType = WeaponType;
            config.name = name;
            
            config.shootConfig = shootConfig.Clone() as ShootConfigScriptableObject;
            config.indicatorConfig = indicatorConfig.Clone() as IndicatorConfigScriptableObject;
            config.damageConfig = damageConfig.Clone() as DamageConfigScriptableObject;
            
            config.WeaponPrefab = WeaponPrefab;
            config.WeaponSpawnPoint = WeaponSpawnPoint;
            config.WeaponSpawnRotation = WeaponSpawnRotation;

            return config;
        }

        //  Event Handlers --------------------------------
    }
}