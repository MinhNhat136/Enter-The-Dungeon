using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.UI
{
    public class NetworkGameObject : MonoBehaviour
    {
        [SerializeField] 
        private UIPopup _popup;

        public void OnStart()
        {
            NetworkMini module = new(_popup, new Context());
            module.Initialize();
        }

    }
}

