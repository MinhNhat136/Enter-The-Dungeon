using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VContainer;

public class SignInControllerFactory : IModuleFactory<SignInType, ISignInController>
{
    private Dictionary<SignInType, ISignInController> controllers = new();
    private ICommandPattern signedInCommand;

    [Inject]
    private readonly IObjectResolver container;

    public void SetSignedInCommand(ICommandPattern command)
    {
        signedInCommand = command;
    }

    public ISignInController GetModule(SignInType type)
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
                SignInWithGuessController controller = container.Resolve<SignInWithGuessController>();
                SignInWithGuessService service = container.Resolve<SignInWithGuessService>();
                controller.Initialize(signedInCommand, service);
                return controller; 
            default:
                throw new System.Exception("Invalid SignInType");
        }
    }
}
