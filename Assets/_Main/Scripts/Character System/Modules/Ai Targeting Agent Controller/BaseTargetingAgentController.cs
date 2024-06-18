using System.Linq;
using Atomic.Core;
using UnityEngine;
using UnityEngine.Serialization;

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
        

        public bool IsInitialized => _isInitialized;


        public BaseAgent Model => _model;

        //  Collections -----------------------------------
        private GameObject[] _targets;

        //  Fields ----------------------------------------
        private AiMemoryController _memoryController;
        protected AiVisionSensorController visionSensor;
        private BaseAgent _model;
        private bool _isInitialized;
        public TargetingConfig config;
        
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

                _targets = new GameObject[config.maxNumberTarget];
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        //  Unity Methods   -------------------------------
  
        
        //  Other Methods ---------------------------------
        private void Update()
        {
            FindTarget();
        }

        

        public virtual void FindTarget()
        {
            _memoryController.UpdateSenses(visionSensor, gameObject, config.targetLayer, _targets);
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
            float distanceScore = memory.distance.Normalize(visionSensor.config.distance) * config.distanceWeight;
            float angleScore = memory.angle.Normalize(visionSensor.config.angle) * config.angleWeight;
            return distanceScore + angleScore;
        }

        //  Event Handlers --------------------------------

    }
}

