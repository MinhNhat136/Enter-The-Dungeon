using UnityEngine;

public class SignInGameObject : MonoBehaviour
{

    [SerializeField]
    private ContextContainerSO contextContainer;

    public void OnStart()
    {
        SignInMini signInMini = new()
        {
            Context = contextContainer.Context
        };
        signInMini.Initialize();
    }

}
