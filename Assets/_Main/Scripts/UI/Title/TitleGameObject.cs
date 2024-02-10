using Atomic.UI;

using UnityEngine;

public class TitleGameObject : MonoBehaviour
{
    [SerializeField]
    private TitleView _titleView;
    // Start is called before the first frame update
    void Start()
    {
        TitleMini titleModule = new TitleMini(_titleView);
        titleModule.Initialize();
    }
}
