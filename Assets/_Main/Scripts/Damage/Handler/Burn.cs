using UnityEngine;

namespace Atomic.Damage
{
    public class Burn : PassiveEffect
    {
        private float _currentTick;

        public override void Apply()
        {
        }

        public override void Handle()
        {
            if (_currentTick < Tick)
            {
                _currentTick += Time.deltaTime;
                return;
            }
        }

        public override void Remove()
        {
        }

        public override PassiveEffect Clone()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
