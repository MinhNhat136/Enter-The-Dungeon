using System.Collections.Generic;
using System.Linq;
using CBS.Models;
using CBS.Scriptable;
using UnityEngine;

namespace CBS.UI
{
    public class CurrencyPanel : MonoBehaviour
    {
        [SerializeField]
        private CurrencyDisplayOptions displayOption;
        [SerializeField]
        private string[] selectedCurrencies;

        private CurrencyPrefabs Prefabs { get; set; }
        private AuthData AuthData { get; set; }
        private ICurrency Currencies { get; set; }

        private readonly List<CurrencyItem> _currenciesUI = new();

        private void Start()
        {
            Prefabs = CBSScriptable.Get<CurrencyPrefabs>();
            AuthData = CBSScriptable.Get<AuthData>();
            Currencies = CBSModule.Get<CBSCurrencyModule>();

            Currencies.OnCurrencyUpdated += OnCurrencyUpdated;

            // check cache currencies
            if (AuthData.PreloadCurrency)
            {
                var correctedCurrencies = GetSelectedCurrencies(Currencies.CacheCurrencies);
                SpawnItems(correctedCurrencies);
            }
            else
            {
                Currencies.GetProfileCurrencies(OnCurrenciesGet);
            }
        }

        private void OnDestroy()
        {
            Currencies.OnCurrencyUpdated -= OnCurrencyUpdated;
        }

        private void SpawnItems(Dictionary<string, CBSCurrency> currencies)
        {
            var itemPrefab = Prefabs.CurrencyItem;

            foreach (var currencyPair in currencies)
            {
                var currency = currencyPair.Value;
                var item = Instantiate(itemPrefab, transform);
                var itemUI = item.GetComponent<CurrencyItem>();
                itemUI.Display(currency);
                itemUI.CurrencyID = currencyPair.Value.Code;
                _currenciesUI.Add(itemUI);
            }
        }

        // events
        private void OnCurrenciesGet(CBSGetCurrenciesResult result)
        {
            if (!result.IsSuccess) return;
            var correctedCurrencies = GetSelectedCurrencies(result.Currencies);
            SpawnItems(correctedCurrencies);
        }

        private void OnCurrencyUpdated(CBSCurrency currency)
        {
            foreach (var ui in _currenciesUI)
            {
                ui.UpdateCurrency(currency);
            }
        }

        private Dictionary<string, CBSCurrency> GetSelectedCurrencies(Dictionary<string, CBSCurrency> inputCurrencies)
        {
            if (displayOption == CurrencyDisplayOptions.SELECTED)
            {
                return inputCurrencies.Where(x => selectedCurrencies.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            }
            return inputCurrencies;
        }
    }
}