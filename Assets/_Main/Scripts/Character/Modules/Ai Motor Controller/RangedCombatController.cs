using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class RangedCombatController : MonoBehaviour, ICombatController, IInitializable
    {
        public RangedWeapon RangedWeapon;
        
        public void BeginAttack()
        {
            Debug.Log("Begin ranged attack");
        }

        public void EndAttack()
        {
            Debug.Log("end ranged attack");
        }

        public bool IsInitialized { get; }
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
    }


}