using Atomic.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInControllerBuilder 
{
    private Dictionary<SignInType, ISignInController> _controllers = new();

    public SignInControllerBuilder SetSignInWithGuessController(ISignInController controller)
    {
        _controllers.Add(SignInType.Guess, controller);
        return this;
    }

    public SignInControllerBuilder SetSignInWithGameCenterController(ISignInController controller)
    {
        _controllers[SignInType.GameCenter] = controller;
        return this;
    }

    public SignInControllerBuilder SetSignInWithFacebookController(ISignInController controller)
    {
        _controllers[SignInType.Facebook] = controller;
        return this;
    }

    public SignInControllerBuilder SetSignInWithGoogleController(ISignInController controller)
    {
        _controllers[SignInType.Google] = controller;
        return this;
    }

    public ISignInController GetSignInController(SignInType type)
    {
        if (_controllers.TryGetValue(type, out ISignInController controller))
        {
            return controller;
        }
        else
        {
            throw new KeyNotFoundException($"Controller for sign-in type {type} not found.");
        }
    }
}
