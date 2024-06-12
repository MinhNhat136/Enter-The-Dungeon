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

            protected override void CheckHits()
            {
                var point1 = sourcePoint.transform.position + sourcePoint.transform.forward * CapsuleCastHeight / 2;
                var point2 = sourcePoint.transform.position - sourcePoint.transform.forward * CapsuleCastHeight / 2;

                numCollide = Physics.OverlapCapsuleNonAlloc(point1, point2, CapsuleCastRadius, Colliders, targetLayerMask);
            }
        }
    }
}
