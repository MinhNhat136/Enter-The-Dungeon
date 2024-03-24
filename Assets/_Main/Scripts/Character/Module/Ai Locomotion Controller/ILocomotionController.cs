using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface ILocomotionController: IInitializableWithBaseModel<BaseAgent>, ITickable
    {
        public Vector2 MoveInput { get; set; }
        public float TurnSpeed { get; set; }
        public float MoveSpeed { get; set; }
        public void ApplyMovement();
        public void ApplyRotation();
        public void Stop();

    }
}
