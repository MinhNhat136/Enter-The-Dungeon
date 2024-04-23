using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class RangedWeapon : Weapon
    {
        //  Events ----------------------------------------
        private event Action _onChargeFull;


        //  Properties ------------------------------------
        public GameObject SourcePrefab { get; set; }
        public Transform WeaponMuzzle { get; set; }
        public float AttackRanged { get; set; }

        public ProjectileBase ProjectilePrefab => _projectilePrefab;
        
        [field: SerializeField]
        public float MaxChargeDuration { get; set; } = 2f;
        
        public GameObject MuzzleFlashPrefab { get; set; }
        public float LastChargeTriggerTimestamp { get; private set; }
        public bool IsCharging { get; private set; }
        public float CurrentCharge { get; private set; }

        public event Action OnChargeFull
        {
            add
            {
                _onChargeFull += value; 
            }
            remove
            {
                _onChargeFull -= value; 
            }
        }
        //  Fields ----------------------------------------
        [SerializeField]
        private ProjectileBase _projectilePrefab;

        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void HandleShootInput(bool inputDown, bool inputHold, bool inputRelease)
        {
        }

        public void UpdateCharge()
        {
            if (!IsCharging) return;
            if (CurrentCharge >= MaxChargeDuration)
            {
                IsCharging = false;
                _onChargeFull?.Invoke();
            }

            CurrentCharge += Time.deltaTime;
        }

        public void TryBeginCharge()
        {
            IsCharging = true;
        }

        public void ReleaseCharge()
        {
            CurrentCharge = 0f;
            IsCharging = false;
        }

        public void HandleShoot()
        {
            /*ProjectileBase newProjectile = Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
            newProjectile.Shoot(this);*/
        }

        //  Event Handlers --------------------------------
    }
}