using Atomic.Core;
using UnityEngine;

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

        //  Fields ----------------------------------------
        [SerializeField] private Transform aoeIndicator;
        [SerializeField] private Transform aoeOriginalIndicator;
        [SerializeField] private LineRenderer line;
        [SerializeField] private int lineSegment;
        
        private Vector3 _endPosition;
        private MinMaxFloat _aoEWeight;
        private MinMaxFloat _distanceWeight;
        private Transform _startPosition;

        private float _angle;
        private float _distance;
        private float _gravity;
        private float _tanAlpha;
        private float _height;
        private float _timeToReachTarget;
        private Vector3 _initialVelocity;


        //  Initialization  -------------------------------
        public ITrajectoryIndicator SetDistanceWeight(MinMaxFloat distanceWeight)
        {
            _distanceWeight = distanceWeight;
            return this;
        }

        public ITrajectoryIndicator SetAoEWeight(MinMaxFloat aoEWeight)
        {
            _aoEWeight = aoEWeight;
            return this;
        }

        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            transform.localPosition = Vector3.zero;
            aoeIndicator.localPosition = position;
            aoeOriginalIndicator.localPosition = position;
            return this;
        }

        public ITrajectoryIndicator SetStartPosition(Transform startPosition)
        {
            _startPosition = startPosition;
            return this;
        }

        public ITrajectoryIndicator SetEndPosition(Vector3 endPosition)
        {
            if (endPosition != Vector3.zero)
            {
                _endPosition = endPosition;
            }
            else
            {
                Quaternion forwardTarget = Quaternion.LookRotation(_startPosition.forward, Vector3.up);
                _endPosition = _startPosition.position +
                                  forwardTarget * Vector3.forward * _distanceWeight.GetValueFromRatio(EnergyValue);
            }
            _endPosition.y = 0;
            _initialVelocity = CalculateInitialVelocity(_startPosition.transform.position, _endPosition, _startPosition.forward);
            _timeToReachTarget = TimeToReachTarget();
            return this;
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void Initialize()
        {
            line.positionCount = lineSegment + 1;
            _gravity = Physics.gravity.y;
            
            aoeOriginalIndicator.localScale = Vector3.one * _aoEWeight.GetValueFromRatio(EnergyValue);
        }

        public void Indicate()
        {

            if (!gameObject.activeSelf) return;
            Debug.DrawRay(_startPosition.position, _startPosition.forward, Color.red, 10);
            Debug.DrawRay(_startPosition.position, transform.forward, Color.blue, 10);

            SetAoEIndicatorPos();
            IncreaseAoERadius();
            Visualize();
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
            aoeIndicator.transform.position = line.GetPosition(lineSegment - 1 ) + new Vector3(0, 0.1f, 0);
            aoeOriginalIndicator.transform.position = line.GetPosition(lineSegment - 1 )  + new Vector3(0, 0.1f, 0);
        }

        private void IncreaseAoERadius()
        {
            aoeIndicator.transform.localScale = new Vector3(_aoEWeight.GetValueFromRatio(EnergyValue),
                _aoEWeight.GetValueFromRatio(EnergyValue), _aoEWeight.GetValueFromRatio(EnergyValue));
        }
        
        void Visualize()
        {
            for (int index = 0; index < lineSegment; index++)
            {
                Vector3 pos = CalculatePosition(_startPosition.transform.position, _endPosition, _initialVelocity, _timeToReachTarget*index/lineSegment);
                line.SetPosition(index, pos);
            }
            var posFinal = CalculatePosition(_startPosition.transform.position, _endPosition, _initialVelocity, _timeToReachTarget);
            line.SetPosition(lineSegment, posFinal);
        }
        
        private Vector3 CalculateInitialVelocity(Vector3 startPosition, Vector3 endPosition, Vector3 direction)
        {
            _angle = Vector3.Angle(_startPosition.forward, transform.forward);
            _tanAlpha = Mathf.Tan(_angle * Mathf.Deg2Rad);
            _distance = Vector3.Distance(startPosition, endPosition);
            _height =  endPosition.y - startPosition.y ;
            
            var velocityXZ = Mathf.Sqrt(
                _gravity * _distance * _distance
                / (2.0f * (_height - _distance * _tanAlpha))
                );
            var velocityY = _tanAlpha * velocityXZ ;

            var forwardAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            
            var velocityX = velocityXZ * Mathf.Cos(forwardAngle * Mathf.Deg2Rad);
            var velocityZ = velocityXZ * Mathf.Sin(forwardAngle * Mathf.Deg2Rad);

            var velocity = new Vector3(velocityX, velocityY, velocityZ);
            return velocity;
        }
        
        private Vector3 CalculateVelocityAtTime(Vector3 velocity, float time)
        {
            float Vy  = velocity.y + _gravity * time;
            return new Vector3(velocity.x, Vy, velocity.z);
        }

        private float TimeToReachTarget() => _distance / Mathf.Sqrt(Mathf.Pow(_initialVelocity.z, 2) + Mathf.Pow(_initialVelocity.x, 2));

        Vector3 CalculatePosition(Vector3 startPosition, Vector3 endPosition, Vector3 velocity, float time)
        {
            var result = startPosition + velocity * time;
            result.y = 0.5f * Physics.gravity.y * time * time +
                       velocity.y * time + (startPosition.y - endPosition.y);
            return result;
        }
        //  Event Handlers --------------------------------
    }
}