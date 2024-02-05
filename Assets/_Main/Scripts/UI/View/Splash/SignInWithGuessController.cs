using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VContainer;

public class SignInWithGuessController : ISignInController
{
    private ISignInService service;

    private ICommand signedInCommand;

    public void Initialize(ICommand command, ISignInService service)
    {
        signedInCommand = command;
        this.service = service; 
        
        this.service.PropertyChanged += (sender, e) =>
        {
            if(service.IsSignedIn())
            {
                SignedIn();
            }
        };
    }   

    public void OnSignIn()
    {
        service.SignIn();
    }

    public void SignedIn()
    {
        signedInCommand.Execute(null);
    }
}
