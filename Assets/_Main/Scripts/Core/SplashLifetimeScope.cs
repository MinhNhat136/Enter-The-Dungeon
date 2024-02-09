using Doozy.Runtime.UIManager.Containers;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SplashLifetimeScope : LifetimeScope
{
/*    //[SerializeField] private SplashView splashView;
    [SerializeField] private SplashFeatureManager splashFeatureManager;*/


    protected override void Configure(IContainerBuilder builder)
    {
        RegisterFactory(builder);
        RegisterAllComponent(builder);
        RegisterController(builder);
        RegisterService(builder);
    }

    private void RegisterAllComponent(IContainerBuilder builder)
    {
        //builder.RegisterComponent(splashView);
        //builder.RegisterComponent(splashFeatureManager);
    }

    private void RegisterService(IContainerBuilder builder)
    {
        //builder.Register<SignInWithGuessService>(Lifetime.Singleton).As<IPopupService, ISignInService>().AsSelf();
        builder.Register<PolicyService>(Lifetime.Singleton).As<IPopupService>().AsSelf();
    }

    private void RegisterController(IContainerBuilder builder)
    {
        /*builder.Register<SignInWithGuessController>(Lifetime.Singleton).As<ISignInController>().AsSelf();*/
    }

    private void RegisterFactory(IContainerBuilder builder)
    {
        //builder.Register<SignInControllerFactory>(Lifetime.Singleton);
        builder.Register<PopupPolicyFactory>(Lifetime.Singleton).As<IPopupFactory>().AsSelf();
        builder.Register<PopupSignInFactory>(Lifetime.Singleton).As<IPopupFactory>().AsSelf();
    }
}
