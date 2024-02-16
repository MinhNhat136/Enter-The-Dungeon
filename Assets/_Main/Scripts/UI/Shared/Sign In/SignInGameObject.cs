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
        SignInMini signInMini = new();
        signInMini.Context = contextContainer.Context;
        signInMini.Initialize();
    }

}
