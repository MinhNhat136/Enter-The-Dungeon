namespace Atomic.Core
{
    public interface IObjectPooleable
    {
        public void OnGetFromPool();
        public void OnReleaseToPool();
    }
}