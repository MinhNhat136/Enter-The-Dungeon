using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public interface IHitBoxController : IInitializableWithBaseModel<BaseAgent>, IDamageable
    {
        public Collider Collider { get; set; }
        public void SetTriggerState(bool enabled);
        public void SetColliderState(bool enabled);
    }
}

