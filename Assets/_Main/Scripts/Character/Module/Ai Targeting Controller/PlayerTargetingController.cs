using Atomic.Character.Player;
using Atomic.Core.Interface;
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
    public class PlayerTargetingController : MonoBehaviour, ITickable, IInitializable
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

        //  Fields ----------------------------------------
        [SerializeField]
        private Rig _rigAim;

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

        [SerializeField]
        private string[] _targetMasks;

        private AiSensorMemory _memorySystem;
        private AiVisionSensorController _sensor;
        private PlayerAgent _model;
        private bool _isInitialized;

        public AiMemory bestMemory;

        [SerializeField]
        private Dictionary<string, GameObject[]> layerTargets = new(8);
        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _memorySystem = new AiSensorMemory();

                _model = GetComponent<PlayerAgent>();
                _sensor = GetComponent<AiVisionSensorController>();

                _memorySystem.ForgetAlgorithm = ForgetTarget;

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
                _memorySystem.UpdateSense(_sensor, targets.Key, targets.Value);
            }

            _memorySystem.ForgetMemory();

            EvaluateScores();
            UpdateAimPoint();

        }

        private void UpdateAimPoint()
        {
            _aimTarget.transform.position = Vector3.Lerp(_aimTarget.transform.position, TargetPosition, _turnSpeed * Time.deltaTime);
        }

        void EvaluateScores()
        {
            foreach (var memory in _memorySystem.memories)
            {
                memory.Value.score = CalculateScore(memory.Value);
                if (bestMemory == null || memory.Value.score > bestMemory.score)
                {
                    bestMemory = memory.Value;
                }
            }
        }

        public float CalculateScore(AiMemory memory)
        {
            float distanceScore = Normalize(memory.distance, _sensor.Distance) * _distanceWeight;
            return distanceScore;
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }
        //  Event Handlers --------------------------------
        private bool ForgetTarget(AiMemory memory)
        {
            return memory.lastTime > _memorySpan || memory.gameObject == null ;
        }
    }

}

