using Atomic.Character;
using UnityEngine;

namespace  Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Indicator", menuName = "Weapons/Indicator/VerticalArc", order = 2)]
    public class VerticalArcIndicatorConfigScriptableObject : IndicatorConfigScriptableObject
    {
        public float MaxDistance;
        
        public override void Initialize(BaseAgent owner)
        {
            base.Initialize(owner);
            trajectoryIndicator.SetPosition(indicatorPosition)
                .SetLaunchPosition(owner.transform.position)
                .SetPosition(owner.transform.position)
                .SetMaxDistance(MaxDistance)
                .Set();
        }
        
    }
    
}
