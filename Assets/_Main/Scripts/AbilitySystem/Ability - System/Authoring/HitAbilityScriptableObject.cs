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

        protected ObjectPool<ParticleSystem> HitVfxPool;
        private WaitForSeconds _vfxLifeTimeWaiter;

        public AbstractAbilitySpec CreateHitSpecValue(AbilitySystemController source, AbilitySystemController target, Vector3 hitPoint, Vector3 hitDirection)
        {
            HitAbilitySpec hitAbilitySpec = (HitAbilitySpec)CreateSpec(source);
            hitAbilitySpec.hitTarget = target;
            hitAbilitySpec.hitPoint = hitPoint;
            hitAbilitySpec.hitDirection = hitDirection;
            if (eventOnActivate) hitAbilitySpec.OnApplyGameplayEffect += eventOnActivate.PreApplyEffectSpec;
            return hitAbilitySpec;
        }

        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new HitAbilitySpec(this, owner)
            {
                Level = owner.Level,
                HitVFXPool = HitVfxPool,
            };
            return spec;
        }

        public override void Initialize()
        {
            base.Initialize();
            HitVfxPool = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 7);
            _vfxLifeTimeWaiter = new WaitForSeconds(hitVFX.main.duration);
        }

        public override void Reset()
        {
            base.Reset();
            _vfxLifeTimeWaiter = null;
            HitVfxPool.Clear();
        }

        private ParticleSystem CreateVFX()
        {
            ParticleSystem vfxInstance = Instantiate(hitVFX);
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
            Destroy(vfx.gameObject);
        }

        private IEnumerator DelayReleaseVFX(ParticleSystem vfx)
        {
            yield return _vfxLifeTimeWaiter;
            HitVfxPool.Release(vfx);
        }
        
        public class HitAbilitySpec : AbstractAbilitySpec
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
                HitAbilityScriptableObject abilitySo = Ability as HitAbilityScriptableObject;
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