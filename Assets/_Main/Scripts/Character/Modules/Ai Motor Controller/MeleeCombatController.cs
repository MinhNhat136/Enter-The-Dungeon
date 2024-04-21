using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class MeleeCombatController : MonoBehaviour, ICombatController
    {
        public Weapon CurrentWeapon { get; set; }

        public void BeginPrepareAttack()
        {
            throw new System.NotImplementedException();
        }

        public void PreparingAttack()
        {
            throw new System.NotImplementedException();
        }

        public void EndPrepareAttack()
        {
            throw new System.NotImplementedException();
        }

        public void BeginAttackMove()
        {
            throw new System.NotImplementedException();
        }

        public void AttackMoving()
        {
            throw new System.NotImplementedException();
        }

        public void EndAttackMove()
        {
            throw new System.NotImplementedException();
        }

        public void BeginAttack()
        {
            Debug.Log("begin melee combat");
        }

        public void Attacking()
        {
            throw new System.NotImplementedException();
        }

        public void EndAttack()
        {
            Debug.Log("end melee combat");
        }

        public void CustomAction()
        {
            throw new System.NotImplementedException();
        }
    }
}