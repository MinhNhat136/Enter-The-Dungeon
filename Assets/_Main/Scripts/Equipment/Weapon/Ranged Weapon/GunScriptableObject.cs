using UnityEngine;

namespace Atomic.Equipment
{
    public class GunScriptableObject : RangedWeaponScriptableObject
    {
        [Header("BARREL", order = 2)] 
        public GameObject barrelPrefab;

        public override void BeginCharge()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateCharge()
        {
            throw new System.NotImplementedException();
        }

        public override void CancelCharge()
        {
            throw new System.NotImplementedException();
        }

        public override void Shoot()
        {
            throw new System.NotImplementedException();
        }

        public override void EndShoot()
        {
            throw new System.NotImplementedException();
        }


        protected override void OnProjectileCollide(ProjectileBase projectile, Collider other)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnGetProjectile(ProjectileBase projectile)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnReclaimProjectile(ProjectileBase projectile)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDestroyProjectile(ProjectileBase projectile)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
