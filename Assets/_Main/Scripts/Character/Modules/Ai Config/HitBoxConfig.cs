using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character.Module
{
    [CreateAssetMenu(menuName = "Ai Module Config /Hitbox")]
    public class HitBoxConfig : SerializedScriptableObject
    {
        [Tooltip("Time that this gameObject is invulnerable for, after receiving damage.")]
        public float InvulnerabiltyTime;

        [Tooltip("The angle from the which that damageable is hitable. Always in the world XZ plane, with the forward being rotate by hitForwardRoation")]
        [Range(0.0f, 360.0f)]
        public float HitAngle = 360.0f;

        [Tooltip("Allow to rotate the world forward vector of the damageable used to define the hitAngle zone")]
        [Range(0.0f, 360.0f)]
        [FormerlySerializedAs("hitForwardRoation")]
        public float HitForwardRotation = 360.0f;


        public Dictionary<PassiveEffect, IPassiveEffect> Effects = new();

       
        public void Assign(AiHitBoxController hitBoxController)
        {
            hitBoxController.InvulnerableTime = InvulnerabiltyTime;
            hitBoxController.HitAngle = HitAngle;
            hitBoxController.HitForwardRotation = HitForwardRotation;

            foreach (var effect in Effects)
            {
                hitBoxController.PassiveEffects[effect.Key] = effect.Value.Clone();
            }
        }

        [Button]
        public void AssignEffect()
        {
            foreach(var key in Effects.Keys.ToArray())
            {
                Effects[key] = CreatePassiveEffect(key);
            }
        }

        private IPassiveEffect CreatePassiveEffect(PassiveEffect effect)
        {
            switch (effect)
            {
                case PassiveEffect.None:
                    return null;
                case PassiveEffect.Freeze:
                    return new Freezeable(); 
                case PassiveEffect.Burn:
                    return new Burnable();
                default:
                    return null; 
            }
        }

    }

}
