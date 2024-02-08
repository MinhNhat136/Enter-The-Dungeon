using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using VContainer;

public class PopupPolicyFactory : IPopupFactory
{
    [Inject]
    private IObjectResolver container;

    public IPopupView CreatePopup(GameObject popupPrefab)
    {
        PolicyService policyService = container.Resolve<PolicyService>();
        PolicyViewController policyController = new(policyService);
        PolicyView policyView = popupPrefab.GetComponent<PolicyView>();
        policyView.Initialize(policyController);
        return policyView;
    }
}
