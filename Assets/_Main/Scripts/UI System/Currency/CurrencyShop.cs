using System.Collections.Generic;
using CBS.Models;
using CBS.Scriptable;
using UnityEngine;

namespace CBS.UI
{
    public class CurrencyShop : MonoBehaviour
    {
        [SerializeField]
        private Transform RootContent;

        [SerializeField] private CurrencyShopTab[] tabs;

        private CurrencyPrefabs Prefabs { get; set; }
        private ICurrency Currencies { get; set; }
        private List<CBSCurrencyPack> _packs;
        private List<CurrencyPackItem> _currencyPackItems = new(64);
        private string _currencyTypeShowed;

        private void Start()
        {
            Prefabs = CBSScriptable.Get<CurrencyPrefabs>();
            Currencies = CBSModule.Get<CBSCurrencyModule>();
            Currencies.GetCurrenciesPacks(OnPackGot);
        }

        private void SpawnPacks()
        {
            var count = _packs.Count;
            for (int i = 0; i < count; i++)
            {
                var pack = _packs[i];
                var packPrefab = Prefabs.CurrencyPackItem;
                var packObj = Instantiate(packPrefab, RootContent);
                var currencyPack= packObj.GetComponent<CurrencyPackItem>();
                currencyPack.Initialize(pack);
                currencyPack.Id = pack.ID;
                currencyPack.Hide();
                _currencyPackItems.Add(currencyPack);
            }
        }

        public void ShowPacks(string id)
        {
            foreach (var item in _currencyPackItems)
            {
                item.Hide();
            }
            var currencyPacks = _currencyPackItems.FindAll(currencyPack => currencyPack.Id.Contains(id));
            var count = currencyPacks.Count;
            for (var i = 0; i < count; i++)
            {
                currencyPacks[i].Show();
            }

            foreach (var tab in tabs)
            {
                tab.ShowTab(id);
            }
        }

        public void OnCloseWindow()
        {
            gameObject.SetActive(false);
        }

        // events
        private void OnPackGot(CBSGetCurrenciesPacksResult result)
        {
            if (!result.IsSuccess) 
                return;
            _packs = result.Packs;
            SpawnPacks();
        }
    }
    
}
