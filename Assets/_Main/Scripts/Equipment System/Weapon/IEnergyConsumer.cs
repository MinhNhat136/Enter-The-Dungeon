
using Atomic.Core;

namespace Atomic.Equipment
{
    public interface IEnergyConsumer<out Self> where Self : IEnergyConsumer<Self> 
    {
        public float EnergyValue { set; }

        public Self SetDamageWeight(MinMaxFloat damageWeight)
        {
            return (Self)this;
        }
        
        public Self SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            return (Self)this;
        }

        public Self SetForceWeight(MinMaxFloat forceWeight)
        {
            return (Self)this;
        }

        public Self SetSpeedWeight(MinMaxFloat speedWeight)
        {
            return (Self)this;
        }
        
        public Self SetRadiusWeight(MinMaxFloat radiusWeight)
        {
            return (Self)this;
        }

        public Self SetAoEWeight(MinMaxFloat aoEWeight)
        {
            return (Self)this;
        }

        public Self SetGravityDownAcceleration(MinMaxFloat gravityDownAccelerationWeight)
        {
            return (Self)this;
        }
    }
}
