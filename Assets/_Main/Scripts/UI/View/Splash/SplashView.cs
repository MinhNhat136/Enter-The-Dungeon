using Doozy.Runtime.UIManager.Components;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class SplashView : MonoBehaviour
{
    [SerializeField] private UIButton buttonSignInWithGameCenter;
    [SerializeField] private UIButton buttonSignInWithFacebook;
    [SerializeField] private UIButton buttonSignInWithGoogle;
    [SerializeField] private UIButton buttonSignInWithGuest;

    [SerializeField] private UIButton buttonTapToStart;

    [Inject]
    private SignInControllerFactory signInControllerFactory;

    private ISignInController signInController;
    private ICommand signedInCommand;

    private void OnEnable()
    {
        AddActionToSignInButtons();
        AddActionToTapToStartButton();
        SetVisibleSignInButtons(true);
        SetCommandToFactory();
    }

    private void OnDisable()
    {
        RemoveActionOfSignInButtons();
    }

    private void SetCommandToFactory()
    {
        signedInCommand = new RelayCommand<string>(_ => true, _ => SignedIn());
        signInControllerFactory.SetSignedInCommand(signedInCommand);
    }

    private void AddActionToSignInButtons()
    {
        buttonSignInWithGoogle.onClickEvent.AddListener(() => OnSignIn(signInControllerFactory.GetController(SignInType.Google)));
        buttonSignInWithFacebook.onClickEvent.AddListener(() => OnSignIn(signInControllerFactory.GetController(SignInType.Facebook)));
        buttonSignInWithGameCenter.onClickEvent.AddListener(() => OnSignIn(signInControllerFactory.GetController(SignInType.GameCenter)));
        buttonSignInWithGuest.onClickEvent.AddListener(() => OnSignIn(signInControllerFactory.GetController(SignInType.Guest)));
    }

    private void AddActionToTapToStartButton()
    {
        buttonTapToStart.onClickEvent.AddListener(() => OnLoadingMainScene());
    }

    private void RemoveActionOfSignInButtons()
    {
        buttonSignInWithGoogle.onClickEvent.RemoveAllListeners();
        buttonSignInWithFacebook.onClickEvent.RemoveAllListeners();
        buttonSignInWithGameCenter.onClickEvent.RemoveAllListeners();
        buttonSignInWithGuest.onClickEvent.RemoveAllListeners();
    }

    private void OnSignIn(ISignInController signInController)
    {
        this.signInController = signInController;
        this.signInController.OnSignIn();
    }

    private void SignedIn()
    {
        SetVisibleSignInButtons(false);
        SetVisibleTapToStartButton(true);
    }

    private void SetVisibleSignInButtons(bool isVisible)
    {
        buttonSignInWithGameCenter.gameObject.SetActive(isVisible);
        buttonSignInWithFacebook.gameObject.SetActive(isVisible);
        buttonSignInWithGoogle.gameObject.SetActive(isVisible);
        buttonSignInWithGuest.gameObject.SetActive(isVisible);
    }

    private void SetVisibleTapToStartButton(bool isVisible)
    {
        buttonTapToStart.gameObject.SetActive(isVisible);
    }

    private void SetVisibleLoadingProgress(bool isVisible)
    {

    }

    private void OnLoadingMainScene()
    {
        SetVisibleTapToStartButton(false);
        SetVisibleLoadingProgress(true);
    }
}

public enum SignInType
{
    GameCenter,
    Facebook,
    Google,
    Guest,
}