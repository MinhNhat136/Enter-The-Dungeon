using System;
using Atomic.Character;
using Atomic.Core.Interface;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace  Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    [Serializable]
    public struct AttachPoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public interface IAttachWeaponController : IInitializable
    {

        //  Properties ------------------------------------
        public event Action<CombatMode, Weapon> OnActivate
        {
            add{}
            remove{}
        }
        public Weapon WeaponPrefab { get; }

        public bool IsAttach { get; set; }
        public bool IsActivated { get; set; }
        
        //  Other Methods ---------------------------------
        public void Attach();
        public void Detach();
        public void Activate(bool value);
    
        }
}
