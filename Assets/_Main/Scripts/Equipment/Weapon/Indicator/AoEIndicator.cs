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
        private Transform _startPosition;

        private float _angle;
        private float _distance;
        private float _tanAlpha;
        private float _timeToReachTarget;
        private Vector3 _initialVelocity;


        //  Initialization  -------------------------------
        public ITrajectoryIndicator SetAoEWeight(MinMaxFloat aoEWeight)
        {
            _aoEWeight = aoEWeight;
            return this;
        }

        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            transform.localPosition = Vector3.zero;
            return this;
        }

        public ITrajectoryIndicator SetStartPosition(Transform startPosition)
        {
            _startPosition = startPosition;
            _angle = Vector3.Angle(_startPosition.forward, transform.forward);
            return this;
        }

        public ITrajectoryIndicator SetEndPosition(Vector3 endPosition)
        {
            _endPosition = endPosition;
            _endPosition.y = 0.1f;
            _distance = Vector3.Distance(_startPosition.position, endPosition);
            _initialVelocity = ProjectileMotionExtension.CalculateInitialVelocity(_startPosition, _endPosition, _angle);
            _timeToReachTarget = ProjectileMotionExtension.TimeToReachTarget(_distance, _initialVelocity);
            return this;
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void Initialize()
        {
            aoeOriginalIndicator.localScale = Vector3.one * _aoEWeight.GetValueFromRatio(EnergyValue);
            line.positionCount = lineSegment;
        }

        public void Indicate()
        {
            if (!gameObject.activeSelf) return;
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
            var position = line.GetPosition(lineSegment - 1);
            position.y = 0.1f; 
            aoeIndicator.transform.position = position;
            aoeOriginalIndicator.transform.position = position;
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
                var pos = ProjectileMotionExtension.CalculatePosition(_startPosition.transform.position, _endPosition, _initialVelocity, _timeToReachTarget*index/(lineSegment-1));
                line.SetPosition(index, pos);
            }
        }
        //  Event Handlers --------------------------------
    }
}