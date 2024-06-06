namespace Atomic.Equipment
{
    public class SingleBarrelController : BarrelController
    {
        public override void Charge()
        {
            if (chargeObject)
            {
                chargeObject.SetActive(true);
            }
        }

        public override void OnChargeFull()
        {
            if(onChargeFullFXPrefab) onChargeFullFXPrefab.Play();
        }

        public override void Shoot()
        {
            if (chargeObject)
            {
                chargeObject.SetActive(false);
            }
            shootSystem.Play();
        }
    }
}