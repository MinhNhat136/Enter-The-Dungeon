using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character
{
    public interface ILocomotionController : IInitializableWithBaseModel<AiMotorController>
    {
        public bool IsNavMeshRotate { get; set; }
        public void ApplyMovement();
        public void ApplyRotation();
        public void ApplyStop();
    }
}
