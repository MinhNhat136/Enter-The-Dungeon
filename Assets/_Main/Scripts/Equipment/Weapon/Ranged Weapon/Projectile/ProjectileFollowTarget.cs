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
            return this;
        }

        //  Unity Methods   -------------------------------
        public void Update()
        {
            FollowTarget();
            Vector3 newPosition = myTransform.position;
            newPosition.y += 0.5f * _gravity * Time.deltaTime * Time.deltaTime;
            myTransform.position = newPosition;
        }

        //  Other Methods ---------------------------------
        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            
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
                Vector3 directionToTarget = (_targetTransform.position - myTransform.position).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                myTransform.rotation = Quaternion.RotateTowards(
                    myTransform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }

            myTransform.position += _speed * Time.deltaTime * myTransform.forward;
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

        }
    }
}