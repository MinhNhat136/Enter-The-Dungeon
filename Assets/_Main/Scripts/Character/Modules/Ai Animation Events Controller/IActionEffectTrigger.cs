namespace Atomic.Character.Module
{
    public enum ActionEffectType
    {
        ChargeFull,
        BeginAttackTrail,
        StopAttackTrail,
        BeginHitDamage,
        CustomEffect,
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
