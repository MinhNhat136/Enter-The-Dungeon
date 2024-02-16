using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

public class TitleGameObject : ContextContainer
{
    [SerializeField]
    private AppTitleView _titleView;

    public void OnStart()
    {
        TitleMini titleMini = new TitleMini(_titleView);
        titleMini.Initialize();
        _context = titleMini.Context;
    }
}
