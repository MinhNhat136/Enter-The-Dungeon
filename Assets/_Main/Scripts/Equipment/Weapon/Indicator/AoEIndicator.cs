using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The AoEIndicator class represents an Area of Effect (AoE) indicator in a game.
    /// It is responsible for showing the AoE visually in the game world,
    /// and providing methods for setting its position, launch transform, and target.
    /// </summary>
    public sealed class AoEIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public float DelayActivateTime { get; set; }
        public float EnergyValue { get; set; }
        public float MinEnergyValue { get; set; }
        public float MaxEnergyValue { get; set; }

        //  Fields ----------------------------------------
        [SerializeField] private Transform aoeIndicator;
        [SerializeField] private Transform aoeOriginalIndicator;
        [SerializeField] private LineRenderer line;
        [SerializeField] private int lineSegment;

        private Vector3 _targetPosition;
        private Transform _launchTransform;

        public float _time = 0.7f;
        private float _scaleWeight;
        private float _distanceWeight; 
        private Transform _forwardDirection;

        //  Initialization  -------------------------------
        public ITrajectoryIndicator SetDistanceWeight(float distanceWeight)
        {
            _distanceWeight = distanceWeight;
            return this;
        }
        
        public ITrajectoryIndicator SetScaleWeight(float scaleWeight)
        {
            _scaleWeight = scaleWeight;
            return this;
        }

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
        
        public ITrajectoryIndicator SetForwardDirection(Transform forwardDirection)
        {
            _forwardDirection = forwardDirection;
            return this;
        }

        public ITrajectoryIndicator SetTarget(Vector3 targetPosition)
        {
            if (targetPosition != Vector3.zero)
            {
                _targetPosition = targetPosition;
            }
            else
            {
                _targetPosition = _launchTransform.position +
                                  _forwardDirection.forward * MaxEnergyValue * _distanceWeight;
            }
            _targetPosition.y = 0.1f;    
            return this;
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        public void Set()
        {
            line.positionCount = lineSegment;
            aoeOriginalIndicator.localScale = Vector3.one * MaxEnergyValue * _scaleWeight;
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
            aoeIndicator.transform.localScale = new Vector3(_scaleWeight * EnergyValue, _scaleWeight * EnergyValue, _scaleWeight * EnergyValue);
        }

        private void DrawTrajectoryLine()
        {
            Vector3 velocity = _forwardDirection.forward * _time/CalculateDistancePercent(_launchTransform.position, _targetPosition);
            Visualize(velocity);
        }

        void Visualize(Vector3 velocity)
        {
            for (int index = 0; index < lineSegment; index++)
            {
                Vector3 pos = CalculatePosAtTn(velocity, index / (float)(lineSegment));
                line.SetPosition(index, pos);
            }

        }

        float CalculateDistancePercent(Vector3 target, Vector3 origin)
        {
            return Vector3.Distance(origin, target)/
                   Vector3.Distance(origin,(origin + _forwardDirection.forward * MaxEnergyValue * _distanceWeight));

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