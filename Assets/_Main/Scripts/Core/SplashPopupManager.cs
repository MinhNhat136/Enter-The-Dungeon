using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SplashPopupManager 
{
    [Inject]
    private IObjectResolver container;

    private IPopupFactory GetPopupModules(PopupType type)
    {
        switch (type)
        {
            case PopupType.Policy:
                return container.Resolve<PopupPolicyFactory>();
            case PopupType.SignIn:
                return container.Resolve<PopupSignInFactory>();
            default:
                throw new System.Exception("Invalid SignInType");
        }
    }
}
