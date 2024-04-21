using System;
using Atomic.Character;
using Atomic.Core.Interface;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace  Atomic.Equipment
{
    [Serializable]
    public struct AttachPoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
    
    public interface IAttachWeaponController : IInitializable
    {
        public event Action<CombatMode, Weapon> OnActivate
        {
            add{}
            remove{}
        }
        
        public bool IsAttach { get; set; }
        public bool IsActivated { get; set; }
        
        public AttachWeaponType WeaponType { get; }
        
        public void Attach();
        public void Detach();
        public void Activate(bool value, Animator animator = null);
    }
}
