using Atomic.Character.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This class is responsible for detecting and managing objects in game environment. 
    /// It provides functionalities to scan for nearby objects, 
    /// determine if they are within the sensor's field of view, 
    /// and filter objects based on their layers.
    /// </summary>
    public class AiVisionSensorController : MonoBehaviour, IVisionController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public float VisionDistance { get; set; }
        public float VisionAngle { get; set; }
        public float VisionHeight { get; set ; }
        public float ScanFrequency { get; set; }
        public int MaxObjectRemember { get; set; }
        public LayerMask VisionLayer { get; set; }
        public LayerMask OcclusionLayer { get; set; }
        public Color MeshVisionColor { get; set; }


        public List<GameObject> Objects
        {
            get
            {
                return objects;
            }
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public BaseAgent Model
        {
            get { return _model; }
        }

        //  Fields ----------------------------------------
        private List<GameObject> objects = new(32);


        private Collider[] colliders;
        private int _count;
        private float _scanInterval;
        private float _scanTimer;
        private BaseAgent _model;
        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _model = model;
                _scanInterval = 1.0f / ScanFrequency;
                colliders = new Collider[MaxObjectRemember];
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("AiVisionSensorSystem not initialized");
            }
        }

        //  Unity Methods   -------------------------------
        public void Tick()
        {
            _scanTimer -= Time.deltaTime;
            if (_scanTimer <= 0)
            {
                _scanTimer += _scanInterval;
                Scan();
            }
        }

        //  Other Methods ---------------------------------

        public void Scan()
        {
            _count = Physics.OverlapSphereNonAlloc(transform.position, VisionDistance, colliders, VisionLayer,
                QueryTriggerInteraction.Collide);
            Objects.Clear();
            for (int i = 0; i < _count; i++)
            {
                GameObject obj = colliders[i].gameObject;
                if (IsInSight(obj))
                {
                    Objects.Add(obj);
                }
            }
        }

        private bool IsInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direction = dest - origin;

            direction.y = 0;
            float deltaAngle = Vector3.Angle(direction, transform.forward);
            if (deltaAngle > VisionAngle)
            {
                return false;
            }

            origin.y += VisionHeight / 2;
            dest.y = origin.y;
            if (Physics.Linecast(origin, dest, OcclusionLayer))
            {
                return false;
            }
            return true;
        }

        public int Filter(GameObject[] buffer, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            int count = 0;
            foreach (var obj in Objects)
            {
                if (obj.layer == layer)
                {
                    buffer[count++] = obj;
                }
                if (buffer.Length == count)
                {
                    break;
                }
            }
            return count;
        }

        
        //  Event Handlers --------------------------------

    }
}
