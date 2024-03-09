using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiTargetingSystem : MonoBehaviour
    {
        [HideInInspector] public readonly UnityEvent targetChange = new();

        private float memorySpan = 3.0f;
        public float distanceWeight = 1.0f;

        private LayerMask targetLayer; 

        AiVisionSensorMemory memory = new AiVisionSensorMemory(10);
        AiVisionSensorSystem sensor;

        AiMemory bestMemory;

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
            memory.UpdateSense(sensor, targetLayer);
            memory.ForgetMemory(memorySpan);

            EvaluateScores();
        }

        void EvaluateScores()
        {
            Debug.Log("hello");

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
