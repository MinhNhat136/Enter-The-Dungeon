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
    public class AiMemoryController : MonoBehaviour, IAiMemoryController
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public Dictionary<GameObject, AiMemoryObject> Memories => memories;

        public Predicate<AiMemoryObject> ForgetCondition { get {  return _forgetCondition; } set { _forgetCondition = value; } }

        //  Collections  ----------------------------------
        public Dictionary<GameObject, AiMemoryObject> memories = new Dictionary<GameObject, AiMemoryObject>(32);

        //  Fields ----------------------------------------
        private Predicate<AiMemoryObject> _forgetCondition;



        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void UpdateSense(IVisionController sensor, string layerName, GameObject[] objects)
        {
            int targets = sensor.Filter(objects, layerName);
            for (int i = 0; i < targets; i++)
            {
                RefreshMemory(objects[i]);
            }
        }

        public void RefreshMemory(GameObject target)
        {
            AiMemoryObject memory = FetchMemory(target);
            memory.gameObject = target;
            memory.position = target.transform.position;
            memory.direction = target.transform.position - transform.position;
            memory.layerIndex = target.layer;
            memory.distance = memory.direction.magnitude;
            memory.angle = Vector3.Angle(transform.forward, memory.direction);
            memory.lastTime = Time.time;
        }

        public AiMemoryObject FetchMemory(GameObject gameObject)
        {

            if (!memories.ContainsKey(gameObject))
            {
                AiMemoryObject newMemory = new AiMemoryObject();
                memories.Add(gameObject, newMemory);
            }

            return memories[gameObject];
        }

        public void ForgetMemory()
        {
            List<GameObject> keysToRemove = new List<GameObject>(32);

            foreach (var pair in memories)
            {
                AiMemoryObject memory = pair.Value;

                if (_forgetCondition(memory))
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
