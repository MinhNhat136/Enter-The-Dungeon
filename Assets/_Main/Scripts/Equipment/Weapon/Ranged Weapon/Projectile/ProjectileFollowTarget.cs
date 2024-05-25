using System.Numerics;
using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Atomic.Equipment
{
    public class ProjectileFollowTarget : ProjectileBase
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private float _speed;
        
        private Transform _targetTransform;

        [SerializeField] private float _acceleration;
        [SerializeField] private float _gravity;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _delayReleaseTime;

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
            FollowTarget();
            Vector3 newPosition = MyTransform.position;
            newPosition.y += 0.5f * _gravity * Time.deltaTime * Time.deltaTime;
            MyTransform.position = newPosition;
        }

        //  Other Methods ---------------------------------

        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            if (actionOnHit)
            {
                actionOnHit.Initialize();
            }

            if (actionOnShoot)
            {
                actionOnShoot.Initialize();
            }

            if (Owner.TargetAgent)
            {
                _targetTransform = Owner.TargetAgent.LockPivot;
            }

            Invoke(nameof(ReleaseAfterDelay), _delayReleaseTime);
        }

        private void FollowTarget()
        {
            if (_targetTransform != null)
            {
                Vector3 directionToTarget = (_targetTransform.position - MyTransform.position).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                MyTransform.rotation = Quaternion.RotateTowards(
                    MyTransform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }

            MyTransform.position += _speed * Time.deltaTime * MyTransform.forward;
        }

        protected override void ReleaseAfterDelay()
        {
            OnTriggerEnter(null);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other)
            {
                CancelInvoke(nameof(ReleaseAfterDelay));
            }

            if (!actionOnHit)
            {
                Release?.Invoke(this);
            }

            actionOnHit.OnHit(MyTransform.position, MyTransform.forward, other);
        }
    }
}