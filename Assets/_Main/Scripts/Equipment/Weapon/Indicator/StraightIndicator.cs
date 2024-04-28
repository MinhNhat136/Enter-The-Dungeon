using System;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Represents a straight trajectory indicator.
    /// </summary>
    public class StraightIndicator : MonoBehaviour, ITrajectoryIndicator
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

        private Vector3 _position;
        private Vector3 _forwardDirection;
        private float _launchDistance;
        private float _maxDistance;
        private Vector2 _directionTargetSize;
        private const float EdgeLengthOffset = 0.5f; 
        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------

        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            _position = position;
            return this; 
        }
        
        public ITrajectoryIndicator SetForwardDirection(Vector3 forwardDirection)
        {
            _forwardDirection = forwardDirection;
            return this;
        }
        
        public ITrajectoryIndicator SetLaunchDistance(float distance)
        {
            _launchDistance = distance;
            return this;
        }
        
        public  ITrajectoryIndicator SetMaxDistance(float value)
        {
            _maxDistance = value;
            return this;
        }
        
        public void Set()
        {
            transform.localPosition = _position;
            transform.localRotation = Quaternion.LookRotation(_forwardDirection);

            directionIndicator.size = new Vector2(directionLengthScale * _launchDistance, directionIndicator.size.y);
            
            left.transform.localScale =
                new Vector3(left.transform.localScale.x, left.transform.localScale.y, _launchDistance - EdgeLengthOffset);
            right.transform.localScale =
                new Vector3(right.transform.localScale.x, right.transform.localScale.y, _launchDistance - EdgeLengthOffset);
            _directionTargetSize = new Vector2(_maxDistance * directionLengthScale, directionIndicator.size.y);
        }

        public void Indicate()
        {
            if (!gameObject.activeSelf) return;
            SpreadEdgeLine(left, false);
            SpreadEdgeLine(right, true);
            
            ScaleEdgeLineLength(left);
            ScaleEdgeLineLength(right);

            ScaleSpriteLength();
        }

        private void SpreadEdgeLine(Transform edgeTransform, bool reverse = false)
        {
            edgeTransform.localPosition = Vector3.Slerp(edgeTransform.localPosition, reverse ? edgeTargetPosition : - edgeTargetPosition, speedIndicate * Time.deltaTime);
        }

        private void ScaleEdgeLineLength(Transform edgeTransform)
        {
            edgeTransform.localScale = new Vector3(1, 1, IndicateValue - EdgeLengthOffset);
        }

        private void ScaleSpriteLength()
        {
            directionIndicator
                    .size = /*Vector2.Lerp(directionIndicator.size, _directionTargetSize, speedIndicate * Time.deltaTime);*/
                new Vector2(IndicateValue * directionLengthScale, directionIndicator.size.y);
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

            left.transform.localScale = new Vector3(1, 1, _launchDistance- EdgeLengthOffset);
            right.transform.localScale = new Vector3(1, 1, _launchDistance - EdgeLengthOffset);
            
            directionIndicator.size = new Vector2(_launchDistance * directionLengthScale, directionIndicator.size.y);
        }

        //  Event Handlers --------------------------------
        
    }
    
}
