using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public sealed class AoEIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public float DelayActivateTime { get; set; }
        public float IndicateValue { get; set; }

        //  Fields ----------------------------------------
        [SerializeField] private Transform aoeIndicator;
        [SerializeField] private Transform aoeOriginalIndicator;
        [SerializeField] private LineRenderer line;
        [SerializeField] private int lineSegment;

        private Vector3 _targetPosition;
        private Transform _launchTransform;
        
        private float _maxDistance  = 10f;
        public float _maxTime = 0.7f; 
        
        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            transform.localPosition = Vector3.zero;
            aoeIndicator.localPosition = position;
            aoeOriginalIndicator.localPosition = position;
            return this;
        }

        public ITrajectoryIndicator SetLaunchTransform(Transform launchTransform)
        {
            _launchTransform = launchTransform;
            return this;
        }


        public ITrajectoryIndicator SetTarget(Vector3 targetPosition)
        {
            if (targetPosition == Vector3.zero)
            {
                Vector3 maxDistance = transform.position + transform.forward * _maxDistance;
                _targetPosition = maxDistance;
            }
            else
            {
                _targetPosition = targetPosition;
                _targetPosition.y = 0.5f;
            }
            return this;
        }

        public void Set()
        {
            line.positionCount = lineSegment + 1;
        }

        public void Indicate()
        {
            if (!gameObject.activeSelf) return;
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
            aoeIndicator.transform.localScale = new Vector3(IndicateValue, IndicateValue, IndicateValue);
        }

        private void DrawTrajectoryLine()
        {
            Vector3 velocity = CalculateVelocity(_targetPosition, _launchTransform.position, _maxTime);
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
            Vector3 resultPos = _launchTransform.position + velocity * time;
            resultPos.y = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (velocity.y * time) +
                          _launchTransform.position.y;
            return resultPos;
        }

        //  Event Handlers --------------------------------
    }
}