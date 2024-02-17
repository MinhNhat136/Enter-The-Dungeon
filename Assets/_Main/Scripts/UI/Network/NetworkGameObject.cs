using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Atomic.UI
{
    public class NetworkGameObject : MonoBehaviour
    {
        [SerializeField] 
        private UIPopup _popup;

        [SerializeField]
        private ContextContainerSO _contextContainer;

        public void OnStart()
        {
            NetworkMini module = new(_popup)
            {
                Context = _contextContainer.Context
            };
            module.Initialize();
        }

    }
}

