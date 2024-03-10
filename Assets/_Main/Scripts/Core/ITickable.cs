namespace Atomic.Core.Interface
{
    public interface ITickable
    {
        //  General Methods  ------------------------------
        public void EarlyTick();
        public void Tick();
        public void LateTick();
        public void FixedTick();
    }
}