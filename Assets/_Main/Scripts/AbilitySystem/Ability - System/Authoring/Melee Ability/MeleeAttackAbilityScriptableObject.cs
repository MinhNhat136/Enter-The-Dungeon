using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Melee Attack Ability")]
    public class MeleeAttackAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        [field: SerializeField] 
        public string AnimationName { get; private set; }
        
        [field: SerializeField]
        public float AttackMoveSpeed { get; private set; }
        
        [field: SerializeField]
        public float DelayResetCombo { get; private set; }
        
        [field: SerializeField]
        public HitAbilityScriptableObject HitAbility { get; private set; }

        [field: SerializeField]
        public LayerMask TargetLayerMask { get; private set; }
        
        public MeleeAttackSpec CreateMeleeAttackSpecValue(AbilitySystemController source, Transform sourcePoint)
        {
            MeleeAttackSpec meleeAttackSpec = (MeleeAttackSpec)CreateSpec(source);
            meleeAttackSpec.hitAbilitySo = HitAbility;
            meleeAttackSpec.sourcePoint = sourcePoint;
            return meleeAttackSpec;
        }
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            return new MeleeAttackSpec(this, owner);
        }

        public class MeleeAttackSpec : AbstractAbilitySpec
        {
            public HitAbilityScriptableObject hitAbilitySo;
            public Transform sourcePoint;
            public bool isAttack;

            protected readonly MeleeAttackAbilityScriptableObject MeleeAttackAbility;
            protected readonly List<AbilitySystemController> AffectedControllers = new(16);

            public MeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
                MeleeAttackAbility = ability as MeleeAttackAbilityScriptableObject;
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
                yield return null; 
            }

            protected override IEnumerator ActivateAbility()
            {
                Debug.Log(isAttack);
                while (isAttack)
                {
                    CastHits();
                    yield return null;
                }
            }

            protected virtual void CastHits()
            {
            }
        }
    }
}
