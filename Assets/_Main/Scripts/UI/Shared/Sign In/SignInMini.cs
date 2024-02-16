using Atomic.Controllers;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using System;

public class SignInMini : IMiniMvcs
{
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

            GuessSignInService guessSignInService = new();
            GuessSignInController guessSignInController = new(guessSignInService);

            RequireContext();
            guessSignInService.Initialize(_context);
            guessSignInController.Initialize(_context);
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new Exception("Sign in Mini not yet initialize");
        }
    }

    public void RequireContext()
    {
        if(Context == null)
        {
            throw new Exception("Sign in Mini not have Context");
        }
    }

}
