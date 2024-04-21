using Atomic.Equipment;

namespace Atomic.Character
{
    public enum CombatMode
    {
        Ranged, 
        Melee,
    }
    
    public interface ICombatController
    {
        public Weapon CurrentWeapon { get; set; }
        
        public void BeginPrepareAttack();
        public void PreparingAttack(); 
        public void EndPrepareAttack();

        public virtual void BeginAttackMove()
        {
            
        }

        public virtual void AttackMoving()
        {
            
        }

        public virtual void EndAttackMove()
        {
            
        } 
        
        public void BeginAttack();
        public void Attacking(); 
        public void EndAttack();

        public virtual void CustomAction()
        {
            
        }
    }
}
