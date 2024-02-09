using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyView : IPopupView
{
    [SerializeField] private UIButton buttonAccept;
    [SerializeField] private UIButton buttonTermsOfService;

    private PolicyViewController controller;

    public void Initialize(PolicyViewController controller)
    {
        this.controller = controller;
    }
}
