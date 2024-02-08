using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PolicyService : IPopupService
{
    private bool isAccept = false;
    private bool isPopupShown = false;

    public event PropertyChangedEventHandler PropertyChanged;
    
    public bool IsAccept
    {
        get => isAccept;
        private set 
        { 
            isAccept = value;
            IsPopupShown = false;
        }
    }
    public bool IsPopupShown
    {
        get => isPopupShown;
        set
        {
            isPopupShown = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPopupShown"));
        }
    }


    public void AcceptPolicy()
    {
        IsAccept = true;
    }

    public bool CanShowPopup()
    {
        return !IsAccept && !IsPopupShown;
    }
}
