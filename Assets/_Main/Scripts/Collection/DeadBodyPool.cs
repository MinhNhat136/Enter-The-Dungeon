using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Collection
{
    [CreateAssetMenu(fileName = "DeadBodyPool", menuName = "PoolSO/DeadBodyPool")]
    public class DeadBodyPool : BasePoolSO<BaseSimpleDeadBody>
    {
        [SerializeField]
        private BaseSimpleDeadBody deadBodyPrefab;
        
        [SerializeField]
        private float force = 500f;

        [Range(0f, 1f)]
        [SerializeField]
        private float expansionRatio = 0.5f;
        
        public override void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;
            CreatePool();
        }

        public override void Destroy()
        {
            IsInitialized = false;
            objectPool.Clear();
        }

        protected override void CreatePool()
        {
            objectPool = new ObjectPool<BaseSimpleDeadBody>(
                createFunc: CreateObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: OnDestroyObject,
                collectionCheck: false,
                defaultCapacity: 3,
                maxSize: 10
            );
        }
        
        
        protected override BaseSimpleDeadBody CreateObject()
        {
            BaseSimpleDeadBody deadBody = Instantiate(deadBodyPrefab);
            deadBody.Initialize(force, expansionRatio);
            return deadBody;
        }

        protected override void OnGetObject(BaseSimpleDeadBody prefab)
        {
            prefab.gameObject.SetActive(true);
        }

        protected override void OnReleaseObject(BaseSimpleDeadBody prefab)
        {
            prefab.gameObject.SetActive(false);
        }

        protected override void OnDestroyObject(BaseSimpleDeadBody prefab)
        {
            Destroy(prefab);
        }

        public override void ReturnObjectToPool(BaseSimpleDeadBody prefab)
        {
            objectPool.Release(prefab);
        }
    }
}