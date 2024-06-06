using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Core
{
    public class ScriptableObjectManager : MonoBehaviour
    {
        [SerializeField]
        private List<BaseSo> scriptableObjects = new(16);

        private void Awake()
        {
            foreach (var scriptableObject in scriptableObjects)
            {
                if (scriptableObject != null)
                {
                    scriptableObject.Initialize();
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var scriptableObject in scriptableObjects)
            {
                if (scriptableObject != null)
                {
                    scriptableObject.Reset();
                }
            }
        }

        public void AddScriptableObject(BaseSo scriptableObject)
        {
            if (scriptableObject != null && !scriptableObjects.Contains(scriptableObject))
            {
                scriptableObjects.Add(scriptableObject);
                scriptableObject.Initialize();
            }
        }

        public void RemoveScriptableObject(BaseSo scriptableObject)
        {
            if (scriptableObject != null && scriptableObjects.Contains(scriptableObject))
            {
                scriptableObjects.Remove(scriptableObject);
                scriptableObject.Reset();
            }
        }
    }
}