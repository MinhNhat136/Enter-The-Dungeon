using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Atomic.UI
{
    public class PolicyGameObject : MonoBehaviour
    {
        [SerializeField] 
        private UIPopup _policyPopup;

        [SerializeField]
        private ContextContainerSO _contextContainer;

        public void OnStart()
        {
            PolicyMini mini = new(_policyPopup) 
            {
                Context = _contextContainer.Context
            };
            mini.Initialize();
        }

    }

}
