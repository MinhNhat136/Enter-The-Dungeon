using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This class is responsible for managing and updating memories of detected objects by an AI sensor.
    /// It tracks information about detected objects. 
    /// And also provides functionalities for updating, fetching, and forgetting memories based on specified criteria.
    /// </summary>
    public class AiMemoryController
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public List<AiMemoryObject> Memories => memories;

        public float MemorySpan
        {
            get; set; 
        }
        //  Collections  ----------------------------------
        public List<AiMemoryObject> memories = new List<AiMemoryObject>(32);

        //  Fields ----------------------------------------
        [SerializeField]
        private float _memorySpan; 

        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void UpdateSenses(ISensorController sensor, GameObject unit, string layerName, GameObject[] objects)
        {
            int targets = sensor.Filter(objects, layerName);

            for (int i = 0; i < targets; i++)
            {
                RefreshMemory(objects[i], unit);
            }
        }

        private void RefreshMemory(GameObject target, GameObject unit)
        {
            AiMemoryObject memory = FetchMemory(target);
            memory.gameObject = target;
            memory.position = target.transform.position;
            memory.direction = target.transform.position - unit.transform.position;
            memory.layerIndex = target.layer;
            memory.distance = memory.direction.magnitude;
            memory.angle = Vector3.Angle(unit.transform.forward, memory.direction);
            memory.lastTime = Time.time;
        }

        private AiMemoryObject FetchMemory(GameObject gameObject)
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
            memories.RemoveAll(memory => memory.Age > _memorySpan) ;
        }

        //  Event Handlers --------------------------------

    }

}
