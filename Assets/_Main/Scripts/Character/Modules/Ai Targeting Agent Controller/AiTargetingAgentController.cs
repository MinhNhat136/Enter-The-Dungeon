using Atomic.Character.Model;
using System.Linq;
using Atomic.Character.Config;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>, 
    /// TODO: Replace with comments...
    /// </summary>
    public class AiTargetingAgentController : MonoBehaviour, ITargetingController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public string TargetLayer { get; set; }
        public float MemorySpan { get; set; }
        public float DistanceWeight { get; set; }
        public float AngleWeight { get; set; }
        public float AgeWeight { get; set; }
        public int MaxNumberTarget { get; set; }

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
        private IVisionController _sensor;
        private BaseAgent _model;
        private bool _isInitialized;

        private AiMemoryObject _bestMemory;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _model = model;
                _sensor = _model.VisionController;
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

        public void FindTarget()
        {
            _memoryController.UpdateSenses(_sensor, gameObject,TargetLayer, _targets);
            _memoryController.ForgetMemory();

            EvaluateTargetScores();
            UpdateTarget();
        }

        public void UpdateTarget()
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

        public void EvaluateTargetScores()
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

        private float CalculateScore(AiMemoryObject memory)
        {
            float distanceScore = Normalize(memory.distance, _sensor.VisionDistance) * DistanceWeight;
            float angleScore = Normalize(memory.angle, _sensor.VisionAngle) * AngleWeight;
            float ageScore = Normalize(memory.Age, MemorySpan) * AgeWeight;
            return distanceScore + angleScore + ageScore;
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }
        //  Event Handlers --------------------------------

    }
}

