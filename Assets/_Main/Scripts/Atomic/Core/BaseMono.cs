using Atomic.Core;
using UnityEngine;

namespace Atomic.Core
{
    public abstract class BaseMono : MonoBehaviour, IEntity
    {
        [SerializeField] protected Identity identity;
        [SerializeField] protected Pools pools;
        [SerializeField] protected Ticker ticker;

        [SerializeField] protected bool initOnStart = true;
        protected Transform mTransform;

        protected virtual void Awake()
        {
            mTransform = transform; // Cache the Transform for performance
            BindVariable();
        }

        protected virtual void OnEnable()
        {
            DoEnable();
            ListenEvents();
            if (initOnStart) Initialize();
        }

        protected virtual void OnDisable()
        {
            DoDisable();
            StopListenEvents();
        }

        public virtual void BindVariable()
        {
            // Implementation for binding necessary variables
        }

        public virtual void ListenEvents()
        {
            // Subscribe to events
        }

        public virtual void DoEnable()
        {
            // Custom enable logic
        }

        public virtual void Initialize()
        {
            // Initialization logic here
        }

        public virtual void DoDisable()
        {
            // Custom disable logic
        }

        public virtual void StopListenEvents()
        {
            // Unsubscribe from events
        }

        // Implement other IEntity methods as needed...
    }
}
