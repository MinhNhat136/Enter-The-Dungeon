using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class GuestSignUpGameObject : MonoBehaviour
{
    [SerializeField]
    private UIPopup _popupSignUp;

    [SerializeField]
    private ContextContainerSO _contextContainer;

    public void OnStart()
    {
        GuestSignUpMini guestSignUpMini = new(_popupSignUp)
        {
            Context = _contextContainer.Context
        };
        guestSignUpMini.Initialize();
    }

}
