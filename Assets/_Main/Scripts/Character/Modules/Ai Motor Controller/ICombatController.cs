using Atomic.Equipment;

namespace Atomic.Character.Module
{
    public enum CombatMode
    {
        Ranged, 
        Melee,
    }
    
    public interface ICombatController
    {
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
