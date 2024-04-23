using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controller for melee combat actions.
    /// </summary>
    public class MeleeCombatController : MonoBehaviour, ICombatController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Weapon CurrentWeapon
        {
            get => _meleeWeapon;
            set
            {
                if (value is MeleeWeapon weapon)
                {
                    _meleeWeapon = weapon;
                }
                else
                {
                    throw new System.Exception("weapon for combatMode invalid");
                }
            }
        }


        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }
        
        //  Fields ----------------------------------------
        private MeleeWeapon _meleeWeapon;
        
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
            if (!IsInitialized)
            {
                throw new System.Exception("MeleeCombatController not initialized");
            }
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void RegisterWeapon()
        {
            throw new System.NotImplementedException();
        }

        public void AimTarget()
        {
            
        }

        public void BeginPrepareAttack()
        {
            Debug.Log("BeginPrepareAttack");
        }

        public void PreparingAttack()
        {
            Debug.Log("PreparingAttack");

        }

        public void EndPrepareAttack()
        {
            Debug.Log("EndPrepareAttack");

        }

        public void BeginAttackMove()
        {
            Debug.Log("BeginAttackMove");

        }

        public void AttackMoving()
        {
            Debug.Log("AttackMoving");

        }

        public void EndAttackMove()
        {
            Debug.Log("EndAttackMove");

        }

        public void BeginAttack()
        {
            Debug.Log("BeginAttack");

        }

        public void Attacking()
        {
            Debug.Log("Attacking");

        }

        public void EndAttack()
        {
            Debug.Log("EndAttack");

        }

        public void CustomAction()
        {
            Debug.Log("CustomAction");

        }

        //  Event Handlers --------------------------------
        
    }
}