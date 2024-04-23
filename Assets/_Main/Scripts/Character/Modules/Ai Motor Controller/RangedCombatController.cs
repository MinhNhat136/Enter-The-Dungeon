using System;
using Atomic.Equipment;
using NodeCanvas.Tasks.Actions;
using Unity.Mathematics;
using UnityEngine;
using Object = System.Object;

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
        public Weapon CurrentWeapon
        {
            get => _rangedWeapon;
            set
            {
                if (value is RangedWeapon weapon)
                {
                    _rangedWeapon = weapon;
                    RegisterWeapon();
                }
                else
                {
                    throw new Exception("weapon for combatMode invalid");
                }
            }
        }


        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }
        

        //  Fields ----------------------------------------
        private RangedWeapon _rangedWeapon;

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
        public void RegisterWeapon()
        {
            _rangedWeapon.RegisterOwner(Model);
            _rangedWeapon.OnChargeFull += OnChargeFull;
        }

        public void AimTarget()
        {
            if (!Model.TargetAgent)
                return;
            Vector3 targetDirection = Model.TargetAgent.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion dir = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, 1f);
        }

        public void BeginPrepareAttack()
        {
            _rangedWeapon.TryBeginCharge();
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
            _rangedWeapon.ReleaseCharge();
            _rangedWeapon.HandleShoot();
        }

        public void EndAttack()
        {
            
        }

        public void CustomAction()
        {
        }

        //  Event Handlers --------------------------------
        private void OnChargeFull()
        {
            Debug.Log("charge full");
        }
    }
}

