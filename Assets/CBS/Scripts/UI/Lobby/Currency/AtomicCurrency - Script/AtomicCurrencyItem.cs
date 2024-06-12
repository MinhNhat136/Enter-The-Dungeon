using CBS.Models;
using CBS.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CBS.UI
{
    public class AtomicCurrencyItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valueTitle;
        [SerializeField] private Image iconImage;
        [SerializeField] private CurrencyRechargeTimer timer;

        public string CurrencyID { get; set; }
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
            iconImage.SetNativeSize();
            CheckTimer();
        }

        public void UpdateCurrency(CBSCurrency currency)
        {
            if (currency.Code == Currency.Code)
            {
                Currency = currency;
                valueTitle.text = Currency.ToString();
                CheckTimer();
            }
        }

        private void CheckTimer()
        {
            if (!Currency.Rechargeable)
            {
                timer.gameObject.SetActive(false);
                return;
            }

            if (!Currency.IsMaxRecharge())
            {
                timer.gameObject.SetActive(true);
                timer.StartTimer(Currency);
            }
            else
            {
                timer.gameObject.SetActive(false);
            }
        }

        // button click
        public void ShowCurrenciesWindow()
        {
            var shopPrefab = Prefabs.CurrenciesPacks;
            var currencyShop = UIView.ShowWindow(shopPrefab).GetComponent<AtomicCurrencyShop>();
            currencyShop.ShowPacks(CurrencyID);
        }
    }
}