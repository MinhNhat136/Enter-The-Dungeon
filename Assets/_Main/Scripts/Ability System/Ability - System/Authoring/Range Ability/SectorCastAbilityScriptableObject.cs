using System.Collections;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Range/Sector")]
    public class SectorCastAbilityScriptableObject : AbstractRangeCastAbilityScriptableObject
    {
        public float sectorAngle;
        public float sectorRadius;

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new SectorDamageAbilitySpec(this, owner)
            {
                SectorAngle = sectorAngle,
                SectorRadius = sectorRadius,
            };
            return spec;
        }

        private class SectorDamageAbilitySpec : AbstractRangeCastAbilityScriptableObject.AbstractRangeCastAbilitySpec
        {
            public float SectorAngle;
            public float SectorRadius;
            
            public SectorDamageAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
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
                hitPoint.y = 0; 
                vfxObject.transform.position = hitPoint;
                vfxObject.transform.forward = -hitDirection;

                Collider[] hitColliders = Physics.OverlapSphere(hitPoint, SectorRadius, targetLayerMask);
                Vector3 direction = -hitDirection;
                foreach (var coll in hitColliders)
                {
                    if (coll.gameObject == Owner.gameObject) continue;

                    Vector3 toCollider = (coll.transform.position - hitPoint).normalized;
                    float angle = Vector3.Angle(direction, toCollider);
                    if (angle <= SectorAngle / 2)
                    {
                        var factor = coll.GetComponentInParent<AbilitySystemController>();
                        if (factor && !affectedControllers.Contains(factor))
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

                SectorCastAbilityScriptableObject castAbilitySo = Ability as SectorCastAbilityScriptableObject;
                foreach (var affectedFactor in affectedControllers)
                {
                    foreach (var hitGameplayEffect in castAbilitySo!.gameplayEffects)
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
