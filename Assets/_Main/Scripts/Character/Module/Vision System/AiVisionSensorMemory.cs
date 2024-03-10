using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This class is responsible for managing and updating memories of detected objects by an AI senso.
    /// It tracks information about detected objects. 
    /// And also provides functionalities for updating, fetching, and forgetting memories based on specified criteria.
    /// </summary>
    public class AiVisionSensorMemory
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        [SerializeReference]
        public List<AiMemory> memories = new();
        public GameObject[] objects;


        //  Initialization  -------------------------------
        public AiVisionSensorMemory(int maxObjects)
        {
            objects = new GameObject[maxObjects];
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void UpdateSense(AiVisionSensorSystem sensor, string layerName)
        {
            int targets = sensor.Filter(objects, layerName);
            for (int i = 0; i < targets; i++)
            {
                GameObject target = objects[i];
                RefreshMemory(sensor.gameObject, target);
            }
        }

        public void RefreshMemory(GameObject agent, GameObject target)
        {
            AiMemory memory = FetchMemory(target);
            memory.gameObject = target;
            memory.position = target.transform.position;
            memory.direction = target.transform.position - agent.transform.position;
            memory.distance = memory.direction.magnitude;
            memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
            memory.lastTime = Time.time;
        }

        public AiMemory FetchMemory(GameObject gameObject)
        {
            AiMemory memory = memories.Find(x => x.gameObject == gameObject);
            if (memory == null)
            {
                memory = new AiMemory();
                memories.Add(memory);
            }
            return memory;
        }

        public void ForgetMemory(float olderThan)
        {
            memories.RemoveAll(m => m.Age > olderThan);
            memories.RemoveAll(m => !m.gameObject);
            //Add more logic here
        }

        //  Event Handlers --------------------------------

    }

}
