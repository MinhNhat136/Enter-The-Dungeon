using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character
{
    public class Burnable : IPassiveEffect
    {
        public PassiveEffect PassiveEffect => throw new NotImplementedException();

        public Action OnApplyPassiveEffect => throw new NotImplementedException();

        public void ApplyEffect()
        {
            throw new NotImplementedException();
        }

        public IPassiveEffect Clone()
        {
            throw new NotImplementedException();
        }
    }

}
