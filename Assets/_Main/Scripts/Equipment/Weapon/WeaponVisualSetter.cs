using UnityEngine;

namespace Atomic.Equipment
{
    public class WeaponVisualSetter : MonoBehaviour
    {
        [SerializeField]
        private WeaponVisual defaultVisual;

        [SerializeField]
        private WeaponVisual[] weaponVisuals;
        
        private void OnChanged()
        {
            
        }

    }
    
}
