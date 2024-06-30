using CBS.Scriptable;
using CBS.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CBS.UI
{
public class CurrencyPackItem : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Transform currencySlots;

    private CBSCurrencyPack Pack { get; set; }
    private CurrencyPrefabs Prefabs { get; set; }
    private ICurrency Currency { get; set; }
    private ICBSInAppPurchase CBSPurchase { get; set; }

    
    public string Id { get; set; }
    
    private void Awake()
    {
        Prefabs = CBSScriptable.Get<CurrencyPrefabs>();
        Currency = CBSModule.Get<CBSCurrencyModule>();
        CBSPurchase = CBSModule.Get<CBSInAppPurchaseModule>();
    }

    public void Initialize(CBSCurrencyPack pack)
    {
        Pack = pack;
        title.text = Pack.DisplayName;
        icon.sprite = Pack.GetSprite();
        icon.SetNativeSize();
        price.text = Pack.PriceTitle;
        description.text = Pack.Description;
        
        foreach (var currency in Pack.Currencies)
        {
            var slotPrefab = Prefabs.CurrencySlot;
            var slot = Instantiate(slotPrefab, currencySlots);
            slot.GetComponent<CurrencySlot>().Display(currency.Value);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
}
