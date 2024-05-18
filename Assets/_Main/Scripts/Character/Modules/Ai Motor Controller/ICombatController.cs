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
        public WeaponScriptableObject CurrentWeapon { get; set; }

        public void AimTarget();
        
        public virtual void BeginPrepareAttack()
        {
            
        }

        public virtual void PreparingAttack()
        {
            
        }

        public virtual void EndPrepareAttack()
        {
            
        }

        public virtual void InterruptPrepareAttack()
        {
            
        }
        
        public virtual void BeginAttackMove()
        {
            
        }

        public virtual void AttackMoving()
        {
            
        }

        public virtual void EndAttackMove()
        {
            
        }

        public virtual void InterruptAttackMove()
        {
            
        }

        public virtual void BeginAttack()
        {
            
        }

        public virtual void Attacking()
        {
            
        }
        
        public virtual void EndAttack()
        {
            
        }

        public virtual void InterruptAttack()
        {
            
        }

        public virtual void CustomAction()
        {
            
        }

        public virtual void InterruptAction()
        {
            
        }
    }
}
