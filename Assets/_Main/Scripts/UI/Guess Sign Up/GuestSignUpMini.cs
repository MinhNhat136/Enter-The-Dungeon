using Atomic.Command;
using Atomic.Controllers;
using Atomic.Services;
using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;
using UnityEngine;

public class GuestSignUpMini : IMiniMvcs
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
        set { _context = value; }
    }

    //  Fields ----------------------------------------
    [SerializeField]
    private UIPopup _popup;

    private bool _isInitialized;
    private IContext _context;

    //  Dependencies ----------------------------------


    //  Initialization  -------------------------------
    public GuestSignUpMini(UIPopup popup)
    {
        _popup = popup;
    }

    public void Initialize()
    {
        if (!IsInitialized)
        {
            _isInitialized = true;

            RequireContext();
            Context.CommandManager.AddCommandListener<SignInCompletionCommand>(InitSignUpMVC);
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new Exception("Sign Up Mini not yet initialize");
        }
    }

    public void RequireContext()
    {
        if (Context == null)
        {
            throw new Exception("Sign Up Mini not have Context");
        }
    }

    public void InitSignUpMVC(SignInCompletionCommand command)
    {
        if (command.WasSuccess) return;

        var popup = UIPopup.Get(_popup.name);
        if (popup.TryGetComponent<GuestSignUpView>(out GuestSignUpView view))
        {
            GuestSignUpService service = new();
            GuestSignUpController controller = new(view, service);

            service.Initialize(Context);
            view.Initialize(Context);
            controller.Initialize(Context);

            popup!.Show();
        }
        else throw new System.Exception("Module Policy null view");

    }
}
