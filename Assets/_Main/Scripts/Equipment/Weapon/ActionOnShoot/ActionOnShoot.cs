using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Equipment
{
    public abstract class ActionOnShoot : MonoBehaviour
    {
        [HideInInspector]
        public ProjectileBase projectile;

        public abstract void Initialize();
        public abstract void OnShoot(Vector3 point, Vector3 normal);
    }
    
}
