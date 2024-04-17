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
        public UnityAction OnShoot;
        public event Action OnShootProcessed;

        //  Properties ------------------------------------
        public GameObject SourcePrefab { get; set; }
        public Transform WeaponMuzzle { get; set; }
        public ProjectileBase ProjectilePrefab { get; set; }
        public float MaxChargeDuration { get; set; } = 2f;
        public GameObject MuzzleFlashPrefab { get; set;}
        public float LastChargeTriggerTimestamp { get; private set; }
        public bool IsCharging { get; private set; }
        public float CurrentCharge { get; private set; }

        //  Fields ----------------------------------------

        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        public void HandleShootInput(bool inputDown, bool inputHold, bool inputRelease)
        {
            Debug.Log("shoot");
        }

        public void UpdateCharge()
        {
            
        }

        public void TryBeginCharge()
        {
            
        }

        public void TryShoot()
        {
            Debug.Log("try shoot");
        }

        public void TryReleaseCharge()
        {
            
        }

        
    }
}

