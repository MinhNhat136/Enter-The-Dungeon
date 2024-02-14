using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Atomic.UI
{
    public class NetworkGameObject : MonoBehaviour
    {
        [SerializeField] private UIPopup _popup;
        // Start is called before the first frame update
        void Start()
        {
            NetworkMini module = new(_popup);
            module.Initialize();
        }

    }
}

