using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SplashLifetimeScope : LifetimeScope
{
    [SerializeField] private SplashView splashView; 

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(splashView);

        builder.Register<SignInControllerFactory>(Lifetime.Singleton);
        builder.Register<SignInWithGuessService>(Lifetime.Singleton);
    }
}
