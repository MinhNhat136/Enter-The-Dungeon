using Atomic.Collection;
using UnityEngine;

namespace Atomic.Character
{
    public class BaseSimpleDeadBody : MonoBehaviour, IObjectPoolable
    {
        public BasePoolSO<IObjectPoolable> PoolParent { get; set; }
        public virtual void Initialize(float force, float expansionRatio)
        {
            
        }
        public virtual void Setup(Vector3 pos, bool ragdollLayer)
        {
            
        }

        public virtual void Explode()
        {
            
        }
        public void ReturnToPool()
        {
            PoolParent.ReturnObjectToPool(this);
        }
    }
    
}
