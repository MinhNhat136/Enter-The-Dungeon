using System;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    
    /// <summary>
    /// Controller for ranged combat actions.
    /// </summary>
    public class RangedCombatController : MonoBehaviour, ICombatController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public WeaponBuilder CurrentWeapon
        {
            get => _rangedWeapon;
            set
            {
                if (value is RangedWeaponBuilder rangedWeapon)
                    _rangedWeapon = rangedWeapon;
                else
                {
                    Debug.Log(_rangedWeapon);
                    throw new Exception("Ranged weapon invalid");
                }
            }
        }



        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }
        

        //  Fields ----------------------------------------
        private RangedWeaponBuilder _rangedWeapon;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void AimTarget()
        {
            if (!Model.TargetAgent)
                return;
            Vector3 targetDirection = Model.TargetAgent.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion dir = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, 10f * Time.deltaTime);
        }
        
        public void BeginPrepareAttack()
        {
            _rangedWeapon.BeginCharge();
        }

        public void PreparingAttack()
        {
            _rangedWeapon.UpdateCharge();
        }

        public void EndPrepareAttack()
        {
            
        }
        
        public void BeginAttack()
        {
            _rangedWeapon.Shoot();
        }

        public void Attacking()
        {
            
        }
        
        public void EndAttack()
        {
            _rangedWeapon.EndShoot();
            Model.LastAttackTime = Time.time;
        }

        public void CustomAction()
        {
            
        }

        public void InterruptPrepareAttack()
        {
            _rangedWeapon.CancelCharge();
        }

        //  Event Handlers --------------------------------
        private void OnChargeFull()
        {
            Debug.Log("charge full");
        }
    }
}

