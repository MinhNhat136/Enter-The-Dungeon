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


        private Transform _mTransform;
        
        //  Initialization  -------------------------------
        public override ProjectileBase Spawn(BaseAgent owner, LayerMask hitMask)
        {
            base.Spawn(owner, hitMask);
            _mTransform = transform;
            HitVfx = new ObjectPool<ParticleSystem>(CreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1, 7);
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
            actionOnHit.Initialize();

            Invoke(nameof(ReleaseAfterDelay), _distance / _speed);
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
            
            vfx.GetComponent<SelfPoolRelease>().Release(HitVfx, vfx);
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
            
            actionOnHit.OnHit(_mTransform.position, _mTransform.forward, other);
        }

        //  Event Handlers --------------------------------
    }
}