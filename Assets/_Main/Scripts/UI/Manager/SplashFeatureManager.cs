using Atomic.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashFeatureManager : MonoBehaviour
{
    public TitleGameObject TitleGameObject;
    public SignInGameObject SignInGameObject;
    public NetworkGameObject NetworkGameObject;
    public PolicyGameObject PolicyGameObject;
    public SettingsGameObject SettingsGameObject;

    private void Start()
    {
        NetworkGameObject.OnStart();
        PolicyGameObject.OnStart();
        TitleGameObject.OnStart();
        SignInGameObject.OnStart();
        SettingsGameObject.OnStart();   
    }

}


