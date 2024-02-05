using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SignInWithGuessService : ISignInService
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void SignIn()
    {
        OnPropertyChanged("SignIn");
    }

    public bool IsSignedIn()
    {
        return true;
    }
}
