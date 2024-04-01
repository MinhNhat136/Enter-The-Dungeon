
using System;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------
    public enum PassiveEffect
    {
        None,
        Freeze,
        Burn,
        Toxic,
        Stagger,
        Fear,
        Shock,
        Slow,
        ArmorBreak,
        KnockBack,
        KnockDown,
    }
    //  Class Attributes ----------------------------------

    /// <summary>
    /// Interface defining methods and properties related to passive effects.
    /// </summary>
    public interface IPassiveEffect
    {
        public IPassiveEffect Clone();
        public PassiveEffect PassiveEffect { get; }
        public Action OnApplyPassiveEffect { get; }
        public void ApplyEffect();
    }

}
