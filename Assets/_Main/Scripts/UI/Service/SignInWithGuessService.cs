using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SignInWithGuessService : ISignInService, IPopupService
{
    public event PropertyChangedEventHandler PropertyChanged;

    private bool isPopupShown;

    public bool IsPopupShown
    {
        get => isPopupShown;
        set
        {
            isPopupShown = value;
            OnPropertyChanged("IsPopupShown");
        }
    }


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
        return false;
    }

    public bool CanShowPopup()
    {
        return !IsSignedIn();
    }
}
