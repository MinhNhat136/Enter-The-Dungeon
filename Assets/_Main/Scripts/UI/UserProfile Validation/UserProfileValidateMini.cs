using Atomic.Controllers;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using System;

public class UserProfileValidateMini : IMiniMvcs
{
    //  Properties ------------------------------------
    public bool IsInitialized
    {
        get { return _isInitialized; }
    }

    public IContext Context 
    { 
        get { return _context; } 
    }

    //  Fields ----------------------------------------
    private bool _isInitialized;
    private readonly IContext _context; 

    //  Dependencies ----------------------------------


    //  Initialization  -------------------------------
    public UserProfileValidateMini(IContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        if (!IsInitialized)
        {
            _isInitialized = true;

            UserProfileValidateService service = new();
            UserProfileValidateController controller = new(service);

            service.Initialize(_context);
            controller.Initialize(_context);
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new Exception("Sign in Mini not yet initialize");
        }
    }

}
