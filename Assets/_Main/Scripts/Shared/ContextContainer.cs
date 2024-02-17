using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextContainer : MonoBehaviour
{
    protected IContext _context; 
    public virtual IContext Context
    {
        get
        {
            return _context;
        }
    }
}
