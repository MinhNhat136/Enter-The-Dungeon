using Atomic.Character.Module;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Atomic.Character.Player
{
    public class PlayerTargetingSystem : MonoBehaviour
    {
        [SerializeField]
        private Rig _rigAim;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private GameObject _aimTarget;

        private float memorySpan = 3.0f;
        public float distanceWeight = 1.0f;

        AiVisionSensorMemory memory = new AiVisionSensorMemory(10);
        AiVisionSensorSystem sensor;

        public AiMemory bestMemory;

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

        private void Start()
        {
            sensor = GetComponent<AiVisionSensorSystem>();
        }


        // Update is called once per frame
        public void OnUpdate()
        {
            memory.UpdateSense(sensor, _layerMask);
            memory.ForgetMemory(memorySpan);

            EvaluateScores();
            UpdateAimPoint();
        }

        private void UpdateAimPoint()
        {
            if(Target != null)
            {
                _rigAim.weight = 1.0f;
                _aimTarget.transform.position = Target.transform.position;
            }
            else
                _rigAim.weight = 0.0f;

        }

        void EvaluateScores()
        {
            foreach (var memory in memory.memories)
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
            float distanceScore = Normalize(memory.distance, sensor.Distance) * distanceWeight;
            return distanceScore;
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }
    }

}

