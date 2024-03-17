using Atomic.Core.Interface;

namespace Atomic.Character.Module
{
    public interface IAnimatorController : IInitializable, ITickable
    {
        public void ApplyAnimator();
    }
}
