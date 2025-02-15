using System;
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
        private float _distance;
        private float _time;
        private float _currentFlyTime;
        private Vector3 _initializeVelocity;

        //  Initialization  -------------------------------
        

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public override void Load(BaseAgent owner, Vector3 shootPosition, Vector3 shootDirection, Vector3 shootTarget, float energyValue)
        {
            base.Load(owner, shootPosition, shootDirection, shootTarget, energyValue);
            _currentFlyTime = 0;
        }

        public override void Shoot()
        {
            _distance = Vector3.Distance(ShootPosition, ShootTarget);
            _initializeVelocity =
                ProjectileMotionExtension.CalculateInitialVelocity(transform, ShootTarget,
                    Vector3.Angle(transform.forward, Owner.transform.forward));
            _time = ProjectileMotionExtension.TimeToReachTarget(_distance, _initializeVelocity) + 0.01f;
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
            gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            var effectSpec = abilityOnHit.CreateSpec(
                Owner.AiAbilityController.abilitySystemController,
                myTransform.position,
                myTransform.forward);
            Coroutines.StartCoroutine(effectSpec.TryActivateAbility());
            gameObject.SetActive(false);
        }
    }
}