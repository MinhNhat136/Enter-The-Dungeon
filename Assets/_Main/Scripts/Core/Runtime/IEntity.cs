using Atomic.Core.Interface;

namespace Atomic.Core
{
    public interface IEntity : IInitializable, IDoEnable, ITickable, ICleanUp
    {
       
    }
}

