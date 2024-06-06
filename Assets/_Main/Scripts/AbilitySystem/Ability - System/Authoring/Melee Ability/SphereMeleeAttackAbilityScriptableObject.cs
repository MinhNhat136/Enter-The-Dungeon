using Atomic.Core;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public abstract class SphereMeleeAttackAbilityScriptableObject : MeleeAttackAbilityScriptableObject
    {
        [field: SerializeField] public float SphereCastRadius { get; private set; }
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new SphereMeleeAttackSpec(this, owner)
            {
                Level = owner.Level,
                SphereCastRadius = SphereCastRadius,
            };
            return spec;
        }

        private class SphereMeleeAttackSpec : MeleeAttackSpec
        {
            public float SphereCastRadius;
            
            public SphereMeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(
                ability, owner)
            {
            }
            
            protected override void CastHits()
            {
                Collider[] hitColliders = Physics.OverlapSphere(sourcePoint.transform.position, SphereCastRadius, MeleeAttackAbility.TargetLayerMask);
                foreach (var coll in hitColliders)
                {
                    var factor = coll.GetComponentInParent<AbilitySystemController>();
                    if (factor == Owner) continue;
                    if (factor && !AffectedControllers.Contains(factor))
                    {
                        AffectedControllers.Add(factor);
                        var effectSpec = hitAbilitySo.CreateHitSpecValue(
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
}