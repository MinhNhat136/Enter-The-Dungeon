using Atomic.Character;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class ProjectileStandard : ProjectileBase
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        [SerializeField] private ParticleSystem hitVfxPrefab;
        private float _speed;
        private float _distance;

        private ObjectPool<ParticleSystem> _hitVfxs;

        private Transform _mTransform;
        
        //  Initialization  -------------------------------
        public override ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            base.Spawn(owner, hitMask);
            _mTransform = transform;
            _hitVfxs = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 7);
            return this;
        }

        //  Unity Methods   -------------------------------
        public void Update()
        {
            _mTransform.position += _speed * Time.deltaTime * _mTransform.forward;
        }

        //  Other Methods ---------------------------------
        public override void Shoot()
        {
            _speed = SpeedWeight.GetValueFromRatio(EnergyValue);
            _distance = DistanceWeight.GetValueFromRatio(EnergyValue);

            Invoke(nameof(ReleaseAfterDelay), _distance / _speed);
        }

        public override void OnHit(Vector3 point, Vector3 normal)
        {
            
        }

        private ParticleSystem CreateVFX()
        {
            ParticleSystem vfxInstance = Instantiate(hitVfxPrefab);
            vfxInstance.gameObject.SetActive(false);
            return vfxInstance;
        }

        private void OnGetVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(true);
            vfx.Play();
            
            vfx.GetComponent<SelfPoolRelease>().Release(_hitVfxs, vfx);
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

        protected override void ReleaseAfterDelay() => Release?.Invoke(this);

        public void OnTriggerEnter(Collider other)
        {
            if (!HitMask.ContainsLayer(other.gameObject.layer))
            {
                return;
            }
            
            if (other.TryGetComponent(out BaseAgent agent))
            {
                foreach (var effect in PassiveEffect)
                {
                    var effectApply = effect.Clone();
                    effectApply.Target = agent;
                    effectApply.Source = Owner;
                    
                    effectApply.Apply();
                }
                
                var vfx = _hitVfxs.Get();
                
                var vfxObject = vfx.gameObject;
                vfxObject.transform.position = _mTransform.position;
                vfxObject.transform.forward = -_mTransform.forward;
                
                PassiveEffect.Clear();
                Release?.Invoke(this);
            }
        }

        //  Event Handlers --------------------------------
    }
}