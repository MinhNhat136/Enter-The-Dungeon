using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface IHitBoxController : IInitializableWithBaseModel<BaseAgent>, IDamageable
    {
        public Collider Collider { get; set; }
        public void SetTriggerState(bool enabled);
        public void SetColliderState(bool enabled);
    }
}

