using Atomic.Character;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class ProjectileStandard : ProjectileBase
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private float _speed;
        private float _distance;
        
        //  Initialization  -------------------------------
        public override ProjectileBase Spawn(BaseAgent owner)
        {
            base.Spawn(owner);
            HitVfx = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 7);
            return this;
        }

        //  Unity Methods   -------------------------------
        public void Update()
        {
            MyTransform.position += _speed * Time.deltaTime * MyTransform.forward;
        }

        //  Other Methods ---------------------------------
        
        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            _distance = DistanceWeight.GetValueFromRatio(EnergyValue);
            actionOnHit?.Initialize();

            Invoke(nameof(ReleaseAfterDelay), _distance / _speed);
        }
        
        protected override void ReleaseAfterDelay() => Release?.Invoke(this);

        public void OnTriggerEnter(Collider other)
        {
            if (!actionOnHit)
            {
                Release?.Invoke(this);
            }
            actionOnHit.OnHit(MyTransform.position, MyTransform.forward, other);
        }

        //  Event Handlers --------------------------------
    }
}