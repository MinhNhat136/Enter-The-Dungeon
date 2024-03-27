using Atomic.Character.Model;
using System.Linq;
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

        public GameObject Target
        {
            get { return bestMemory.gameObject; }
        }

        public Vector3 TargetPosition
        {
            get { return bestMemory.position; }
        }

        public bool TargetInSight
        {
            get { return bestMemory.Age < 0.5f; }
        }

        public float TargetDistance
        {
            get { return bestMemory.distance; }
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public BaseAgent Model
        {
            get { return _model; }
        }

        //  Collections -----------------------------------
        private GameObject[] targets;

        //  Fields ----------------------------------------
        

        private IAiMemoryController _memoryController;
        private IVisionController _sensor;
        private BaseAgent _model;
        private bool _isInitialized;

        private AiMemoryObject bestMemory;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                targets = new GameObject[MaxNumberTarget];
                _isInitialized = true;

                _model = model;
                _sensor = _model.VisionController;
                _memoryController = _model.MemoryController;

                _memoryController.ForgetCondition = ForgetTargetConditions;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------

        public void Tick()
        {
            _memoryController.UpdateSenses(_sensor, TargetLayer, targets);
            _memoryController.ForgetMemory();

            EvaluateTargetScores();
            UpdateTarget();
        }

        public void UpdateTarget()
        {
            if (bestMemory == null)
            {
                return;
            }
            if (bestMemory.gameObject == null)
            {
                return;
            }
            if (!targets.Contains(bestMemory.gameObject))
            {
                bestMemory = null;
                return;
            }
            if (bestMemory.gameObject.TryGetComponent<BaseAgent>(out BaseAgent targetAgent))
            {

                _model.TargetAgent = targetAgent;
            }
        }

        public void EvaluateTargetScores()
        {
            foreach (var memory in _memoryController.Memories)
            {
                memory.score = CalculateScore(memory);
                if (bestMemory == null || memory.score > bestMemory.score)
                {
                    bestMemory = memory;
                }
            }
        }

        public float CalculateScore(AiMemoryObject memory)
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
        public bool ForgetTargetConditions(AiMemoryObject memory)
        {
            return memory.Age > MemorySpan;
        }
    }
}

