using Atomic.Character;
using UnityEngine;

namespace Atomic.Damage
{
    public abstract class DamageForce : ScriptableObject
    {
        public abstract void ApplyForce(BaseAgent agent);
    }
}
