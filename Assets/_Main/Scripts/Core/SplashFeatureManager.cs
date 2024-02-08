using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SplashFeatureManager : MonoBehaviour
{
    [Inject]
    private IObjectResolver container;

    [SerializeField] private GameObject policyPopupPrefab;
    [SerializeField] private GameObject signInPopupPrefab;


    private IPopupFactory popupFactory;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShowPolicy();
        }
    }

    private void ShowPolicy()
    {
        popupFactory = container.Resolve<PopupPolicyFactory>();
        IPopupView popupView = popupFactory.CreatePopup(policyPopupPrefab);
        popupView.Show();
    }

    private void ShowSignIn()
    {
        popupFactory = container.Resolve<PopupSignInFactory>();
        IPopupView popupView = popupFactory.CreatePopup(signInPopupPrefab);
        popupView.Show();
    }

}
