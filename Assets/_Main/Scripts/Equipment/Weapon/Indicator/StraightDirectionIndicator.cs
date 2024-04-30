using System;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Represents a straight trajectory indicator.
    /// </summary>
    public class StraightDirectionIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public float DelayActivateTime { get; set; }
        public float IndicateValue { get; set; }

        //  Fields ----------------------------------------
        [Header("DIRECTION-ARROW")] 
        [SerializeField] private float directionLengthScale;
        [SerializeField] private SpriteRenderer directionIndicator;

        [Header("EDGE-LINE")]
        [SerializeField] private Transform left;
        [SerializeField] private Transform right;
        [SerializeField] private Vector3 edgeTargetPosition; 

        [Header("PARAMETER")]
        [SerializeField] private float speedIndicate;

        private float _distanceWeight;
        private const float EdgeLengthOffset = 0.5f;
        
        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public ITrajectoryIndicator SetDistanceWeight(float distanceWeight)
        {
            _distanceWeight = distanceWeight; 
            return this;
        }
        
        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            transform.position = position;
            return this; 
        }

        public ITrajectoryIndicator SetLaunchTransform(Transform launchTransform)
        {
            return this;
        }
        
        public ITrajectoryIndicator SetForwardDirection(Vector3 forwardDirection)
        {
            transform.localRotation = Quaternion.LookRotation(forwardDirection);
            return this;
        }
        
        public void Indicate()
        {
            if (!gameObject.activeSelf) return;
            SpreadEdgeLine(left);
            SpreadEdgeLine(right, true);
            
            ScaleEdgeLineLength(left);
            ScaleEdgeLineLength(right);

            ScaleSpriteLength();
        }

        private void SpreadEdgeLine(Transform edgeTransform, bool reverse = false)
        {
            edgeTransform.localPosition = 
                Vector3.Slerp(edgeTransform.localPosition, reverse ? edgeTargetPosition : - edgeTargetPosition, speedIndicate * Time.deltaTime);
        }

        private void ScaleEdgeLineLength(Transform edgeTransform)
        {
            edgeTransform.localScale = new Vector3(1, 1, _distanceWeight * IndicateValue - EdgeLengthOffset);
        }

        private void ScaleSpriteLength()
        {
            directionIndicator.size = new Vector2( _distanceWeight * IndicateValue * directionLengthScale, directionIndicator.size.y);
        }
        
        public void Activate()
        {
            Invoke("DelayActivate", DelayActivateTime);
        }

        private void DelayActivate()
        {
            transform.gameObject.SetActive(true);
        }

        public void DeActivate()
        {
            CancelInvoke("DelayActivate");
            transform.gameObject.SetActive(false);
            Reset();
        }

        private void Reset()
        {
            left.transform.localPosition = Vector3.zero;
            right.transform.localPosition = Vector3.zero;
        }

        //  Event Handlers --------------------------------
        
    }
    
}
