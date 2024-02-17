using Atomic.UI;
using UnityEngine;

public class AppTitleGameObject : MonoBehaviour
{
    [SerializeField]
    private AppTitleView _titleView;

    [SerializeField]
    private ContextContainerSO contextContainer;

    public void OnStart()
    {
        AppTitleMini titleMini = new(_titleView)
        {
            Context = contextContainer.Context
        };

        titleMini.Initialize();
    }
}
