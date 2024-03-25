using Atomic.Core.Interface;

namespace Atomic.Character.Module
{
    public interface IAnimatorController : ITickable
    {
        public void ApplyAnimator();
    }
}
