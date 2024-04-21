using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public interface ILocomotionController : IInitializableWithBaseModel<AiMotorController>
    {
        public float Acceleration { get; set; }
        public float MoveSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public void ApplyMovement();
        public void ApplyRotation();
        public void ApplyStop();
    }
}
