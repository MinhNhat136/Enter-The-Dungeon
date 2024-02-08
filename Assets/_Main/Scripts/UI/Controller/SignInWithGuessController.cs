using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VContainer;

public class SignInWithGuessController : ISignInController
{
    private SignInWithGuessService service;

    private ICommandPattern signedInCommand;

    public void Initialize(ICommandPattern command, SignInWithGuessService service)
    {
        signedInCommand = command;
        this.service = service; 
        
        this.service.PropertyChanged += (sender, e) =>
        {
            if(service.IsSignedIn())
            {
                OnSignInSuccess();
            }
            else OnSignInFailed();
        };
    }   

    public void StartSignInProcess()
    {
        service.SignIn();
    }

    public void OnSignInSuccess()
    {
        signedInCommand.Execute(null);
    }

    public void OnSignInFailed()
    {
        service.IsPopupShown = true;
    }
}
