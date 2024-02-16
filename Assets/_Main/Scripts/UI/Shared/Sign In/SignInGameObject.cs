using Atomic.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInGameObject : MonoBehaviour
{

    [SerializeField]
    private ContextContainer contextContainer;

    public void OnStart()
    {
        SignInMini signInMini = new SignInMini();
        signInMini.Context = contextContainer.Context;
        signInMini.Initialize();
    }

}
