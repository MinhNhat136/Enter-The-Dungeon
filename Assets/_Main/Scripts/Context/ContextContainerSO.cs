using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

[CreateAssetMenu(menuName = "ContextContainer/Context")]
public class ContextContainerSO : ScriptableObject
{
    private IContext context;

    public IContext Context
    {
        get
        {
            if (context == null)
            {
                context = new Context();
            }
            return context;
        }
    }

    private void OnEnable()
    {
        context = new Context();
    }
}
