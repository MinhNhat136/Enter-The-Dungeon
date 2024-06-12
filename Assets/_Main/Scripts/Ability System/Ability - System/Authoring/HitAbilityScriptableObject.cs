using System.Collections;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Hit Ability")]
    public class HitAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        public ParticleSystem hitVFX;
        public GameplayEffectScriptableObject[] hitGameplayEffects;

        private ObjectPool<ParticleSystem> _hitVfxPool;
        private WaitForSeconds _vfxLifeTimeWaiter;

        public AbstractAbilitySpec CreateSpec(AbilitySystemController source, AbilitySystemController target, Vector3 hitPoint, Vector3 hitDirection)
        {
            var hitAbilitySpec = (HitAbilitySpec)CreateSpec(source);
            hitAbilitySpec.hitTarget = target;
            hitAbilitySpec.hitPoint = hitPoint;
            hitAbilitySpec.hitDirection = hitDirection;
            return hitAbilitySpec;
        }

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new HitAbilitySpec(this, owner)
            {
                Level = owner.Level,
                HitVFXPool = _hitVfxPool,
            };
            foreach (var eventOnActivate in eventOnActivates)
            {
                spec.OnApplyGameplayEffect += eventOnActivate.PreApplyEffectSpec;
            }
            return spec;
        }

        public override void Initialize()
        {
            base.Initialize();
            _hitVfxPool = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 7);
            _vfxLifeTimeWaiter = new WaitForSeconds(hitVFX.main.duration);
        }

        public override void Reset()
        {
            base.Reset();
            _vfxLifeTimeWaiter = null;
            _hitVfxPool.Clear();
        }

        private ParticleSystem CreateVFX()
        {
            var vfxInstance = Instantiate(hitVFX);
            vfxInstance.gameObject.SetActive(false);
            return vfxInstance;
        }

        private void OnGetVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(true);
            vfx.Play();
            Coroutines.StartCoroutine(DelayReleaseVFX(vfx));
        }

        private void OnReleaseVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(false);
            vfx.Stop();
        }

        private void OnDestroyVFX(ParticleSystem vfx)
        {
            Destroy(vfx);
        }

        private IEnumerator DelayReleaseVFX(ParticleSystem vfx)
        {
            yield return _vfxLifeTimeWaiter;
            _hitVfxPool.Release(vfx);
        }

        private class HitAbilitySpec : AbstractAbilitySpec
        {
            public ObjectPool<ParticleSystem> HitVFXPool;
            public Vector3 hitPoint;
            public Vector3 hitDirection;
            public AbilitySystemController hitTarget;

            public HitAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(
                ability, owner)
            {
            }
            
            protected override bool CheckGameplayTags()
            {
                return true;
            }

            protected override IEnumerator PreActivate()
            {
                var vfx = HitVFXPool.Get();
                var vfxObject = vfx.gameObject;
                vfxObject.transform.position = hitPoint;
                vfxObject.transform.forward = -hitDirection;
                yield return null;
            }

            protected override IEnumerator ActivateAbility()
            {
                var abilitySo = Ability as HitAbilityScriptableObject;
                for (int index = 0; index < abilitySo!.hitGameplayEffects.Length; index++)
                {
                    var effectSpec = Owner.MakeOutgoingSpec(abilitySo!.hitGameplayEffects[index])
                        .SetTarget(hitTarget).SetIndex(index);
                    OnApplyGameplayEffect?.Invoke(effectSpec);
                    hitTarget.ApplyGameplayEffectSpecToSelf(effectSpec);
                }
                yield return null;
            }
            
            public override void CancelAbility()
            {
            }
        }
    }
}