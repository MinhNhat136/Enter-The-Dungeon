namespace Atomic.Character
{
    public enum ActionEffectType
    {
        BeginHit = 1 << 3,
        EndHit = 1 << 4,
        
        BeginKnockdown = 1 << 1,
        EndKnockdown = 1 << 2,
        
        BeginStun = 1 << 5, 
        EndStun = 1 << 6,
        
        BeginBreak = 1 << 7, 
        EndBreak = 1 << 8,
    }
    
    public interface IActionEffectTrigger
    {
        /// <summary>
        /// Check if Component is Enabled
        /// </summary>
        bool enabled { get; set; }
        /// <summary>
        /// Method Called from <seealso cref="AnimatorMoveSender"/>
        /// </summary>
        void OnActionEffectTrigger(ActionEffectType effectType);
    }
    
}
