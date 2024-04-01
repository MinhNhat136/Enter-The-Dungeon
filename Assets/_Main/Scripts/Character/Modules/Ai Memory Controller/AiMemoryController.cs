using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This class is responsible for managing and updating memories of detected objects by an AI sensor.
    /// It tracks information about detected objects. 
    /// And also provides functionalities for updating, fetching, and forgetting memories based on specified criteria.
    /// </summary>
    public class AiMemoryController : MonoBehaviour, IAiMemoryController
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public List<AiMemoryObject> Memories => memories;

        public Predicate<AiMemoryObject> ForgetCondition { get { return _forgetCondition; } set { _forgetCondition = value; } }

        //  Collections  ----------------------------------
        public List<AiMemoryObject> memories = new List<AiMemoryObject>(32);

        //  Fields ----------------------------------------
        private Predicate<AiMemoryObject> _forgetCondition;

        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void UpdateSenses(IVisionController sensor, string layerName, GameObject[] objects)
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
            AiMemoryObject memory = memories.Find(x => x.gameObject == gameObject);
            if (memory == null)
            {
                memory = new AiMemoryObject();
                memories.Add(memory);
            }
            return memory;
        }

        public void ForgetMemory()
        {
            memories.RemoveAll(_forgetCondition);
        }

        //  Event Handlers --------------------------------

    }

}
