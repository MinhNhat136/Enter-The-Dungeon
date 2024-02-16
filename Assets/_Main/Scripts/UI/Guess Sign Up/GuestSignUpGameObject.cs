using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestSignUpGameObject : MonoBehaviour
{
    [SerializeField]
    private UIPopup _popupSignUp;

    [SerializeField]
    private ContextContainer _contextContainer;

    public void OnStart()
    {
        GuestSignUpMini guestSignUpMini = new(_popupSignUp)
        {
            Context = _contextContainer.Context
        };
        guestSignUpMini.Initialize();
    }

}
