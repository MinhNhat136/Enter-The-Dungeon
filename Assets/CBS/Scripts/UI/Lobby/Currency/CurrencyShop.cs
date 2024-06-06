﻿using CBS.Models;
using CBS.Scriptable;
using System.Collections.Generic;
using UnityEngine;

namespace CBS.UI
{
    public class CurrencyShop : MonoBehaviour
    {
        [SerializeField]
        private Transform RootContent;

        private CurrencyPrefabs Prefabs { get; set; }
        private ICurrency Currencies { get; set; }

        private void Start()
        {
            Prefabs = CBSScriptable.Get<CurrencyPrefabs>();
            Currencies = CBSModule.Get<CBSCurrencyModule>();

            Currencies.GetCurrenciesPacks(OnPackGetted);
        }

        private void SpawnPacks(List<CBSCurrencyPack> packs)
        {
            int count = packs.Count;
            for (int i = 0; i < count; i++)
            {
                var pack = packs[i];
                var packPrefab = Prefabs.CurrencyPackItem;
                var packObj = Instantiate(packPrefab, RootContent);
                packObj.GetComponent<CurrencyPackItem>().Display(pack);
            }
        }

        public void OnCloseWindow()
        {
            gameObject.SetActive(false);
        }

        // events
        public void OnPackGetted(CBSGetCurrenciesPacksResult result)
        {
            if (result.IsSuccess)
            {
                SpawnPacks(result.Packs);
            }
        }
    }
}
