using System;
using Atomic.AbilitySystem;
using Atomic.Character;
using Atomic.Core;
using UnityEngine;
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
                transform.LookAt(_targetTransform);
            }
            else
            {
                transform.LookAt(Owner.modelTransform.forward);
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
            gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            var otherAbilityComponent = other.GetComponentInParent<AbilitySystemController>();
            if (otherAbilityComponent)
            {
                var effectSpec = abilityOnHit.CreateSpec(
                    Owner.AiAbilityController.abilitySystemController, 
                    other.gameObject.transform.position, 
                    myTransform.forward);
                Coroutines.StartCoroutine(effectSpec.TryActivateAbility());     
            }
            gameObject.SetActive(false);
        }
    }
}