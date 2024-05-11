using UnityEngine;

namespace Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapons/Ranged/Config/Triple Shot", order = 1)]
    public class TripleShotWeaponScriptableObject : SingleShotWeaponScriptableObject
    {
        // The angle between each shot in degrees
        public float angleBetweenShots = 10f;

        public override void Shoot()
        {
            // Shoot the center bullet
            base.Shoot();

            // Calculate the rotation for the left and right bullets
            Quaternion leftRotation = Quaternion.Euler(0, -angleBetweenShots, 0);
            Quaternion rightRotation = Quaternion.Euler(0, angleBetweenShots, 0);

            // Calculate the direction for the left and right bullets
            Vector3 leftDirection = leftRotation * barrelController.transform.forward;
            Vector3 rightDirection = rightRotation * barrelController.transform.forward;

            // Shoot the left bullet
            var leftProjectile = projectilePool.Get();
            leftProjectile.Load(barrelController.shootSystem.transform.position, 
                leftDirection,
                _targetPosition, energyValue);
            barrelController.Shoot(leftProjectile);

            // Shoot the right bullet
            var rightProjectile = projectilePool.Get();
            rightProjectile.Load(barrelController.shootSystem.transform.position, 
                rightDirection,
                _targetPosition, energyValue);
            barrelController.Shoot(rightProjectile);
        }
    }
}