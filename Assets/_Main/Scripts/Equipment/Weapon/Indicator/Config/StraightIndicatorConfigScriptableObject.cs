using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Indicator", menuName = "Weapons/Indicator/Straight", order = 1)]
    public class StraightIndicatorConfigScriptableObject : IndicatorConfigScriptableObject
    {
        public float maxDistance;
        public float launchDistance; 

        public override void Initialize(BaseAgent owner)
        {
            base.Initialize(owner);
            trajectoryIndicator.SetPosition(indicatorPosition)
                .SetForwardDirection(owner.transform.forward)
                .SetLaunchDistance(launchDistance)
                .SetMaxDistance(maxDistance)
                .Set();
        }
    }
    
}
