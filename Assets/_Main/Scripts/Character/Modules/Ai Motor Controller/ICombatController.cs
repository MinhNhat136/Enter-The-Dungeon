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
        public void BeginAttack();

        public virtual void CustomAttack()
        {
            
        }
        public void EndAttack();
    }
}
