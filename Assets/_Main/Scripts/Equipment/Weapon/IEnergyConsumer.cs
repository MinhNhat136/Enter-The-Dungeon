
using Atomic.Core;

namespace Atomic.Equipment
{
    public interface IEnergyConsumer<out Self> where Self : IEnergyConsumer<Self> 
    {
        public float EnergyValue { set; }
        public float MinEnergyValue { set; }
        public float MaxEnergyValue { set; }
        
        public Self SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            return (Self)this;
        }

        public Self SetVelocityWeight(MinMaxFloat velocity)
        {
            return (Self)this;
        }

        public Self SetScaleWeight(MinMaxFloat scaleWeight)
        {
            return (Self)this;
        }
    }
}
