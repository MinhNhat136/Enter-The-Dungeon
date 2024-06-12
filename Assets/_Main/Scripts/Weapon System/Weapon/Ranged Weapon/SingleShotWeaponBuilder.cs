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
            
            var effectSpec = projectileSpawnAbilityScriptableObject.CreateSpec(Owner,
                EnergyValue,
                BarrelController.shootSystem.transform.position,
                BarrelController.transform.forward,
                TargetPosition);
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


    }
}
