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
            
            protected override void CheckHits() => numCollide = Physics.OverlapSphereNonAlloc(sourcePoint.transform.position, SphereCastRadius, Colliders, targetLayerMask);
        }
    }
}