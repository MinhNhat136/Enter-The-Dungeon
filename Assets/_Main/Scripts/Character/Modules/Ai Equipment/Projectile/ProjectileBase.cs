using System;
using Atomic.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Equipment
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        public BaseAgent Owner { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialDirection { get; private set; }
        public float InitialCharge { get; private set; }

        public UnityAction OnShoot ;

        public void Shoot(RangedWeapon controller)
        {
            Owner = controller.Owner;
            InitialPosition = transform.position;
            InitialDirection = transform.forward;
            InitialCharge = controller.CurrentCharge;

            OnShoot?.Invoke();
        }
    }
}

