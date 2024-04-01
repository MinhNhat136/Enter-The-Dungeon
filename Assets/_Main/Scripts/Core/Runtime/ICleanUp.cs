namespace Atomic.Core.Interface
{
    public interface ICleanUp
    {
        public bool IsCleanUp { get; }
        public void CleanUp();
    }

}
