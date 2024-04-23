using UnityEngine;

namespace Atomic.Equipment
{
    public class ShotGunProjectile : ProjectileBase
    {
        public void OnShoot()
        {
            transform.Translate(transform.forward);
        }
    }
}
