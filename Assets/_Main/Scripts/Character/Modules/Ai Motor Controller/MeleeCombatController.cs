using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class MeleeCombatController : MonoBehaviour, ICombatController
    {
        public Weapon CurrentWeapon { get; set; }

        public void BeginAttack()
        {
            Debug.Log("begin melee combat");
        }

        public void EndAttack()
        {
            Debug.Log("end melee combat");
        }
    }
}