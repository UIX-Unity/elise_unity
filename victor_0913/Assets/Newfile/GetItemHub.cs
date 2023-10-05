using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;

public class GetItemHub : MonoBehaviour
{
    GameSetting info;
    [SerializeField] UnityEngine.GameObject item;
    public string[] text;
    public string[] text1;
    public string[] image;

    private bool button;
    private bool corutien;
    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        button = false;
        corutien = false;

        button = bool.Parse("true");
        for(int i=0;i<2;i++)
        {
            UnRockEpiSode(i);
        }
    }
    void Update()
    {
        if(corutien == true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                button = true;
                if (button == true)
                {
                    corutien = false;
                    StartCoroutine(EndObject());
                }
            }
            
        }
      
    }

    public void BuyEpisode()
    {
        if(info.epiRock[info.episodeIndex] == false)
        {
            //결제 시작
            //결제 성공
            info.epiRock[info.episodeIndex] = true;

            item.GetComponent<GetItem>().sprite.spriteName = image[info.episodeIndex-1];
            item.GetComponent<GetItem>().label.text = text[info.episodeIndex-1];
            item.GetComponent<GetItem>().label1.text = text1[info.episodeIndex-1];
            SetPlayFabEpisode();
            StartCoroutine(StartObject());
            //결제 실패

        }
    }
    private void UnRockEpiSode(int number)
    {
        if((info.epiUnRockScore[number] <= info.epiMaxMusicSocre[number]) && info.epiRock[number+1] == false)
        {
            info.epiRock[number+1] = true;
            item.GetComponent<GetItem>().sprite.spriteName = image[number];
            item.GetComponent<GetItem>().label.text = text[number];
            item.GetComponent<GetItem>().label1.text = text1[number];
            SetPlayFabEpisode();
            StartCoroutine(StartObject());
        }
    }
    IEnumerator StartObject()
    {
        info.buttonControl = true;
        yield return new WaitForSeconds(1f);
        item.SetActive(true);
        float color = item.GetComponent<UIPanel>().alpha = 0;
        //Color color = new Color(0, 0, 0, 1);
        float t = 0f;
        float time = 1f;
        while (color < 1)
        {
            t += Time.deltaTime / time;
            color = Mathf.Lerp(0, 1, t);

            item.GetComponent<UIPanel>().alpha = color;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        corutien = true;

    }
    IEnumerator EndObject()
    {
        yield return new WaitForSeconds(1f);

        float color = item.GetComponent<UIPanel>().alpha = 1;
        //Color color = new Color(0, 0, 0, 1);
        float t = 0f;
        float time = 1f;
        while (color > 0)
        {
            t += Time.deltaTime / time;
            color = Mathf.Lerp(1, 0, t);

            item.GetComponent<UIPanel>().alpha = color;
            yield return null;
        }

        item.SetActive(false);
        info.buttonControl = false;
    }
    // Update is called once per frame

   

    void SetPlayFabEpisode()
    {

        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { {"unRockEpiSode", SetEpisodeData() } } };
        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
  
        }
        , (error) => Debug.Log("데이터 저장 실패"));
    }
    //
    private string SetEpisodeData()
    {
        string tempText = "";
        for(int i=0;i<info.epiRock.Length;i++)
        {
            if(info.epiRock[i] == false)
            {
                tempText += 0;
            }
            else
            {
                tempText += 1;
            }
        }
        return tempText;
    }

    void GetPlayFabEpisode()
    {
        //데이터 불러오기
        var request = new GetUserDataRequest() { PlayFabId = info.myId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {

            foreach (var eachData in result.Data)
            {

              
            }
            //데이터 저장
        }, (error) => Debug.Log("데이터 불러오기실패"));
    }


//    //the actual method constructing the web request for validation
//    private void WaitForRequest(Product p)
//    {
//#pragma warning disable 0219
//        Hashtable hash = SIS.MiniJson.JsonDecode(p.receipt) as Hashtable;
//        string receipt = string.Empty;
//#pragma warning restore 0219

//#if UNITY_ANDROID
//        string signature = string.Empty;
//        switch (StandardPurchasingModule.Instance().appStore)
//        {
//            case AppStore.GooglePlay:
//                hash = SIS.MiniJson.JsonDecode(hash["Payload"] as string) as Hashtable;
//                receipt = hash["json"] as string;
//                signature = hash["signature"] as string;

//                ValidateGooglePlayPurchaseRequest gRequest = new ValidateGooglePlayPurchaseRequest()
//                {
//                    ReceiptJson = receipt,
//                    Signature = signature,
//                    CurrencyCode = p.metadata.isoCurrencyCode,
//                    PurchasePrice = (uint)(p.metadata.localizedPrice * 100)
//                };

//                Debug.LogError($"GP Purchase Request : {gRequest.ToJson()}");

//                PlayFabClientAPI.ValidateGooglePlayPurchase(gRequest, OnValidationResult, OnValidationError, p.definition.storeSpecificId);
//                break;

//            case AppStore.AmazonAppStore:
//                ValidateAmazonReceiptRequest aRequest = new ValidateAmazonReceiptRequest()
//                {
//                    ReceiptId = p.receipt,
//                    UserId = IAPManager.extensions.GetExtension<IAmazonExtensions>().amazonUserId,
//                    CurrencyCode = p.metadata.isoCurrencyCode,
//                    PurchasePrice = (int)(p.metadata.localizedPrice * 100)
//                };
//                PlayFabClientAPI.ValidateAmazonIAPReceipt(aRequest, OnValidationResult, OnValidationError, p.definition.storeSpecificId);
//                break;
//        }
//#elif UNITY_IOS || UNITY_TVOS
//         receipt = hash["Payload"] as string;

//            ValidateIOSReceiptRequest request = new ValidateIOSReceiptRequest()
//            {
//                ReceiptData = receipt,
//                CurrencyCode = p.metadata.isoCurrencyCode,
//                PurchasePrice = (int)(p.metadata.localizedPrice * 100)
//            };
//            PlayFabClientAPI.ValidateIOSReceipt(request, OnValidationResult, OnValidationError, p.definition.storeSpecificId);
//#endif
//    }
}
