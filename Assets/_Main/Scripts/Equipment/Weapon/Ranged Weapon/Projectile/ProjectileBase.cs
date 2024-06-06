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
        public BaseAgent Owner { get; private set; }
        public Vector3 ShootPosition { get; private set; }
        protected Vector3 ShootTarget { get; private set;}
        public ObjectPool<ProjectileBase> ProjectilePool { get; set; }
        
        public float EnergyValue { get; set; }
        protected MinMaxFloat SpeedWeight { get; private set; }
        protected MinMaxFloat DistanceWeight { get; set; }
        public MinMaxFloat ForceWeight { get; private set; }
        
        //  Fields ----------------------------------------
        [SerializeField] protected ParticleSystem trail;
        [SerializeField] protected AbstractRangeCastAbilityScriptableObject abilityOnHit;
        
        [HideInInspector]
        public Transform myTransform;
        
        //  Initialization  -------------------------------
        public virtual ProjectileBase Spawn(BaseAgent owner)
        {
            myTransform = transform;
            Owner = owner;
            return this;
        }

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

        public ProjectileBase SetForceWeight(MinMaxFloat forceWeight)
        {
            ForceWeight = forceWeight;
            return this; 
        }
        // Unity Methods ----------------------------------
        private void Awake()
        {
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
        public virtual void Load( Vector3 shootPosition, Vector3 shootDirection, Vector3 shootTarget, float energyValue)
        {
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

