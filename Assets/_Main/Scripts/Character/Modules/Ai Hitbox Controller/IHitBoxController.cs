using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IHitBoxController : IInitializableWithBaseModel<BaseAgent>, IDamageable
    {
        public Collider Collider { get; set; }
        public void TurnOnTrigger();
        public void TurnOffTrigger();
        public void TurnOnHitBox();
        public void TurnOffHitBox();
    }
}

