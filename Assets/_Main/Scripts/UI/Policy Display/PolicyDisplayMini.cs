using Atomic.Command;
using Atomic.Controllers;
using Atomic.Services;
using Atomic.UI.Views;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyDisplayMini : IMiniMvcs
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    public bool IsInitialized
    {
        get { return _isInitialized; }
    }

    public IContext Context
    {
        get { return _context; }
        private set { _context = value; }
    }

    //  Fields ----------------------------------------
    private bool _isInitialized;


    //  Dependencies ----------------------------------
    private readonly UIPopup _policyPopup;
    private IContext _context;

    //  Initialization  -------------------------------
    public PolicyDisplayMini(UIPopup policyPopup, IContext context)
    {
        _policyPopup = policyPopup;
        _context = context;
    }

    public void Initialize()
    {
        if (!IsInitialized)
        {
            _isInitialized = true;

            RequireContext();

            Context.CommandManager.AddCommandListener<PolicyValidateCompletionCommand>(Command_OnPolicyChecked);
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new System.Exception("No instance of Policy Display Mini");
        }
    }

    public void RequireContext()
    {
        if (Context == null)
        {
            throw new Exception("Policy Display Mini not have Context");
        }
    }

    //  Other Methods ---------------------------------
    public void Command_OnPolicyChecked(PolicyValidateCompletionCommand command)
    {
        if (command.IsAccepted) return;
        Debug.Log("show policy");
        InitPolicyPopupMVC();
        
    }


    public void InitPolicyPopupMVC()
    {
        var popup = UIPopup.Get(_policyPopup.name);
        Debug.Log("init nay");
        if (popup.TryGetComponent<PolicyInfoView>(out PolicyInfoView view))
        {
            PolicyDisplayService service = new();
            PolicyDisplayController controller = new(service, view);

            service.Initialize(Context);
            view.Initialize(Context);
            controller.Initialize(Context);

            popup!.Show();
        }
        else throw new System.Exception("Policy popup view component not found.");

    }

    //  Event Handlers --------------------------------
}
