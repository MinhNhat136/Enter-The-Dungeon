using Atomic.Character.Model;
using Atomic.Character.Player;
using Atomic.Core.Interface;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>, 
    /// TODO: Replace with comments...
    /// </summary>
    [ShowOdinSerializedPropertiesInInspector]
    public class AiTargetingController : MonoBehaviour, ITargetingController
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
        [SerializeField]
        private string[] _targetMasks;

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<string, GameObject[]> layerTargets = new(4);

        //  Fields ----------------------------------------
        [SerializeField]
        private GameObject _aimTarget;

        [SerializeField]
        private float _memorySpan = 3.0f;

        [SerializeField]
        private float _turnSpeed = 3.0f;

        [SerializeField]
        private float _distanceWeight = 1.0f;

        [SerializeField]
        private int _maxRememberObjects = 8;

        private IAiMemoryController _memoryController;
        private IVisionController _sensor;
        private BaseAgent _model;
        private bool _isInitialized;

        public AiMemoryObject bestMemory;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _model = model;
                _sensor = _model.VisionController;
                _memoryController = _model.MemoryController;

                _memoryController.ForgetCondition = ForgetTargetCondition;

                for (int index = 0; index < _targetMasks.Length; index++)
                {
                    layerTargets.Add(_targetMasks[index], new GameObject[_maxRememberObjects]);
                }

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
            foreach (var targets in layerTargets)
            {
                _memoryController.UpdateSense(_sensor, targets.Key, targets.Value);
            }

            _memoryController.ForgetMemory();

            EvaluateScores();
            UpdateAimPoint();

        }

        private void UpdateAimPoint()
        {
            _aimTarget.transform.position = Vector3.Lerp(_aimTarget.transform.position, TargetPosition, _turnSpeed * Time.deltaTime);
        }

        void EvaluateScores()
        {
            foreach (var memory in _memoryController.Memories)
            {
                memory.Value.score = CalculateScore(memory.Value);
                if (bestMemory == null || memory.Value.score > bestMemory.score)
                {
                    bestMemory = memory.Value;
                }
            }
        }

        public float CalculateScore(AiMemoryObject memory)
        {
            float distanceScore = Normalize(memory.distance, _sensor.VisionDistance) * _distanceWeight;
            return distanceScore;
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }
        //  Event Handlers --------------------------------
        private bool ForgetTargetCondition(AiMemoryObject memory)
        {
            return memory.lastTime > _memorySpan || memory.gameObject == null ;
        }

        
    }

}

