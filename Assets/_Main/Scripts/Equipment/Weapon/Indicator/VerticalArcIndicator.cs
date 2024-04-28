using NodeCanvas.Tasks.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.Equipment
{
    public sealed class VerticalArcIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        
        public float DelayActivateTime { get; set; }
        public float IndicateValue { get; set; }


        [SerializeField] 
        private Transform aoeIndicator;
        [SerializeField] 
        private Transform aoeOriginalIndicator;
        [SerializeField] 
        private LineRenderer line;
        [SerializeField] 
        private int lineSegment;

        private Vector3 _launchPosition;
        private Vector3 _targetPosition;
        private float _maxDistance;
        private bool IsActive = false;


        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            transform.localPosition = Vector3.zero;
            aoeIndicator.localPosition = position;
            aoeOriginalIndicator.localPosition = position;
            return this;
        }

        public ITrajectoryIndicator SetLaunchPosition(Vector3 launchPosition)
        {
            _launchPosition = launchPosition;
            return this;
        }

        public ITrajectoryIndicator SetTarget(Transform targetPosition)
        {
            if (!targetPosition)
            {
                _targetPosition = transform.forward + new Vector3(0, 0, _maxDistance);
            }
            else
            {
                _targetPosition = targetPosition.position;
            }
            _targetPosition.y = 0.5f;
            return this;
        }

        public ITrajectoryIndicator SetMaxDistance(float distance)
        {
            _maxDistance = distance;
            return this;
        }

        public void Set()
        {
            line.positionCount = lineSegment + 1;
        }
        

        public void Indicate()
        {
            if (gameObject.activeSelf == false) return;
            SetAoEIndicatorPos();
            IncreaseAoERadius();
            DrawTrajectoryLine();
        }
        

        public void Activate()
        {
            Invoke(nameof(ActiveGameObject), DelayActivateTime);
        }

        public void ActiveGameObject() => gameObject.SetActive(true);
        
        public void DeActivate()
        {
            CancelInvoke(nameof(ActiveGameObject));
            gameObject.SetActive(false);
            aoeIndicator.localScale = Vector3.one;
        }

        private void SetAoEIndicatorPos()
        {
            aoeIndicator.transform.position = _targetPosition;
            aoeOriginalIndicator.transform.position = _targetPosition;
        }

        private void IncreaseAoERadius()
        {
            aoeIndicator.transform.localScale = Vector3.Slerp(aoeIndicator.transform.localScale,
                aoeOriginalIndicator.transform.localScale, 3 * Time.deltaTime);
        }

        private void DrawTrajectoryLine()
        {
            Vector3 velocity = CalculateVelocity(_targetPosition, _launchPosition, 1);
            Visualize(velocity); 
        }

        void Visualize(Vector3 velocity)
        {
            for (int index = 0; index < lineSegment; index++)
            {
                Vector3 pos = CalculatePosAtTn(velocity, index / (float)(lineSegment));
                line.SetPosition(index, pos);
            }
            
            line.SetPosition(lineSegment, _targetPosition);
        }
        
        Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXz = distance;
            distanceXz.y = 0f;
            
            float xzVelocity = distanceXz.magnitude / time;
            float yVelocity = (distance.y / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);
 
            Vector3 resultVelocity = distanceXz.normalized;
            resultVelocity *= xzVelocity;
            resultVelocity.y = yVelocity;
 
            return resultVelocity;
        }
        
        Vector3 CalculatePosAtTn(Vector3 velocity, float time)
        {
            Vector3 resultPos = _launchPosition + velocity * time;
            resultPos.y = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (velocity.y * time) + _launchPosition.y;
            return resultPos;
        }

    }
}