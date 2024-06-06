using System.Collections;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Sphere")]
    public class SphereCastAbilityScriptableObject : AbstractRangeCastAbilityScriptableObject
    {
        public float explosionRadius;
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            return new ExplodeAbilitySpec(this, owner)
            {
                ExplosionRadius = explosionRadius,
            };
        }

        private class ExplodeAbilitySpec : AbstractRangeCastAbilityScriptableObject.AbstractRangeCastAbilitySpec
        {
            public float ExplosionRadius;

            
            public ExplodeAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
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
                
                Collider[] hitColliders = Physics.OverlapSphere(hitPoint, ExplosionRadius, targetLayerMask);
                foreach (var coll in hitColliders)
                {
                    var factor = coll.GetComponentInParent<AbilitySystemController>();
                    if (factor == Owner) continue;
                    if (factor && !affectedControllers.Contains(factor))
                    {
                        affectedControllers.Add(factor);
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
                
                SphereCastAbilityScriptableObject castAbilitySo = Ability as SphereCastAbilityScriptableObject;
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
