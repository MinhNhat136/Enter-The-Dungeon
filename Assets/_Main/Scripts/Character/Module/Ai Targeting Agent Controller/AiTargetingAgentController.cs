using Atomic.Character.Model;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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
        private GameObject[] targets ;

        //  Fields ----------------------------------------
        [SerializeField]
        private string targetLayer;

        [SerializeField]
        private float _memorySpan = 3.0f;

        [SerializeField]
        private float _distanceWeight = 1.0f;

        [SerializeField]
        private float _angleWeight = 1.0f;

        [SerializeField]
        private float _ageWeight = 1.0f;

        [SerializeField]
        private int _maxNumberTarget = 16; 

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
                targets = new GameObject[_maxNumberTarget];
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
            _memoryController.UpdateSenses(_sensor, targetLayer, targets);
            _memoryController.ForgetMemory();

            EvaluateTargetScores();
            UpdateTarget();
        }

        public void UpdateTarget()
        {
            if(bestMemory == null)
            {
                return;
            }
            if (bestMemory.gameObject == null)
            {
                return; 
            }
            if(!targets.Contains(bestMemory.gameObject))
            {
                bestMemory = null;
                return;
            }
            if(bestMemory.gameObject.TryGetComponent<BaseAgent>(out BaseAgent targetAgent))
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
                    Debug.Log("cap nhat");
                }
            }
        }

        public float CalculateScore(AiMemoryObject memory)
        {
            float distanceScore = Normalize(memory.distance, _sensor.VisionDistance) * _distanceWeight;
            float angleScore = Normalize(memory.angle, _sensor.VisionAngle) * _angleWeight;
            float ageScore = Normalize(memory.Age, _memorySpan) * _ageWeight;
            return distanceScore + angleScore + ageScore;
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }
        //  Event Handlers --------------------------------
        public bool ForgetTargetConditions(AiMemoryObject memory)
        {
            return memory.Age > _memorySpan;
        }
    }
}

