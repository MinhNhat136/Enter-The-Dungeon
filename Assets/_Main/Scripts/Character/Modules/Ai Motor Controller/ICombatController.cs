using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character
{
    public enum CombatMode
    {
        Ranged, 
        Melee,
    }
    
    public interface ICombatController : IInitializableWithBaseModel<BaseAgent>
    {
        public WeaponBuilder CurrentWeapon { get; set; }

        public void AimTarget();
        
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
        
        public  void EndAttack()
        {
        }

        public void CustomAction()
        {
            
        }
    }
}
