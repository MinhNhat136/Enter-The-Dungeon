namespace Atomic.Collection
{
    public interface IObjectPoolable
    {
        public BasePoolSO<IObjectPoolable> PoolParent { get; set; }
        public void ReturnToPool();
    }
    
}
