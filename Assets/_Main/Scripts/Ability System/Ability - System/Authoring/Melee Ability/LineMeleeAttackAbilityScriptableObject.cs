using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Melee/Line")]
    public class LineMeleeAttackAbilityScriptableObject : AbstractMeleeAttackAbilityScriptableObject
    {
        [field: SerializeField]
        public float LineCastDistance { get; private set; }

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new LineMeleeAttackSpec(this, owner)
            {
                LineCastDistance = LineCastDistance
            };
            return spec;
        }

        private class LineMeleeAttackSpec : AbstractMeleeAttackSpec
        {
            public float LineCastDistance;

            public LineMeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) 
                : base(ability, owner)
            {
            }

            protected override bool CheckGameplayTags()
            {
                return true;
            }

            protected override void CastHits()
            {
                var origin = sourcePoint.transform.position;
                var direction = sourcePoint.transform.forward;

                var hits = Physics.RaycastAll(origin, direction, LineCastDistance, targetLayerMask);
                foreach (var hit in hits)
                {
                    var factor = hit.collider.GetComponentInParent<AbilitySystemController>();
                    if (!factor || AffectedControllers.Contains(factor)) continue;
                    if (factor == Owner) continue;
                    AffectedControllers.Add(factor);
                    var effectSpec = hitAbilitySo.CreateSpec(
                        Owner,
                        factor,
                        hit.point,
                        direction);
                    Coroutines.StartCoroutine(effectSpec.TryActivateAbility());
                }
            }
        }
    }
}
