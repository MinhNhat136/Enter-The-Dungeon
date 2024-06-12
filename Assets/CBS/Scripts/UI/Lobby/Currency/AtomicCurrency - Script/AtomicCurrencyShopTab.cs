using TMPro;
using UnityEngine;

namespace CBS.UI
{
    public class AtomicCurrencyShopTab : MonoBehaviour
    {
        private static readonly Color ColorOnShow = new(246, 225, 156, 255);
        private static readonly Color ColorOnHide = new(190, 181, 182, 255);

        [SerializeField] private string tabId;
        [SerializeField] private TextMeshProUGUI tapText;
        [SerializeField] private GameObject tapHighLight;
        
        private void Awake()
        {
            tapText.color = ColorOnHide;
            tapHighLight.SetActive(true);
        }

        public void ShowTab(string id)
        {
            if (tabId != id)
            {
                tapText.color = ColorOnHide;
                tapHighLight.SetActive(false);
                return;
            }
            tapText.color = ColorOnShow;
            tapHighLight.SetActive(true);
        }
    }
}