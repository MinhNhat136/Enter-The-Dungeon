using Atomic.Character;
using UnityEngine;

namespace  Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Indicator", menuName = "Weapons/Indicator/VerticalArc", order = 2)]
    public class VerticalArcIndicatorBuilderScriptableObject : IndicatorBuilderScriptableObject
    {
        public override void Initialize(BaseAgent owner)
        {
            base.Initialize(owner);
            var position = owner.transform.position;
            trajectoryIndicator.SetPosition(indicatorPosition)
                .SetLaunchPosition(position)
                .SetPosition(position)
                .SetMaxDistance(maxDistance)
                .Set();
        }
    }
}
