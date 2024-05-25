using Atomic.Character;
using Atomic.Core;
using UnityEngine;

namespace  Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapons/Ranged/Config/Single Shot", order = 1)]
    public class SingleShotWeaponBuilder : RangedWeaponBuilder
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------
        public Vector3 TargetPosition { get; private set; }

        //  Initialization  -------------------------------
        public override void Attach(Transform parent, BaseAgent owner)
        {
            base.Attach(parent, owner);
            
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void BeginCharge()
        {
            TrajectoryIndicator?.Activate();
            BarrelController.Charge();
        }

        public override void UpdateCharge()
        {
            EnergyValue = Mathf.Clamp01(EnergyValue + speedCharge * Time.deltaTime);
            TargetPosition = FindTarget();
            AimTarget(TargetPosition);
        }

        public override void CancelCharge()
        {
            TrajectoryIndicator?.DeActivate();
        }

        public override void Shoot()
        {
            Vector3 shootDirection = BarrelController.shootSystem.transform.forward;
            shootDirection.Normalize();
            
            var projectile = ProjectilePool.Get();
            projectile.Load(BarrelController.shootSystem.transform.position, 
                BarrelController.transform.forward,
                TargetPosition, EnergyValue);
            foreach (var effect in effectBuilders)
            {
                projectile.PassiveEffect.Add(effect.CreatePassiveEffect());
            }
            BarrelController.Shoot(projectile);
        }

        public override void EndShoot()
        {
            EnergyValue = 0;
            if (TrajectoryIndicator != null) TrajectoryIndicator.EnergyValue = EnergyValue;
            TrajectoryIndicator?.Indicate();
            TrajectoryIndicator?.DeActivate();
        }
        
        //  Event Handlers --------------------------------
        protected override void ReleaseProjectile(ProjectileBase projectile)
        {
            projectile.PassiveEffect.Clear();
            ProjectilePool.Release(projectile);
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
