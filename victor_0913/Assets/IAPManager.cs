using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPIDs
{
    public const string EPISODE_4 = "episode_4";
}

//IAPManager
public class IAPManager : BaseMonoSingleton<IAPManager>, IStoreListener
{
    GameSetting info;
    [SerializeField] public UnityEngine.GameObject[] epiButton;
    [SerializeField] public UnityEngine.GameObject[] inappButton;
    // Items list, configurable via inspector
    private List<CatalogItem> Catalog;

    [SerializeField] public GetItemHub getItem;

    // The Unity Purchasing system
    private static IStoreController m_StoreController;

    public void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        // Make PlayFab log in
        RefreshIAPItems();

        Debug.Log(inappButton[0].GetComponent<UILabel>().text);

    }
    // Bootstrap the whole thing
    public void Start()
    {
        

        //구매한 상품
        //epi4
        if(info.epiRock[3] == true)
        {
            Debug.Log("AA");
            epiButton[0].SetActive(true);
            inappButton[0].SetActive(false);
        }
    }

    public void RefreshIAPItems()
    {
        Debug.LogFormat("RefreshIAPItems");

        PlayFabClientAPI.GetCatalogItems
            (new GetCatalogItemsRequest(), result => {

                Debug.LogFormat("GetCatalogItems Result : {0}", result.ToJson());

                Catalog = result.Catalog;

                // Make UnityIAP initialize
                InitializePurchasing();
            }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    // This is invoked manually on Start to initialize UnityIAP
    public void InitializePurchasing()
    {
        Debug.LogFormat("{0}, IsInitialized({1})", MethodBase.GetCurrentMethod().Name, IsInitialized);

        // If IAP is already initialized, return gently
        if (IsInitialized) return;

        // Create a builder for IAP service
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

        // Register each item from the catalog
        foreach (var item in Catalog)
        {
            // 인앱 결제 상품만 추가
            if (item.Tags.Contains("iap"))
            {
                builder.AddProduct(item.ItemId, ProductType.Consumable);
            }
        }

        // Trigger IAP service initialization
        UnityPurchasing.Initialize(this, builder);
    }

    // We are initialized when StoreController and Extensions are set and we are logged in
    public bool IsInitialized
    {
        get
        {
            return m_StoreController != null && Catalog != null;
        }
    }

    // This is automatically invoked automatically when IAP service is initialized
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.LogFormat("{0}", MethodBase.GetCurrentMethod().Name);

        m_StoreController = controller;
    }

    // This is automatically invoked automatically when IAP service failed to initialized
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    // This is automatically invoked automatically when purchase failed
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    // This is invoked automatically when successful purchase is ready to be processed
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        // NOTE: this code does not account for purchases that were pending and are
        // delivered on application start.
        // Production code should account for such case:
        // More: https://docs.unity3d.com/ScriptReference/Purchasing.PurchaseProcessingResult.Pending.html

        if (!IsInitialized)
        {
            return PurchaseProcessingResult.Complete;
        }

        // Test edge case where product is unknown
        if (e.purchasedProduct == null)
        {
            Debug.LogWarning("Attempted to process purchase with unknown product. Ignoring");
            return PurchaseProcessingResult.Complete;
        }

        // Test edge case where purchase has no receipt
        if (string.IsNullOrEmpty(e.purchasedProduct.receipt))
        {
            Debug.LogWarning("Attempted to process purchase with no receipt: ignoring");
            return PurchaseProcessingResult.Complete;
        }

        Debug.Log("Processing transaction: " + e.purchasedProduct.transactionID);

        // Deserialize receipt
        var googleReceipt = GooglePurchase.FromJson(e.purchasedProduct.receipt);

        // Invoke receipt validation
        // This will not only validate a receipt, but will also grant player corresponding items
        // only if receipt is valid.
        PlayFabClientAPI.ValidateGooglePlayPurchase(new ValidateGooglePlayPurchaseRequest()
        {
            // Pass in currency code in ISO format
            CurrencyCode = e.purchasedProduct.metadata.isoCurrencyCode,
            // Convert and set Purchase price
            PurchasePrice = (uint)(e.purchasedProduct.metadata.localizedPrice * 100),
            // Pass in the receipt
            ReceiptJson = googleReceipt.PayloadData.json,
            // Pass in the signature
            Signature = googleReceipt.PayloadData.signature
        }
        , OnPurchasedSuccess
        , OnPurchasedFailure);

        return PurchaseProcessingResult.Complete;
    }

    private void OnPurchasedSuccess(ValidateGooglePlayPurchaseResult result)
    {
        Debug.Log("Validation successful!");

        foreach (var fulfillment in result.Fulfillments)
        { 
            foreach (var item in fulfillment.FulfilledItems)
            {
                switch (item.ItemId)
                {
                    case IAPIDs.EPISODE_4:
                        info.epiMaxMusicSocre[2] = 3000;
                        epiButton[0].SetActive(true);
                        inappButton[0].SetActive(false);
                        info.buttonControl = true;
                        getItem.BuyEpisode();
                        break;
                }
            }
        }
    }

    private void OnPurchasedFailure(PlayFabError error)
    {
        Debug.Log("Validation failed: " + error.GenerateErrorReport());
    }

    // This is invoked manually to initiate purchase
    public void BuyProductID(string productId)
    {
        if (info.buttonControl == false)
        {


            // If IAP service has not been initialized, fail hard
            if (!IsInitialized) throw new Exception("IAP Service is not initialized!");

            // Pass in the product id to initiate purchase
            m_StoreController.InitiatePurchase(productId);
        }
    }
}

// The following classes are used to deserialize JSON results provided by IAP Service
// Please, note that JSON fields are case-sensitive and should remain fields to support Unity Deserialization via JsonUtilities
public class JsonData
{
    // JSON Fields, ! Case-sensitive

    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
}

public class PayloadData
{
    public JsonData JsonData;

    // JSON Fields, ! Case-sensitive
    public string signature;
    public string json;

    public static PayloadData FromJson(string json)
    {
        var payload = JsonUtility.FromJson<PayloadData>(json);
        payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
        return payload;
    }
}

public class GooglePurchase
{
    public PayloadData PayloadData;

    // JSON Fields, ! Case-sensitive
    public string Store;
    public string TransactionID;
    public string Payload;

    public static GooglePurchase FromJson(string json)
    {
        var purchase = JsonUtility.FromJson<GooglePurchase>(json);
        purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
        return purchase;
    }
}