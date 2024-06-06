using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Range/Cone")]
    public class ConeCastAbilityScriptableObject : AbstractRangeCastAbilityScriptableObject
    {
        public float coneAngle;
        public float coneDistance;

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new ConeDamageAbilitySpec(this, owner)
            {
                ConeAngle = coneAngle,
                ConeDistance = coneDistance,
            };
            return spec;
        }

        private class ConeDamageAbilitySpec : AbstractRangeCastAbilityScriptableObject.AbstractRangeCastAbilitySpec
        {
            public float ConeAngle;
            public float ConeDistance;
            
            public ConeDamageAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
            }

            public override void CancelAbility()
            {
            }

            protected override bool CheckGameplayTags()
            {
                return true;
            }

            protected override IEnumerator PreActivate()
            {
                var vfx = hitVFXPool.Get();
                var vfxObject = vfx.gameObject;
                vfxObject.transform.position = hitPoint;
                vfxObject.transform.forward = -hitDirection;

                Collider[] hitColliders = Physics.OverlapSphere(hitPoint, ConeDistance, targetLayerMask);
                foreach (var coll in hitColliders)
                {
                    var factor = coll.GetComponentInParent<AbilitySystemController>();
                    if (factor == Owner) continue;
                    if (factor && !affectedControllers.Contains(factor))
                    {
                        Vector3 directionToTarget = (coll.transform.position - hitPoint).normalized;
                        float angleToTarget = Vector3.Angle(hitDirection, directionToTarget);

                        if (angleToTarget <= ConeAngle / 2)
                        {
                            affectedControllers.Add(factor);
                        }
                    }
                }
                yield return null;
            }

            protected override IEnumerator ActivateAbility()
            {
                if (Ability.cooldown)
                {
                    var cdSpec = Owner.MakeOutgoingSpec(Ability.cooldown);
                    Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
                }

                if (Ability.cost)
                {
                    var costSpec = Owner.MakeOutgoingSpec(Ability.cost);
                    Owner.ApplyGameplayEffectSpecToSelf(costSpec);
                }

                ConeCastAbilityScriptableObject abilitySo = Ability as ConeCastAbilityScriptableObject;
                foreach (var affectedFactor in affectedControllers)
                {
                    foreach (var hitGameplayEffect in abilitySo!.gameplayEffects)
                    {
                        var effectSpec = Owner.MakeOutgoingSpec(hitGameplayEffect).SetTarget(affectedFactor);
                        OnApplyGameplayEffect?.Invoke(effectSpec);
                        affectedFactor.ApplyGameplayEffectSpecToSelf(effectSpec);
                    }
                }

                yield return null;
            }
        }
    }
}
