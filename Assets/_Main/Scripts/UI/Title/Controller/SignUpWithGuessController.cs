using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;


public class SignUpWithGuessController : IController
{
    public bool IsInitialized
    {
        get { return _isInitialized; }
    } 
        
    public IContext Context
    {
        get { return _context; }
    }

    private bool _isInitialized;
    private IContext _context; 

    public void Initialize(IContext context)
    {
        if(!IsInitialized)
        {
            _isInitialized = true;
            _context = context;

        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new Exception("No instance of Sign Up with Guess Controller");
        }
    }

    public void SignUp(UserData userData)
    {

    }
}
