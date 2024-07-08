using System;

namespace Atomic.AbilitySystem
{
    /// <summary>
    /// Period configuration for duration/infinite GE
    /// </summary>
    [Serializable]
    public struct GameplayEffectPeriod
    {
        public float period;
        
        public bool executeOnApplication;
    }

}
