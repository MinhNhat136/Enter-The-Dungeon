using Atomic.UI;

using UnityEngine;

public class TitleGameObject : MonoBehaviour
{
    [SerializeField]
    private TitleView _titleView;
    // Start is called before the first frame update
    void Start()
    {
        TitleModule titleModule = new TitleModule(_titleView);
        titleModule.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
