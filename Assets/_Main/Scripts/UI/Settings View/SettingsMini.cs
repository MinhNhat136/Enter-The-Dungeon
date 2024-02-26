using Atomic.Template;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMini : IMiniMvcs
{
    private RightGroupSettingsView _rightGroupSettingsView;
    private LeftGroupSettingsView _leftGroupSettingsView;
    private BottomGroupSettingsView _bottomGroupSettingsView;
    private bool _isInitialize;
    private IContext _context; 

    public SettingsMini(RightGroupSettingsView rightGroupSettingsView, LeftGroupSettingsView leftGroupSettingsView, BottomGroupSettingsView bottomGroupSettingsView)
    {
        _rightGroupSettingsView = rightGroupSettingsView;
        _leftGroupSettingsView = leftGroupSettingsView;
        _bottomGroupSettingsView = bottomGroupSettingsView;
    }
    public bool IsInitialized
    {
        get
        {
            return _isInitialize;
        }
    }

    public IContext Context
    {
        get
        {
            return _context;
        }
    }

    public void Initialize()
    {
        if(!IsInitialized)
        {
            _isInitialize = true;

            _context = new Context();

            RightGroupSettingsController rightController = new RightGroupSettingsController(_rightGroupSettingsView);


            _rightGroupSettingsView.Initialize(_context);
            rightController.Initialize(_context);

        }
    }

    public void RequireIsInitialized()
    {
        if(!IsInitialized)
        {
            throw new System.Exception("Setting Minis not initialize");
        }
    }

}
