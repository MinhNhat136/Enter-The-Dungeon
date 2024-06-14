using Atomic.Core;
using RMC.Core.Architectures.Mini.Context;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ContextManager : MonoBehaviour
{
    [Tooltip("CAUTION: Sort Service < Popup < View < Core")]
    [SerializeField]
    private BaseGameObjectInitializableWithContext[] _modules;

    [Button]
    private void CheckForDuplicateReferences()
    {
        Dictionary<BaseGameObjectInitializableWithContext, bool> encounteredElements = new();

        foreach (var module in _modules)
        {
            if (encounteredElements.ContainsKey(module))
            {
                Debug.LogError("Duplicate reference detected: " + module.gameObject.name);
            }
            else
            {
                encounteredElements.Add(module, true);
            }
        }
        Debug.Log("No duplicate module or reference found ");
    }

    //cai nay quen xoa day:V
    
    public void Start()
    {
        var context = new Context();
        foreach (var module in _modules)
        {
            module.Initialize(context);
        }
    }
}
