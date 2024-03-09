using System;
using Unity.VisualScripting;
using UnityEngine;


namespace Atomic.Character.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private const float REFERENCE_BULLET_SPEED = 20;
        //This is the default speed from whcih our mass formula is derived.


        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Transform gunPoint;



        [SerializeField] private Transform weaponHolder;



        private void Shoot()
        {
            GameObject newBullet =
                Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

            rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
            rbNewBullet.velocity = BulletDirection() * bulletSpeed;

            Destroy(newBullet, 10);
            GetComponentInChildren<Animator>().SetTrigger("Fire");
        }

        public Vector3 BulletDirection()
        {
            /* Transform aim = player.aim.Aim();

             Vector3 direction = (aim.position - gunPoint.position).normalized;

             if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
                 direction.y = 0;

             //weaponHolder.LookAt(aim);
             //gunPoint.LookAt(aim); TODO: find a better place for it. 

             return direction;*/
            return Vector3.zero;
        }

        public Transform GunPoint() => gunPoint;
    }

}
