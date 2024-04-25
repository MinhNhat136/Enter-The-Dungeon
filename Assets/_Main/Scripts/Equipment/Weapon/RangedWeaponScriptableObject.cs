using System;
using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public enum ShootModeEnum
    {
        Single,
        Spread,
        Continuous,
    }

    public enum IndicatorParent
    {
        Character, 
        ShootPoint,
    }
    
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
        public ShootModeEnum shootMode;
        public ProjectileTrajectoryEnum trajectoryType;
        public IndicatorParent indicatorParent; 
        
        [Header("CONFIG", order = 2)]
        public ProjectileConfigScriptableObject projectileConfig;
        
        [Header("INDICATOR", order = 3)]
        public GameObject indicatorPrefab;
        public Vector3 indicatorPosition;
        public Vector3 indicatorRotation;

        [Header("PARAMETER", order = 4)] 
        public MinMaxSlider rangeAttack;
        public float criticalChance;
        
        private ObjectPool<ProjectileBase> _projectilePool;
        private IProjectileTrajectoryController _trajectoryController;
        private ITrajectoryIndicator _trajectoryIndicator;
        private ParticleSystem _shootSystem;
        private GameObject _indicator;

        private Vector3 _shootPoint; 
        
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
            SetIndicator();
        }

        public override void Detach()
        {
            base.Detach();
            if (_projectilePool != null)
            {
                _projectilePool.Clear();
            }

            _trajectoryController = null;
            _trajectoryIndicator = null;
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

        private void SetIndicator()
        {
            _indicator = Instantiate(indicatorPrefab, Owner.transform, true);
            _indicator.transform.localPosition = indicatorPosition;
            _indicator.transform.localRotation = Quaternion.Euler(indicatorRotation);
            _indicator.SetActive(false);

            _trajectoryIndicator = _indicator.GetComponent<ITrajectoryIndicator>();
        }

        public void BeginCharge()
        {
            _trajectoryIndicator.TurnOn();
        }

        public void UpdateCharge()
        {
            _trajectoryIndicator.Indicate();
            _shootPoint = _shootSystem.transform.forward;
            Debug.Log(_shootPoint);
        }

        public void EndCharge()
        {
            
        }

        public void CancelCharge()
        {
            _trajectoryIndicator.TurnOff();
        }

        public void Shoot()
        {
            Debug.Log("shoot");
            Debug.Log(_shootPoint);

            _trajectoryIndicator.TurnOff();
            
            _shootSystem.Play();
            Vector3 shootDirection = _shootSystem.transform.forward;
            shootDirection.Normalize();

            ProjectileBase bullet = _projectilePool.Get();
            bullet.gameObject.SetActive(true);

            bullet.Spawn(_shootSystem.transform.position, _trajectoryController);
            bullet.Shoot(Owner, _shootPoint);
        }

        private ProjectileBase CreateProjectile()
        {
            ProjectileBase instance = Instantiate(projectileConfig.bulletPrefab);
            instance.gameObject.SetActive(false);

            return instance;
        }

        public object Clone()
        {
            RangedWeaponScriptableObject config = CreateInstance<RangedWeaponScriptableObject>();

            config.WeaponType = WeaponType;
            config.name = name;
            config.projectileConfig = projectileConfig.Clone() as ProjectileConfigScriptableObject;

            config.WeaponPrefab = WeaponPrefab;
            config.WeaponSpawnPoint = WeaponSpawnPoint;
            config.WeaponSpawnRotation = WeaponSpawnRotation;

            return config;
        }

        //  Event Handlers --------------------------------
    }
}