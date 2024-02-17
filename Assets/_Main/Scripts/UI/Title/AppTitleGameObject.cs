using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

public class AppTitleGameObject : MonoBehaviour
{
    [SerializeField]
    private AppTitleView _titleView;

    [SerializeField]
    private UIPopup _signUpPopup;

    public void OnStart()
    {
        var context = new Context();
        AppTitleMini titleMini = new(_titleView, context);
        GuestSignUpMini guestSignUpMini = new(_signUpPopup, context);
        SignInValidationMini signInMini = new(context);

        titleMini.Initialize();
        signInMini.Initialize();
        guestSignUpMini.Initialize();
    }
}
