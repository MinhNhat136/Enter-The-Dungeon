using System;
using Atomic.Character;
using UnityEngine.Pool;

namespace  Atomic.Damage
{
    [Serializable]
    public abstract class PassiveEffect
    {
        public StatusEffectType EffectType { get; set; }
        public float Interval { get; set; }
        public float Tick { get; set; }
        public float Damage { get; set; }
        public BaseAgent Target { get; set; }
        public BaseAgent Source { get; set; }
        public ObjectPool<EffectPopupAnimation> effectPool; 
        
        public abstract void Apply();
        public abstract void Handle();
        public abstract void Remove();
        public abstract PassiveEffect Clone();
    }
}

