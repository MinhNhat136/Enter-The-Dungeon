using Atomic.Character;
using Atomic.Core;
using UnityEngine;

namespace Atomic.Equipment
{
    public class ProjectileCannon : ProjectileBase
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private float _speed;
        private float _distance;
        private float _time;
        private float _currentFlyTime;
        private Vector3 _initializeVelocity;

        //  Initialization  -------------------------------
        public override ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            base.Spawn(owner, hitMask);
            return this;
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void Load(Vector3 shootPosition, Vector3 shootDirection, Vector3 shootTarget, float energyValue)
        {
            base.Load(shootPosition, shootDirection, shootTarget, energyValue);
            _currentFlyTime = 0;
        }

        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            _distance = Vector3.Distance(ShootPosition, ShootTarget);
            _initializeVelocity =
                ProjectileMotionExtension.CalculateInitialVelocity(transform, ShootTarget,
                    Vector3.Angle(transform.forward, Owner.transform.forward));
            _time = ProjectileMotionExtension.TimeToReachTarget(_distance, _initializeVelocity) + 0.2f;
        }

        public void Update()
        {
            if (_currentFlyTime < _time)
            {
                _currentFlyTime += Time.deltaTime;
                transform.position =
                    ProjectileMotionExtension.CalculatePosition(ShootPosition, ShootTarget, _initializeVelocity,
                        _currentFlyTime);
            }
            else
            {
                Invoke(nameof(TriggerOnCollisionAfterDelay), 0);
            }
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
            if (other != null && !HitMask.ContainsLayer(other.gameObject.layer))
            {
                return;
            }
            OnProjectileTrigger?.Invoke(this, other);
        }
    }
}