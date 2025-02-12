using Atomic.Core;
using UnityEngine;


namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The StraightDirectionIndicator class represents a straight trajectory indicator in a game.
    /// It provides methods for setting its position, forward direction, and distance weight.
    /// </summary>
    public class StraightDirectionIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public float DelayActivateTime { get; set; }
        public float EnergyValue { get; set; }

        //  Fields ----------------------------------------
        [Header("DIRECTION-ARROW")] 
        [SerializeField] private float directionLengthScale;
        [SerializeField] private SpriteRenderer directionIndicator;

        [Header("EDGE-LINE")]
        [SerializeField] private Transform leftSideEdge;
        [SerializeField] private Transform rightSideEdge;
        [SerializeField] private float edgeDistanceScale;
        
        private MinMaxFloat _distanceWeight;
        private const float EdgeLengthOffset = 0.5f;
        
        //  Initialization  -------------------------------
        public ITrajectoryIndicator SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            _distanceWeight = distanceWeight; 
            return this;
        }
        
        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            transform.position = position;
            return this; 
        }
        
        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------
        public void Indicate()
        {
            if (!gameObject.activeSelf) return;
            SpreadEdgeLine(leftSideEdge);
            SpreadEdgeLine(rightSideEdge, true);
            
            ScaleEdgeLineLength(leftSideEdge);
            ScaleEdgeLineLength(rightSideEdge);

            ScaleSpriteLength();
        }

        private void SpreadEdgeLine(Transform edgeTransform, bool reverse = false)
        {
            var distance = _distanceWeight.GetValueFromRatio(EnergyValue) * (reverse ? edgeDistanceScale : -edgeDistanceScale);
            edgeTransform.localPosition = new Vector3(distance, 0, 0);
        }

        private void ScaleEdgeLineLength(Transform edgeTransform)
        {
            edgeTransform.localScale = new Vector3(1, 1, _distanceWeight.GetValueFromRatio(EnergyValue) - EdgeLengthOffset);
        }

        private void ScaleSpriteLength()
        {
            directionIndicator.size = new Vector2(_distanceWeight.GetValueFromRatio(EnergyValue) * directionLengthScale,
                directionIndicator.size.y);
        }
        
        public void Activate()
        {
            Invoke(nameof(ActiveGameObject), DelayActivateTime);
        }

        private void ActiveGameObject()
        {
            transform.gameObject.SetActive(true);
        }

        public void DeActivate()
        {
            CancelInvoke(nameof(ActiveGameObject));
            transform.gameObject.SetActive(false);
        }

        //  Event Handlers --------------------------------
        
    }
    
}
