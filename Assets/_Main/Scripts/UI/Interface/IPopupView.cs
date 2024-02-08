using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPopupView : MonoBehaviour
{
    private UIPopup GetPopup()
    {
        string popupName = name.Replace("(Clone)", "");
        return UIPopup.Get(popupName);
    }

    public void Show()
    {
        var popup = GetPopup();
        popup.Show();
    }

    public void Hide()
    {
        var popup = GetPopup();
        popup.Hide();
    }
}
