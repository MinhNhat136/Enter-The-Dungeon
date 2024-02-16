using Atomic.Controllers;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUpMini : IMiniMvcs
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    public bool IsInitialized
    {
        get { return _isInitialized; }
    }

    public IContext Context
    {
        get { return _context; }
        set { _context = value; }
    }

    //  Fields ----------------------------------------
    private bool _isInitialized;
    private IContext _context;

    //  Dependencies ----------------------------------


    //  Initialization  -------------------------------
    public void Initialize()
    {
        if (!IsInitialized)
        {
            _isInitialized = true;


            
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new Exception("Sign Up Mini not yet initialize");
        }
    }

    public void RequireContext()
    {
        if (Context == null)
        {
            throw new Exception("Sign Up Mini not have Context");
        }
    }
}
