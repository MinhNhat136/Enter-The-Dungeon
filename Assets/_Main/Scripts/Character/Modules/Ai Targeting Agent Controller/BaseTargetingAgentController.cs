using System.Linq;
using Atomic.Core;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The BaseTargetingAgentController class provides a base implementation for targeting systems in agents.
    /// This class handles the initialization of targeting components, finding and updating targets based on
    /// sensor data, and evaluating potential targets using configurable weights for distance, angle, and age.
    /// </summary>
    public class BaseTargetingAgentController : MonoBehaviour, ITargetingController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public string TargetLayer { get; set; }
        public float DistanceWeight { get; set; }
        public float AngleWeight { get; set; }
        public float AgeWeight { get; set; }
        public int MaxNumberTarget { get; set; }
        public ISensorController VisionSensor => visionSensor;

        public GameObject Target => _bestMemory.gameObject;

        public Vector3 TargetPosition => _bestMemory.position;

        public bool TargetInSight => _bestMemory.Age < 0.5f;

        public float TargetDistance => _bestMemory.distance;

        public bool IsInitialized => _isInitialized;

        public BaseAgent Model => _model;

        //  Collections -----------------------------------
        private GameObject[] _targets;

        //  Fields ----------------------------------------
        [SerializeField] private TargetingConfig _targetingConfig;

        private AiMemoryController _memoryController;
        protected AiVisionSensorController visionSensor;
        private BaseAgent _model;
        private bool _isInitialized;

        private AiMemoryObject _bestMemory;

        //  Initialization  -------------------------------
        public virtual void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _model = model;
                visionSensor = _model.VisionController;
                _memoryController = _model.MemoryController;

                _targetingConfig.Assign(this);
                _targets = new GameObject[MaxNumberTarget];
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------

        public virtual void FindTarget()
        {
            _memoryController.UpdateSenses(visionSensor, gameObject,TargetLayer, _targets);
            _memoryController.ForgetMemory();

            EvaluateTargetScores();
            UpdateTarget();
        }

        public virtual void UpdateTarget()
        {
            if (_bestMemory == null)
            {
                _model.TargetAgent = null;
                return;
            }
            if (_bestMemory.gameObject == null)
            {
                return;
            }
            if (!_targets.Contains(_bestMemory.gameObject))
            {
                _bestMemory = null;
                return;
            }
            if (_bestMemory.gameObject.TryGetComponent(out BaseAgent targetAgent))
            {
                _model.TargetAgent = targetAgent;
            }
        }

        public virtual void EvaluateTargetScores()
        {
            if(_memoryController.Memories.Count == 0)
            {
                _bestMemory = null;
            }
            foreach (var memory in _memoryController.Memories)
            {
                memory.score = CalculateScore(memory);
                if (_bestMemory == null || memory.score > _bestMemory.score)
                {
                    _bestMemory = memory;
                }
            }
        }

        protected virtual float CalculateScore(AiMemoryObject memory)
        {
            float distanceScore = memory.distance.Normalize(visionSensor.VisionDistance) * DistanceWeight;
            float angleScore = memory.angle.Normalize(visionSensor.VisionAngle) * AngleWeight;
            return distanceScore + angleScore;
        }

        //  Event Handlers --------------------------------

    }
}

