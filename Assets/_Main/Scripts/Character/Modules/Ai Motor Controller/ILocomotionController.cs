using Atomic.Character.Model;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    public interface ILocomotionController : IInitializableWithBaseModel<AiMotorController>
    {
        public bool IsStopped { get; set; }
        public Vector2 MoveInput { get; set; }
        public float Acceleration { get; set; }
        public float MoveSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public void ApplyMovement();
        public void ApplyRotation();
        public void Stop();
    }
}
