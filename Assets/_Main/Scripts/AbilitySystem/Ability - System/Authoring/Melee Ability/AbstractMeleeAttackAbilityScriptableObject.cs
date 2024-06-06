using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    public abstract class AbstractMeleeAttackAbilityScriptableObject : AbstractAbilityScriptableObject
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
        
        public AbstractMeleeAttackSpec CreateSpec(AbilitySystemController source, Transform sourcePoint)
        {
            var abstractMeleeAttackSpec = (AbstractMeleeAttackSpec)CreateSpec(source);
            abstractMeleeAttackSpec.sourcePoint = sourcePoint;
            abstractMeleeAttackSpec.Level = source.Level;
            abstractMeleeAttackSpec.hitAbilitySo = HitAbility;
            abstractMeleeAttackSpec.targetLayerMask = TargetLayerMask;
            return abstractMeleeAttackSpec;
        }
        
        public abstract class AbstractMeleeAttackSpec : AbstractAbilitySpec
        {
            public HitAbilityScriptableObject hitAbilitySo;
            public Transform sourcePoint;
            public LayerMask targetLayerMask;

            private bool _canTick;
            protected readonly List<AbilitySystemController> AffectedControllers = new(16);

            protected AbstractMeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
            }
            
            protected override IEnumerator PreActivate()
            {
                _canTick = true;
                yield return null; 
            }

            protected override IEnumerator ActivateAbility()
            {
                while (_canTick)
                {
                    CastHits();
                    yield return null;
                }
            }

            public override void CancelAbility() => _canTick = false;

            protected override void EndAbility() => AffectedControllers.Clear();

            protected abstract void CastHits();
        }
    }
}
