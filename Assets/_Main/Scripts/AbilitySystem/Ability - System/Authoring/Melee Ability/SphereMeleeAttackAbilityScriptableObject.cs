using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Melee/Sphere")]
    public class SphereMeleeAttackAbilityScriptableObject : AbstractMeleeAttackAbilityScriptableObject
    {
        [field: SerializeField] 
        public float SphereCastRadius { get; private set; }
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new SphereMeleeAttackSpec(this, owner)
            {
                SphereCastRadius = SphereCastRadius,
            };
            return spec;
        }

        private class SphereMeleeAttackSpec : AbstractMeleeAttackSpec
        {
            public float SphereCastRadius;
            
            public SphereMeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(
                ability, owner)
            {
            }
            
            protected override bool CheckGameplayTags()
            {
                return true;
            }
            
            protected override void CastHits()
            {
                var hitColliders = Physics.OverlapSphere(sourcePoint.transform.position, SphereCastRadius, targetLayerMask);
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