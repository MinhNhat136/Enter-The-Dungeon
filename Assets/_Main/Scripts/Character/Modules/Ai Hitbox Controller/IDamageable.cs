using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------
    public struct DamageMessage
    {
        public BaseAgent Damager { get; set; }
        public int Amount { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 DamageSource { get; set; }
        public PassiveEffect PassiveEffect { get; set; }
    }
    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>

    public interface IDamageable
    {
        public Dictionary<PassiveEffect, IPassiveEffect> PassiveEffects { get; }
        public event Action OnReceiveDamage { add { } remove { } }
        public event Action OnHitWhileInvulnerable { add { } remove { } }
        public event Action OnBecomeVulnerable { add { } remove { } }
        public event Action OnResetDamage { add { } remove { } }
        public void ApplyDamage(DamageMessage data);
        public void ResetDamage();
    }

}

