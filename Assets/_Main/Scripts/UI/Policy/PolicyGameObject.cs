using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Atomic.UI
{
    public class PolicyGameObject : MonoBehaviour
    {
        [SerializeField] private UIPopup _policyPopup;
        // Start is called before the first frame update


        public void OnStart()
        {
            PolicyMini mini = new(_policyPopup);
            mini.Initialize();
        }



    }

}
