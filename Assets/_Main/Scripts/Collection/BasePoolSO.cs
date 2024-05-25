using Atomic.Core.Interface;
using Sirenix.OdinInspector;
using UnityEngine.Pool;

namespace Atomic.Collection
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public abstract class BasePoolSO<T> : SerializedScriptableObject, IInitializable, IObjectPoolable where T : class, IObjectPoolable
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public BasePoolSO<IObjectPoolable> PoolParent { get; set; }
        public bool IsInitialized { get; protected set; }

        //  Fields ----------------------------------------
        public ObjectPool<T> objectPool;
        
        //  Initialization  -------------------------------
        public abstract void Initialize();
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Base Pool SO not initialized");
            }
        }
        public abstract void Destroy();
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        protected abstract void CreatePool();
        protected abstract T CreateObject();
        protected abstract void OnGetObject(T prefab);
        protected abstract void OnReleaseObject(T prefab);
        protected abstract void OnDestroyObject(T prefab);
        public abstract void ReturnObjectToPool(T prefab);
        public void ReturnToPool()
        {
            PoolParent.ReturnObjectToPool(this);
        }

        //  Event Handlers --------------------------------
        
    }
}
