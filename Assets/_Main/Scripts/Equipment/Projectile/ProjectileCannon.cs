using Atomic.Character;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    public class ProjectileCannon : ProjectileBase
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private float _distance;
        private float _time;
        private float _currentFlyTime;
        private Vector3 _initializeVelocity;

        //  Initialization  -------------------------------
        public override ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            base.Spawn(owner, hitMask);
            HitVfx = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 5);
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
            _distance = Vector3.Distance(ShootPosition, ShootTarget);
            actionOnHit.Initialize();
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
                Invoke(nameof(ReleaseAfterDelay), 0);
            }
        }
        
        protected override void ReleaseAfterDelay()
        {
            actionOnHit.OnHit(MyTransform.position, MyTransform.forward, null);
            Release?.Invoke(this);   
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!HitMask.ContainsLayer(other.gameObject.layer))
            {
                return;
            }
            
            actionOnHit.OnHit(MyTransform.position, MyTransform.forward, other);
            Release?.Invoke(this);
        }
    }
}