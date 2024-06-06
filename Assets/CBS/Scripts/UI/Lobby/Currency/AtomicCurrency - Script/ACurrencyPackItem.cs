using CBS;
using CBS.Scriptable;
using CBS.UI;
using CBS.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ACurrencyPackItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Transform currencySlots;

    private CBSCurrencyPack Pack { get; set; }
    private CurrencyPrefabs Prefabs { get; set; }
    private ICurrency Currency { get; set; }
    private ICBSInAppPurchase CBSPurchase { get; set; }

    private void Awake()
    {
        Prefabs = CBSScriptable.Get<CurrencyPrefabs>();
        Currency = CBSModule.Get<CBSCurrencyModule>();
        CBSPurchase = CBSModule.Get<CBSInAppPurchaseModule>();
    }

    public void Display(CBSCurrencyPack pack)
    {
        Pack = pack;
        icon.sprite = Pack.GetSprite();
        price.text = Pack.PriceTitle;

        foreach (var currency in Pack.Currencies)
        {
            var slotPrefab = Prefabs.CurrencySlot;
            var slot = Instantiate(slotPrefab, currencySlots);
            slot.GetComponent<CurrencySlot>().Display(currency.Value);
        }
    }

    // button events
    public void PurchasePack()
    {
        Currency.PurchasePackWithRealMoney(Pack.ID, result =>
        {
            if (!result.IsSuccess)
            {
                new PopupViewer().ShowFabError(result.Error);
            }
            else
            {
                new PopupViewer().ShowSimplePopup(new PopupRequest
                {
                    Title = ItemTXTHandler.PurchaseTitle,
                    Body = ItemTXTHandler.PurchaseBody
                });
            }
        });
    }
}