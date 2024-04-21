using System;
using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character
{
    public class RangedCombatController : MonoBehaviour, ICombatController, IInitializable
    {
        public RangedWeapon RangedWeapon;

        public Weapon CurrentWeapon
        {
            get => RangedWeapon;
            set
            {
                if (value is RangedWeapon weapon)
                {
                    RangedWeapon = weapon;
                }
                else
                {
                    throw new Exception("Ranged weapon invalid");
                }
            }
        }

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