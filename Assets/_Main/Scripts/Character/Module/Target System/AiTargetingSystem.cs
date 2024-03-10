using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiTargetingSystem : MonoBehaviour
    {
        [SerializeField] private string _targetLayer; 
        [SerializeField] private float _memorySpan = 3.0f;
        [SerializeField] public float _distanceWeight = 1.0f;


        private AiVisionSensorMemory memory = new AiVisionSensorMemory(10);
        private AiVisionSensorSystem sensor;
        private AiMemory bestMemory;

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

        // Start is called before the first frame update
        void Start()
        {
            sensor = GetComponent<AiVisionSensorSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            memory.UpdateSense(sensor, "Player");
            memory.ForgetMemory(_memorySpan);

            EvaluateScores();
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
            float distanceScore = Normalize(memory.distance, sensor.Distance) * _distanceWeight;
            return distanceScore;
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }
    }

}
