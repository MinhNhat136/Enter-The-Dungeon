using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class RangedCombatController : MonoBehaviour, ICombatController, IInitializable
    {
        public RangedWeapon RangedWeapon;

        public void BeginPrepareAttack()
        {
        }

        public void PreparingAttack()
        {
        }

        public void EndPrepareAttack()
        {
        }

        public void BeginAttackMove()
        {
        }

        public void AttackMoving()
        {
        }

        public void EndAttackMove()
        {
        }

        public void BeginAttack()
        {
        }

        public void Attacking()
        {
        }

        public void EndAttack()
        {
        }

        public void CustomAction()
        {
        }

        public bool IsInitialized { get; }
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
    }


}