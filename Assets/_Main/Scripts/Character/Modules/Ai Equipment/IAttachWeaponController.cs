using System;
using System.Numerics;
using Atomic.Character.Module;
using Atomic.Core.Interface;
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
        public event Action OnAttach 
        { 
            add{}
            remove {}
        }

        public event Action OnDetach
        {
            add {}
            remove {}
        }
        
        public event Action<CombatMode> OnActivate
        {
            add{}
            remove{}
        }
        
        public bool IsAttach { get; set; }
        public bool IsActivated { get; set; }
        
        public AttachWeaponType WeaponType { get; }
        
        public void Attach();
        public void Detach();
        public void Activate(bool value);
    }
}
