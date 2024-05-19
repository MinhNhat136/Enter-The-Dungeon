using Atomic.Core.Interface;
using Atomic.Equipment;

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
        
        public void BeginPrepareAttack()
        {
            
        }

        public void PreparingAttack()
        {
            
        }

        public void EndPrepareAttack()
        {
            
        }

        public void InterruptPrepareAttack()
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

        public void InterruptAttackMove()
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

        public void InterruptAttack()
        {
            
        }

        public void CustomAction()
        {
            
        }

        public void InterruptAction()
        {
            
        }
    }
}
