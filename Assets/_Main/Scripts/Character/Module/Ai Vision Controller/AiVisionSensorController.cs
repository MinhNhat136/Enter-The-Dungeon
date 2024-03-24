using Atomic.Core.Interface;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Linq;
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
        public float VisionDistance { get { return _distance; } }
        public float VisionAngle { get { return _angle; } }
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


        //  Fields ----------------------------------------

        [SerializeField]
        private float _distance = 10f;

        [SerializeField]
        private float _angle = 30f;

        [SerializeField]
        private float _height = 1.0f;

        [SerializeField]
        private int _scanFrequency = 30;

        [SerializeField]
        private int _maxObjectsRemember = 10;

        [SerializeField]
        private LayerMask _targetLayers;

        [SerializeField]
        private LayerMask _occlusionLayer;

        [SerializeField]
        private Color _meshColor = Color.red;

        [SerializeField]
        private List<GameObject> objects = new(32);


        private Collider[] colliders;
        private Mesh mesh;
        private int count;
        private float scanInterval;
        private float scanTimer;
        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize()
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                scanInterval = 1.0f / _scanFrequency;
                colliders = new Collider[_maxObjectsRemember];
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
            scanTimer -= Time.deltaTime;
            if (scanTimer <= 0)
            {
                scanTimer += scanInterval;
                Scan();
            }
        }

        private void OnValidate()
        {
            mesh = CreateWedMesh();
        }

        private void OnDrawGizmos()
        {
            if (mesh)
            {
                Gizmos.color = _meshColor;
                Gizmos.DrawMesh(mesh, transform.position, transform.rotation);

                Gizmos.DrawWireSphere(transform.position, _distance);
                for (int i = 0; i < count; i++)
                {
                    Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
                }

                Gizmos.color = Color.green;
                foreach (var obj in Objects)
                {
                    Gizmos.DrawSphere(obj.transform.position, 0.2f);
                }
            }
        }

        //  Other Methods ---------------------------------

        public void Scan()
        {
            count = Physics.OverlapSphereNonAlloc(transform.position, _distance, colliders, _targetLayers,
                QueryTriggerInteraction.Collide);
            Objects.Clear();
            for (int i = 0; i < count; i++)
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
            if (deltaAngle > _angle)
            {
                return false;
            }

            origin.y += _height / 2;
            dest.y = origin.y;
            if (Physics.Linecast(origin, dest, _occlusionLayer))
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

        Mesh CreateWedMesh()
        {
            Mesh mesh = new Mesh();

            int segments = 10;
            int numTriangles = (segments * 4) + 2 + 2;
            int numVertices = numTriangles * 3;
            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
            Vector3 bottomRight = Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;

            Vector3 topCenter = bottomCenter + Vector3.up * _height;
            Vector3 topRight = bottomRight + Vector3.up * _height;
            Vector3 topLeft = bottomLeft + Vector3.up * _height;

            int vert = 0;

            // left Side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            // right side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -_angle;
            float deltaAngle = (_angle * 2) / segments;
            for (int i = 0; i < segments; ++i)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance;

                topRight = bottomRight + Vector3.up * _height;
                topLeft = bottomLeft + Vector3.up * _height;

                // far side
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                // top
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;

                // bottom
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;

                currentAngle += deltaAngle;
            }


            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;

        }

        //  Event Handlers --------------------------------

    }
}
