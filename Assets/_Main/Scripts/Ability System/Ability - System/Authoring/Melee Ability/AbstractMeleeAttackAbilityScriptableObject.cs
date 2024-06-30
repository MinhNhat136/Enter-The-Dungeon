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
        public AnimationClip AnimationClip { get; private set; }
        
        [field: SerializeField]
        public float AttackMoveSpeed { get; private set; }

        public GameplayEffectScriptableObject[] gameplayEffects;

        [field: SerializeField]
        public LayerMask TargetLayerMask { get; private set; }
        
        public AbstractMeleeAttackSpec CreateSpec(AbilitySystemController source, Transform sourcePoint)
        {
            var abstractMeleeAttackSpec = (AbstractMeleeAttackSpec)CreateSpec(source);
            abstractMeleeAttackSpec.sourcePoint = sourcePoint;
            abstractMeleeAttackSpec.Level = source.Level;
            abstractMeleeAttackSpec.abilitySo = this;
            abstractMeleeAttackSpec.targetLayerMask = TargetLayerMask;
            foreach (var eventOnActivate in eventOnActivates)
            {
                abstractMeleeAttackSpec.OnApplyGameplayEffect += eventOnActivate.PreApplyEffectSpec;
            }
            return abstractMeleeAttackSpec;
        }
        
        public abstract class AbstractMeleeAttackSpec : AbstractAbilitySpec
        {
            public AbstractMeleeAttackAbilityScriptableObject abilitySo;
            public Transform sourcePoint;
            public LayerMask targetLayerMask;
            
            private bool _canTick;
            protected int numCollide;
            private readonly List<AbilitySystemController> _affectedControllers = new(16);
            protected readonly Collider[] Colliders;
            
            protected AbstractMeleeAttackSpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
                Colliders = new Collider[32];
            }
            
            protected override IEnumerator PreActivate()
            {
                _canTick = true;
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
                yield return null; 
            }

            protected override IEnumerator ActivateAbility()
            {
                while (_canTick)
                {
                    CheckHits();
                    ApplyEffects();
                    yield return null;
                }
                EndAbility();
            }

            public override void CancelAbility() => _canTick = false;

            protected override void EndAbility() => _affectedControllers.Clear();

            protected abstract void CheckHits();

            private void ApplyEffects()
            {
                for (int indexCollide = 0; indexCollide < numCollide; indexCollide++)
                {
                    var factor = Colliders[indexCollide].GetComponentInParent<AbilitySystemController>();
                    if (!factor || _affectedControllers.Contains(factor)) continue;
                    if (factor == Owner) continue;
                    _affectedControllers.Add(factor);
                    for (int index = 0; index < abilitySo!.gameplayEffects.Length; index++)
                    {
                        var effectSpec = Owner.MakeOutgoingSpec(abilitySo!.gameplayEffects[index])
                            .SetTarget(factor).SetIndex(index);
                        OnApplyGameplayEffect?.Invoke(effectSpec);
                        factor.ApplyGameplayEffectSpecToSelf(effectSpec);
                    }
                }
                
            }
        }
    }
}
