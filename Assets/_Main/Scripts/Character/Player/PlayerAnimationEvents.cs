using UnityEngine;

namespace Atomic.Character.Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        private PlayerWeaponVisuals _visualController;

        private void Start()
        {
            _visualController = GetComponentInParent<PlayerWeaponVisuals>();
        }

        public void ReloadIsOver()
        {
            _visualController.MaximizeRigWeight();
            //refill-bullets
        }

        public void ReturnRig()
        {
            _visualController.MaximizeRigWeight();
            _visualController.MaximizeLeftHandWeight();
        }

        public void WeaponGrabIsOver()
        {
            _visualController.SetBusyGrabbingWeaponTo(false);
        }
    }
}

