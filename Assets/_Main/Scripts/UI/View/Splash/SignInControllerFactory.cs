using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VContainer;

public class SignInControllerFactory : IControllerFactory<SignInType, ISignInController>
{
    private Dictionary<SignInType, ISignInController> controllers = new();
    private ICommand signedInCommand;

    [Inject]
    private IObjectResolver container;

    public void SetSignedInCommand(ICommand command)
    {
        signedInCommand = command;
    }

    public ISignInController GetController(SignInType type)
    {
        if (controllers.ContainsKey(type))
        {
            return controllers[type];
        }

        ISignInController controller = CreateController(type);
        controllers.Add(type, controller);
        return controller;
    }

    private ISignInController CreateController(SignInType type)
    {
        switch (type)
        {
            case SignInType.Guest:
                ISignInController controller = new SignInWithGuessController();
                ISignInService service = container.Resolve<SignInWithGuessService>();
                controller.Initialize(signedInCommand, service);
                return controller; 
            default:
                throw new System.Exception("Invalid SignInType");
        }
    }
}
