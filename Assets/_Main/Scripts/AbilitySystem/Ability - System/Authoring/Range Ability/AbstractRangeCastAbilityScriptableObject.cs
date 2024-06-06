using System.Collections;
using System.Collections.Generic;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.AbilitySystem
{
    public abstract class AbstractRangeCastAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        [field: SerializeField]
        public LayerMask TargetLayerMask { get; private set; }
        
        [field: SerializeField]
        public ParticleSystem HitVFX { get; private set; }
        
        public GameplayEffectScriptableObject[] gameplayEffects;
        
        private ObjectPool<ParticleSystem> HitVFXPool { get; set; }
        
        private WaitForSeconds _vfxLifeTimeWaiter;
        
        public AbstractAbilitySpec CreateSpec(AbilitySystemController source, Vector3 hitPoint, Vector3 hitDirection)
        {
            AbstractRangeCastAbilitySpec hitAbilitySpec = (AbstractRangeCastAbilitySpec)CreateSpec(source);
            hitAbilitySpec.Level = source.Level;
            hitAbilitySpec.hitPoint = hitPoint;
            hitAbilitySpec.hitDirection = hitDirection;
            hitAbilitySpec.targetLayerMask = TargetLayerMask;
            hitAbilitySpec.hitVFXPool = HitVFXPool;
            if (eventOnActivate) hitAbilitySpec.OnApplyGameplayEffect += eventOnActivate.PreApplyEffectSpec;
            return hitAbilitySpec;
        }

        public override void Initialize()
        {
            base.Initialize();
            HitVFXPool = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 7);
            _vfxLifeTimeWaiter = new WaitForSeconds(HitVFX.main.duration);
        }

        public override void Reset()
        {
            base.Reset();
            _vfxLifeTimeWaiter = null;
            HitVFXPool.Clear();
        }

        private ParticleSystem CreateVFX()
        {
            ParticleSystem vfxInstance = Instantiate(HitVFX);
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
            HitVFXPool.Release(vfx);
        }

        protected abstract class AbstractRangeCastAbilitySpec : AbstractAbilitySpec
        {
            public Vector3 hitPoint;
            public Vector3 hitDirection;
            public LayerMask targetLayerMask;
            public ObjectPool<ParticleSystem> hitVFXPool;
            protected readonly List<AbilitySystemController> affectedControllers = new(16);
            
            protected AbstractRangeCastAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
            }

            protected override void EndAbility()
            {
                base.EndAbility();
                affectedControllers.Clear(); 
            }
        }
    }
    
}
