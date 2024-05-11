using UnityEngine;

namespace Atomic.Equipment
{
    public abstract class BarrelController : MonoBehaviour
    {
        public ParticleSystem shootSystem;
        public ParticleSystem onChargeFullFXPrefab;
        public GameObject chargeObject;
        
        public abstract void Charge();
        public abstract void OnChargeFull();
        public abstract void Shoot(ProjectileBase projectile);
    }
}

