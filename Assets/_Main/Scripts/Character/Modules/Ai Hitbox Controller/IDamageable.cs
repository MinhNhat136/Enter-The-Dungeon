using Atomic.Character.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------
    public struct DamageMessage
    {
        public BaseAgent damager;
        public int amount;
        public Vector3 direction;
        public Vector3 damageSource;
        public PassiveEffect passiveEffect;
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

