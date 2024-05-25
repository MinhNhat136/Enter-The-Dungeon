using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Collection
{
    [CreateAssetMenu(fileName = "Original Pool", menuName = "PoolSO/Original Pool")]
    public class PoolOriginal : BasePoolSO<BasePoolSO<IObjectPoolable>>
    {
        public List<IObjectPoolable> pool;
        
        public override void Initialize()
        {
        }

        public override void Destroy()
        {
        }

        protected override void CreatePool()
        {
        }

        protected override BasePoolSO<IObjectPoolable> CreateObject()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnGetObject(BasePoolSO<IObjectPoolable> prefab)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnReleaseObject(BasePoolSO<IObjectPoolable> prefab)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDestroyObject(BasePoolSO<IObjectPoolable> prefab)
        {
            throw new System.NotImplementedException();
        }

        public override void ReturnObjectToPool(BasePoolSO<IObjectPoolable> prefab)
        {
            throw new System.NotImplementedException();
        }
    }
}