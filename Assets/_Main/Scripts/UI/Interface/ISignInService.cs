using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public interface ISignInService : INotifyPropertyChanged 
{
    public void SignIn();
    public bool IsSignedIn();
}
