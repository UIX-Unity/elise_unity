using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;
public class startMainScript : MonoBehaviour
{
    GameSetting info;

    //전기 경험치 관련
    [SerializeField] UILabel expLabel;
    [SerializeField] UISprite expSprite;

    [SerializeField] Transform particlePos;
    [SerializeField] ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        FadePanel.GetInstance.ScreenFade(true, 1.5f);

        StartCoroutine(ExpUp());
        info.buttonControl = false;
    }

    //데이터 불러오기
    void OnLoginGetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = info.myId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
               

            }
        }, (error) => Debug.Log("데이터 불러오기실패"));
    }

    void ExpUpdate()
    {
        info.expSample = Mathf.Lerp(info.expSample, info.exp, 1f);
        expLabel.text = Mathf.Max(0, info.expSample).ToString("N0") + "%";
        expSprite.fillAmount = Mathf.Max(0, info.expSample) / 100;
    }

    IEnumerator ExpUp()
    {
        expLabel.text = Mathf.Max(0, info.expSample).ToString("N0") + "%";
        expSprite.fillAmount = Mathf.Max(0, info.expSample) / 100;

        yield return  new WaitForSeconds(1.5f);

        float t = 0f+ info.expSample;
        float t1 = 0f;

        float time = 0.04f;
        while (info.expSample < info.exp)
        {
            t += Time.deltaTime / time;
            t1 += Time.deltaTime;
            particlePos.localPosition = new Vector2(-1852 + ((1852f / 100) * (expSprite.fillAmount * 100)), particlePos.position.y);
            Debug.Log(t1);
            if(t1 >0.1f&& !(info.expSample == info.exp))
            {
                particle.Play();
                t1 = 0;
            }
            

            info.expSample =Mathf.Max(0,Mathf.Min(100.00f, Mathf.Clamp(t ,info.expSample, info.exp)));


            expLabel.text = Mathf.Max(0, info.expSample).ToString("N0") + "%";
            expSprite.fillAmount = Mathf.Max(0, info.expSample) / 100;
            yield return null;
        }
        particlePos.localPosition = new Vector2(-1852 + ((1852f / 100) * (expSprite.fillAmount * 100)), particlePos.position.y);
       
    }

    public void SetStatExp()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "exp", info.exp.ToString("N2") } } };
        PlayFabClientAPI.UpdateUserData(request, (result) => Debug.Log("데이터 저장 성공"), (error) => Debug.Log("데이터 저장 실패"));

    }
    public void expInti()
    {
        info.exp = 0;
        info.expSample = 0;
        expLabel.text = "0";
        expSprite.fillAmount = 0;
        SetStatExp();
    }
    public void SetPlayFabItem()
    {

        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "unRockItem", SetItem() } } };
        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {

        }
        , (error) => Debug.Log("데이터 저장 실패"));
    }
    //item플레이펩에 저장
    private string SetItem()
    {
        string tempText = "";
        for (int i = 0; i < info.itemRock.Length; i++)
        {
            if (info.itemRock[i] == false)
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
}
