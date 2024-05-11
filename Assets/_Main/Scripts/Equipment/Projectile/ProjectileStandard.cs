using Atomic.Character;
using UnityEngine;

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
        public override ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            base.Spawn(owner, hitMask);
            return this;
        }
        
        //  Unity Methods   -------------------------------
        public void Update()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        //  Other Methods ---------------------------------
        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            _distance = DistanceWeight.GetValueFromRatio(EnergyValue);
            
            Invoke(nameof(TriggerOnCollisionAfterDelay), _distance / _speed);
        }

        public override void OnHit(Vector3 point, Vector3 normal, Collider collide)
        {
            
        }

        protected override void TriggerOnCollisionAfterDelay()
        {
            OnTriggerEnter(null);
        }

        public void OnTriggerEnter(Collider other)
        {
            OnProjectileTrigger?.Invoke(this, other);
        }
        
        //  Event Handlers --------------------------------
    }
    
}
