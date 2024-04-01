namespace Atomic.Core.Interface
{
    public interface ITickable
    {
        //  General Methods  ------------------------------
        public void Tick();
        public virtual void FixedTick()
        {

        }
        public virtual void LateTick()
        {

        }
    }
}