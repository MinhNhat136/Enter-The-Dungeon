using Atomic.Controllers;
using Atomic.Core;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

public class SplashUIFlowBaseObject : BaseGameObjectInitializableWithContext
{

    [SerializeField]
    private SplashUIFlowView _view; 

    public override void Initialize(IContext context)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            _context = context;

            SplashUIFlowMini mini = new(_view);
            mini.Initialize(_context);

        }
    }

    public override void RequireIsInitialized()
    {
        if (!_isInitialized)
        {
            throw new System.Exception("Splash UI Flow Base Object not initialized");
        }
    }
}
