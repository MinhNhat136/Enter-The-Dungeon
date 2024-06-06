using System.Collections;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Capsule")]
    public class CapsuleCastAbilityScriptableObject : AbstractRangeCastAbilityScriptableObject
    {
        public float capsuleRadius;
        public float capsuleHeight;

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new CapsuleDamageAbilitySpec(this, owner)
            {
                CapsuleRadius = capsuleRadius,
                CapsuleHeight = capsuleHeight,
            };
            return spec;
        }

        private class CapsuleDamageAbilitySpec : AbstractRangeCastAbilityScriptableObject.AbstractRangeCastAbilitySpec
        {
            public float CapsuleRadius;
            public float CapsuleHeight;
            
            public CapsuleDamageAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
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
                Vector3 capsuleStart = hitPoint;
                Vector3 capsuleEnd = hitPoint + hitDirection * CapsuleHeight;

                Collider[] hitColliders = Physics.OverlapCapsule(capsuleStart, capsuleEnd, CapsuleRadius, targetLayerMask);
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

                CapsuleCastAbilityScriptableObject castAbilitySo = Ability as CapsuleCastAbilityScriptableObject;
                foreach (var affectedFactor in affectedControllers)
                {
                    var vfx = hitVFXPool.Get();
                    var vfxObject = vfx.gameObject;
                    vfxObject.transform.position = affectedFactor.transform.position;
                    vfxObject.transform.forward = -hitDirection;
                    
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
