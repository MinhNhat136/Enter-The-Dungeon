
using Atomic.Character.Model;
using Atomic.Core.Interface;

namespace Atomic.Character
{
    public interface IAiWeaponControlSystem : IInitializableWithBaseModel<BaseAgent>
    {
        public void ApplyAttack();
    }

}
