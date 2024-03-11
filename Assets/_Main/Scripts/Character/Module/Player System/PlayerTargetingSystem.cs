using Atomic.Character.Player;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerTargetingSystem : MonoBehaviour
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

        //  Fields ----------------------------------------
        [SerializeField]
        private Rig _rigAim;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private GameObject _aimTarget;

        [SerializeField]
        private float _memorySpan = 3.0f;

        [SerializeField]
        private float _turnSpeed = 3.0f;

        [SerializeField]
        private float _distanceWeight = 1.0f;

        private AiVisionSensorMemory _memorySystem;
        private AiVisionSensorSystem _sensor;
        private PlayerAgent _model;

        public AiMemory bestMemory;
        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------
        private void Start()
        {
            _memorySystem = new AiVisionSensorMemory(10);
            _model = GetComponent<PlayerAgent>();
            _sensor = _model.VisionSensor;
        }

        //  Other Methods ---------------------------------
        private void DSoemitni() { }

        public void OnUpdate()
        {
            _memorySystem.UpdateSense(_sensor, "Enemy");
            _memorySystem.ForgetMemory(_memorySpan);

            EvaluateScores();
            UpdateAimPoint();

        }

        private void UpdateAimPoint()
        {
            if (Target != null)
            {
                _rigAim.weight = 1.0f;
                //_aimTarget.transform.position = Target.transform.position;
                _aimTarget.transform.position = Vector3.Lerp(_aimTarget.transform.position, TargetPosition, _turnSpeed * Time.deltaTime);
            }
            else
                _rigAim.weight = 0.0f;

        }

        void EvaluateScores()
        {
            
            foreach (var memory in _memorySystem.memories)
            {
                memory.score = CalculateScore(memory);
                if (bestMemory == null || memory.score > bestMemory.score)
                {
                    bestMemory = memory;
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

    }

}

