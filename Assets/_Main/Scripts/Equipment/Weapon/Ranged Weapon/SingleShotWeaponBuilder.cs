using UnityEngine;

namespace  Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class SingleShotWeaponBuilder : RangedWeaponBuilder
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------
        public Vector3 TargetPosition { get; private set; }

        //  Initialization  -------------------------------

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

            projectileSpawnAbilityScriptableObject.projectile = projectile;
            var effectSpec = projectileSpawnAbilityScriptableObject.CreateSpec(Owner.AiAbilityController.abilitySystemController);
            StartCoroutine(effectSpec.TryActivateAbility());     
            
            BarrelController.Shoot();
        }

        public override void EndShoot()
        {
            EnergyValue = 0;
            if (TrajectoryIndicator != null) TrajectoryIndicator.EnergyValue = EnergyValue;
            TrajectoryIndicator?.Indicate();
            TrajectoryIndicator?.DeActivate();
        }
        
        //  Event Handlers --------------------------------
        protected override void OnGetProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        protected override void OnReleaseProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        protected override void OnDestroyProjectile(ProjectileBase projectile)
        {
            Destroy(projectile);
        }

    }
}
