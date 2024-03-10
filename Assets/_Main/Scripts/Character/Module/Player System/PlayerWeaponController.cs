using UnityEngine;


namespace Atomic.Character.Player
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerWeaponController : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private const float REFERENCE_BULLET_SPEED = 20;


        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private Transform weaponHolder;


        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
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

        //  Event Handlers --------------------------------






    }

}
