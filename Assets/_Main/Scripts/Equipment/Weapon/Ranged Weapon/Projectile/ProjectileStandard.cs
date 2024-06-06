using Atomic.AbilitySystem;
using Atomic.Character;
using Atomic.Core;
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
        public override ProjectileBase Spawn(BaseAgent owner)
        {
            base.Spawn(owner);
            return this;
        }

        //  Unity Methods   -------------------------------
        public void Update()
        {
            myTransform.position += _speed * Time.deltaTime * myTransform.forward;
        }

        //  Other Methods ---------------------------------
        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            _distance = DistanceWeight.GetValueFromRatio(EnergyValue);

            Invoke(nameof(ReleaseAfterDelay), _distance / _speed);
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

        //  Event Handlers --------------------------------
    }
}