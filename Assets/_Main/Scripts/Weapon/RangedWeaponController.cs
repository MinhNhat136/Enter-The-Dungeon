using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Weapon
{
    public class RangedWeaponController : MonoBehaviour
    {
        public string WeaponName;
        public Sprite WeaponIcon;
        public GameObject WeaponRoot;
        public Transform WeaponMuzzle;
        public ProjectileBase ProjectilePrefab;
        
        [Tooltip("Duration to reach maximum charge")]
        public float MaxChargeDuration = 2f;

        [Tooltip("Prefab of the muzzle flash")]
        public GameObject MuzzleFlashPrefab;

        public UnityAction OnShoot;
        public event Action OnShootProcessed;

        public float LastChargeTriggerTimestamp { get; private set; }
        Vector3 m_LastMuzzlePosition;

        public GameObject Owner { get; set; }
        public GameObject SourcePrefab { get; set; }
        public bool IsCharging { get; private set; }
        public bool IsWeaponActive { get; private set; }
        public float CurrentCharge { get; private set; }
        public Vector3 MuzzleWorldVelocity { get; private set; }

        public void UpdateCharge()
        {
            
        }

        public void ShowWeapon()
        {
            
        }

        public void TryBeginCharge()
        {
            
        }

        public void TryShoot()
        {
            
        }

        public void TryReleaseCharge()
        {
            
        }
        
        
        
    }
}

