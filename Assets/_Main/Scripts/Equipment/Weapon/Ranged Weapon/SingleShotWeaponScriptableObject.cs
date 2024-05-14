using Atomic.Character;
using UnityEngine;

namespace  Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapons/Ranged/Config/Single Shot", order = 1)]
    public class SingleShotWeaponScriptableObject : RangedWeaponScriptableObject
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------
        protected Vector3 _targetPosition;

        //  Initialization  -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void BeginCharge()
        {
            trajectoryIndicator.Activate();
            barrelController.Charge();
        }

        public override void UpdateCharge()
        {
            energyValue = Mathf.Clamp01(energyValue + speedCharge * Time.deltaTime);
            _targetPosition = FindTarget();
            AimTarget(_targetPosition);
        }

        public override void CancelCharge()
        {
            trajectoryIndicator.DeActivate();
        }

        public override void Shoot()
        {
            Vector3 shootDirection = barrelController.shootSystem.transform.forward;
            shootDirection.Normalize();
            
            var projectile = projectilePool.Get();
            projectile.Load(barrelController.shootSystem.transform.position, 
                barrelController.transform.forward,
                _targetPosition, energyValue);
            foreach (var effect in effectBuilders)
            {
                projectile.PassiveEffect.Add(effect.CreatePassiveEffect());
            }
            barrelController.Shoot(projectile);
        }

        public override void EndShoot()
        {
            energyValue = 0;
            trajectoryIndicator.EnergyValue = energyValue;
            trajectoryIndicator.Indicate();
            trajectoryIndicator.DeActivate();
        }
        
        //  Event Handlers --------------------------------
        protected override void ReleaseProjectile(ProjectileBase projectile)
        {
            projectile.PassiveEffect.Clear();
            projectilePool.Release(projectile);
        }

        protected override void OnGetProjectile(ProjectileBase projectile)
        {
            projectile.Release += ReleaseProjectile;
            projectile.gameObject.SetActive(true);
        }

        protected override void OnReleaseProjectile(ProjectileBase projectile)
        {
            projectile.Release -= ReleaseProjectile;
            projectile.gameObject.SetActive(false);
        }

        protected override void OnDestroyProjectile(ProjectileBase projectile)
        {
            Destroy(projectile);
        }
    }
}
