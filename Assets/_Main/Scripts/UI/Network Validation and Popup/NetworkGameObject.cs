using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    public class NetworkGameObject : MonoBehaviour
    {
        [SerializeField] 
        private UIPopup _popup;

        public void Start()
        {
            NetworkMini module = new(_popup, new Context());
            module.Initialize();
        }

    }
}

