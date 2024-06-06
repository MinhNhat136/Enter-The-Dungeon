using Atomic.Character;
using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Melee/Capsule")]
    public class CapsuleMeleeAttackAbilityScriptableObject : AbstractMeleeAttackAbilityScriptableObject
    {
        [field: SerializeField]
        public float CapsuleCastRadius { get; private set; }

        [field: SerializeField]
        public float CapsuleCastHeight { get; private set; }

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new CapsuleMeleeAttackSpec(this, owner)
            {
                CapsuleCastRadius = CapsuleCastRadius,
                CapsuleCastHeight = CapsuleCastHeight
            };
            return spec;
        }

        private class CapsuleMeleeAttackSpec : AbstractMeleeAttackSpec
        {
            public float CapsuleCastRadius;
            public float CapsuleCastHeight;

            public CapsuleMeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) 
                : base(ability, owner)
            {
            }

            protected override bool CheckGameplayTags()
            {
                return true;
            }

            protected override void CastHits()
            {
                var point1 = sourcePoint.transform.position + sourcePoint.transform.forward * CapsuleCastHeight / 2;
                var point2 = sourcePoint.transform.position - sourcePoint.transform.forward * CapsuleCastHeight / 2;

                var hitColliders = Physics.OverlapCapsule(point1, point2, CapsuleCastRadius, targetLayerMask);
                foreach (var coll in hitColliders)
                {
                    var factor = coll.GetComponentInParent<AbilitySystemController>();
                    if (!factor || AffectedControllers.Contains(factor)) continue;
                    if (factor == Owner) continue;
                    AffectedControllers.Add(factor);
                    var effectSpec = hitAbilitySo.CreateSpec(
                        Owner,
                        factor,
                        coll.gameObject.transform.position,
                        sourcePoint.forward);
                    Coroutines.StartCoroutine(effectSpec.TryActivateAbility());
                }
            }
        }
    }
}
