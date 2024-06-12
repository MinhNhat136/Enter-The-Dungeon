using System;
using TMPro;
using UnityEngine;

namespace CBS.UI
{
    public class SimplePopup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;
        
        [SerializeField]
        private TextMeshProUGUI body;

        private Action CurrentAction { get; set; }

        public void Setup(PopupRequest request)
        {
            Clear();
            title.text = request.Title;
            body.text = request.Body;
            CurrentAction = request.OnOkAction;
        }

        private void Clear()
        {
            title.text = string.Empty;
            body.text = string.Empty;
            CurrentAction = null;
        }

        // button event
        public void OnOk()
        {
            CurrentAction?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public struct PopupRequest
    {
        public string Title;
        public string Body;
        public Action OnOkAction;
    }
}
