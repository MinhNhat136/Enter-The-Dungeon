using Atomic.AbilitySystem;
using Atomic.Character;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The ProjectileBase class serves as an abstract base class for all projectiles in the game.
    /// It handles the initialization, launching, and visual effects of projectiles, as well as their 
    /// interactions with the environment and other game entities. This class implements the 
    /// IEnergyConsumer interfaces, indicating it can consume energy and be 
    /// managed within an object pool. 
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ProjectileBase : MonoBehaviour, IEnergyConsumer<ProjectileBase>
    {
        //  Events ----------------------------------------

        
        //  Properties ------------------------------------
        protected BaseAgent Owner { get; private set; }
        protected Vector3 ShootPosition { get; private set; }
        protected Vector3 ShootTarget { get; private set;}
        public ObjectPool<ProjectileBase> ProjectilePool { get; set; }
        
        public float EnergyValue { get; set; }
        protected MinMaxFloat SpeedWeight { get; private set; }
        protected MinMaxFloat DistanceWeight { get; set; }
        
        //  Fields ----------------------------------------
        [SerializeField] protected ParticleSystem trail;
        [SerializeField] protected AbstractRangeCastAbilityScriptableObject abilityOnHit;
        
        [HideInInspector]
        public Transform myTransform;
        
        //  Initialization  -------------------------------
        public ProjectileBase SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            DistanceWeight = distanceWeight;
            return this;
        }

        public ProjectileBase SetSpeedWeight(MinMaxFloat speedWeight)
        {
            SpeedWeight = speedWeight;
            return this;
        }
        
        // Unity Methods ----------------------------------
        private void Awake()
        {
            myTransform = transform;
            if (trail)
            {
                trail.Stop();
            }
        }

        private void OnDisable()
        {
            if (trail)
            {
                trail.Stop();
            }
            ProjectilePool.Release(this);
        }

        //  Other Methods ---------------------------------
        public virtual void Load(BaseAgent owner, Vector3 shootPosition, Vector3 shootDirection, Vector3 shootTarget, float energyValue)
        {
            CancelInvoke(nameof(ReleaseAfterDelay));            
            Owner = owner;
            ShootPosition = shootPosition;
            ShootTarget = shootTarget;
            EnergyValue = energyValue;

            var projectileTransform = transform;
            projectileTransform.position = shootPosition;
            projectileTransform.forward = shootDirection;
            if (trail)
            {
                trail.Play();
            }
        }

        public abstract void Shoot();
        protected abstract void ReleaseAfterDelay();

        //  Event Handlers --------------------------------
    }
}

