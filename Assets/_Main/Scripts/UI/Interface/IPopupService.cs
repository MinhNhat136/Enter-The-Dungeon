using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public interface IPopupService : INotifyPropertyChanged
{
    bool CanShowPopup();
    bool IsPopupShown { get; set; }
}
