
namespace Atomic.Equipment
{
    public interface IEnergyConsumer 
    {
        public float EnergyValue { set; }
        public float MinEnergyValue { set; }
        public float MaxEnergyValue { set; }
        
        public IEnergyConsumer SetDistanceWeight(float distanceWeight)
        {
            return this;
        }

        public IEnergyConsumer SetVelocityWeight(float velocity)
        {
            return this;
        }

        public IEnergyConsumer SetScaleWeight(float scaleWeight)
        {
            return this;
        }

        public IEnergyConsumer SetTimeLifeWeight(float timeLifeWeight)
        {
            return this;
        }
    }
}
