using CBS.Models;
using CBS.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CBS.UI
{
    public class CurrencySlot : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI valueTitle;
        [SerializeField]
        private Image iconImage;

        private CBSCurrency Currency { get; set; }
        private CurrencyPrefabs Prefabs { get; set; }

        private void Awake()
        {
            Prefabs = CBSScriptable.Get<CurrencyPrefabs>();
        }

        public void Display(CBSCurrency currency)
        {
            Currency = currency;
            // draw ui
            valueTitle.text = Currency.ToString();
            iconImage.sprite = Currency.GetSprite();
        }

        public void UpdateCurrency(CBSCurrency currency)
        {
            if (currency.Code == Currency.Code)
            {
                Currency = currency;
                valueTitle.text = Currency.ToString();
            }
        }
    }
}
