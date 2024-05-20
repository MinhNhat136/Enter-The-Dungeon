using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    public abstract class ActionOnHit : MonoBehaviour
    {
        [HideInInspector]
        public ProjectileBase projectile;

        public abstract void Initialize();
        public abstract void OnHit(Vector3 point, Vector3 normal, Collider collide);
    }

}
