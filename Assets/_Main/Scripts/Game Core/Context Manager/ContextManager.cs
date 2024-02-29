using Atomic.Core;
using RMC.Core.Architectures.Mini.Context;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ContextManager : MonoBehaviour
{
    [SerializeField]
    private BaseGameObjectInitializableWithContext[] _modules;

    [Button]
    private bool CheckForDuplicateReferences()
    {
        Dictionary<BaseGameObjectInitializableWithContext, bool> encounteredElements = new();

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

    public void Start()
    {
        var context = new Context();
        foreach (var module in _modules)
        {
            module.Initialize(context);
        }
    }
}
