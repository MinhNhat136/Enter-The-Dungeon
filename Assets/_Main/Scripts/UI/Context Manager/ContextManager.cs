using Atomic.Core;
using RMC.Core.Architectures.Mini.Context;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Scripting;

public class ContextManager : MonoBehaviour
{
    [SerializeField]
    private GameObjInitializableWithContext[] _modules;

    [Button]
    private bool CheckForDuplicateReferences()
    {
        Dictionary<GameObjInitializableWithContext, bool> encounteredElements = new Dictionary<GameObjInitializableWithContext, bool>();

        foreach (var module in _modules)
        {
            if (encounteredElements.ContainsKey(module))
            {
                Debug.LogError("Duplicate reference detected: " + module.gameObject.name);
                return true;
            }
            else
            {
                encounteredElements.Add(module, true);
            }
        }
        Debug.Log("No duplicate module or reference found ");
        return false;
    }

    private void Start()
    {
        var context = new Context();
        foreach (var module in _modules)
        {
            module.Initialize(context);
        }
    }
}
