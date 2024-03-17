using System;
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
    public class AiSensorMemory : IAiMemory
    {
        //  Events ----------------------------------------
        public Predicate<AiMemory> ForgetAlgorithm;

        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        public Dictionary<GameObject, AiMemory> memories = new Dictionary<GameObject, AiMemory>(32);


        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void UpdateSense(AiVisionSensorController sensor, string layerName, GameObject[] objects)
        {
            int targets = sensor.Filter(objects, layerName);
            for (int i = 0; i < targets; i++)
            {
                RefreshMemory(sensor.gameObject, objects[i]);
            }
        }

        public void RefreshMemory(GameObject agent, GameObject target)
        {
            AiMemory memory = FetchMemory(target);
            memory.gameObject = target;
            memory.position = target.transform.position;
            memory.direction = target.transform.position - agent.transform.position;
            memory.layerIndex = target.layer;
            memory.distance = memory.direction.magnitude;
            memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
            memory.lastTime = Time.time;
        }

        public AiMemory FetchMemory(GameObject gameObject)
        {

            if (!memories.ContainsKey(gameObject))
            {
                AiMemory newMemory = new AiMemory();
                memories.Add(gameObject, newMemory);
            }

            return memories[gameObject];
        }

        public void ForgetMemory()
        {
            List<GameObject> keysToRemove = new List<GameObject>(32);

            foreach (var pair in memories)
            {
                AiMemory memory = pair.Value;

                if (ForgetAlgorithm(memory))
                {
                    keysToRemove.Add(pair.Key);
                }
            }

            foreach (GameObject key in keysToRemove)
            {
                memories.Remove(key);
            }
        }

        //  Event Handlers --------------------------------

    }

}
