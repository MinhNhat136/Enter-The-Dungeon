using Atomic.Core.Interface;

namespace Atomic.Character.Module
{
    public interface ILocomotionController: IInitializable, ITickable
    {
        public void ApplyMovement();
        public void ApplyRotation();
    }
}
