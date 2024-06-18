using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This class is responsible for detecting and managing objects in game environment. 
    /// It provides functionalities to scan for nearby objects, 
    /// determine if they are within the sensor's field of view, 
    /// and filter objects based on their layers.
    /// </summary>
    public class AiVisionSensorController : MonoBehaviour, ISensorController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        


        public List<GameObject> Objects => _objects;

        public bool IsInitialized => _isInitialized;

        public BaseAgent Model => _model;

        //  Fields ----------------------------------------
        public VisionConfig config;

        private List<GameObject> _objects = new(32);


        private Collider[] _colliders;
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
                
                _scanInterval = 1.0f / config.scanFrequency;
                _colliders = new Collider[config.maxObjectRemember];
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("AiVisionSensorSystem not initialized");
            }
        }

        //  Unity Methods   -------------------------------
        private void Update()
        {
            VisionScan();
        }

        public void VisionScan()
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
            _count = Physics.OverlapSphereNonAlloc(transform.position, config.distance, _colliders, config.visionLayer);
            Objects.Clear();
            for (int i = 0; i < _count; i++)
            {
                GameObject obj = _colliders[i].gameObject;
                if (IsInSight(obj) && !Objects.Contains(obj))
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
            if (deltaAngle > config.angle)
            {
                return false;
            }

            origin.y += config.height / 2;
            dest.y = origin.y;
            if (Physics.Linecast(origin, dest, config.occlusionLayer))
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