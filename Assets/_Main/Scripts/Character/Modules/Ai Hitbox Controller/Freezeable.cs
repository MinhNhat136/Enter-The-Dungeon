using System;

namespace Atomic.Character.Module
{
    public class Freezeable : IPassiveEffect
    {
        public const PassiveEffect Effect = PassiveEffect.Freeze;
        private Action _onFreeze;

        public PassiveEffect PassiveEffect
        {
            get { return Effect; }
        }

        public Action OnApplyPassiveEffect 
        { 
            get { return _onFreeze; }  
        }

        public void ApplyEffect()
        {

        }

        public IPassiveEffect Clone()
        {
            return new Freezeable();
        }
    }

}
