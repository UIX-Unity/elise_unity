using UnityEngine;

using UnityEngine.Purchasing;

using System;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

using PlayFab;
using PlayFab.ClientModels;
public class PlayfabLogin : MonoBehaviour
{
    GameSetting info;
    bool googleLogin;
    string myID;



    [SerializeField] StartScene start;

    [SerializeField] UnityEngine.GameObject LoginError;

    // Start is called before the first frame update
    public void Start()
    {
        googleLogin = false;
        info = Resources.Load("GameSetting") as GameSetting;
        //구글 연동
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)

            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        //로그인 방식 2020-02-07 새로운 테스트 버전
        //Social.localUser.Authenticate((success)=>
        //{
        //    if(success)
        //    {
        //        PlayGamesPlatform.Instance.GetServerAuthCode((code,suthToken))
        //    }
        //}
        //) 
        //{

        //}

        //구글 로그인 2020-02-06 전버전
        OnLoginSuccessGoogle();


        info.buttonControl = false;

        //if (!string.IsNullOrEmpty(PlayFabSettings.TitleId))
        //{
        //    var request = new LoginWithCustomIDRequest
        //    {
        //        CustomId = SystemInfo.deviceUniqueIdentifier,
        //        CreateAccount = true,
        //    };

        //    PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFail);

        //    info = Resources.Load("GameSetting") as GameSetting;

        //    info.expSample = info.exp;
        //    Debug.Log("Login");
        //}
    }

    private void OnLoginSuccessGoogle()
    {

        Social.localUser.Authenticate((bool success) =>
        {
            var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            Debug.Log("Server Auth Code: " + serverAuthCode);

            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                ServerAuthCode = serverAuthCode,
                CreateAccount = true

            }, (result) =>
            {
                Debug.Log(result.PlayFabId);

                googleLogin = true;
                start.isDraw = true;
                info.loginCheck = true;
                myID = result.PlayFabId;
                info.myId = myID;
                //데이터 불러오기
                OnLoginGetData();
                GetMaxMusicScore();
                GetPlayFabEpisode();
                GetPlayFabItem();

                //로그인 정보
                Hashtable hash;


                //곡진행률 받기
                for (int i = 0; i < info.episodes.Length; i++)
                {
                    SetEpiData(i);
                }

            }, OnLoginFail);

        });



        //    Social.localUser.Authenticate((bool success) =>
        //{
        //    Debug.Log("구글 연동 성공");
        //    var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();


        //    PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
        //    {
        //        TitleId = PlayFabSettings.TitleId,
        //        ServerAuthCode = serverAuthCode,
        //        CreateAccount = true
        //    }, (result) =>
        //    {
        //        Debug.Log(result.PlayFabId);

        //        info = Resources.Load("GameSetting") as GameSetting;

        //        info.expSample = info.exp;

        //    }, OnLoginFail);



        //    googleLogin = true;
        //});
    }

    //곡 진행률
    public void SetEpiData(int episodeIndex)
    {
        //데이터 불러오기
        var request = new GetUserDataRequest() { PlayFabId = info.myId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {

            foreach (var eachData in result.Data)
            {

                if (Equals("episode" + "0" + episodeIndex.ToString(), eachData))
                {
                    //에피소드 진행률 저장
                    info.epiMaxMusicSocre[episodeIndex] = float.Parse(eachData.Value.Value);
                }

            }
            //데이터 저장
        }, (error) => Debug.Log("데이터 불러오기실패"));


    }
    void OnLoginGetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
                if (eachData.Key == "exp")
                {
                    info.expSample = float.Parse(eachData.Value.Value);
                    info.exp = float.Parse(eachData.Value.Value);
                }

            }
        }, (error) => Debug.Log("데이터 불러오기실패"));
    }

    void GetMaxMusicScore()
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            int epi = 0;
            int song = 0;
            int lv = 0;
            int index = 0;
            foreach (var eachData in result.Data)
            {
                for (int i = 0; i < result.Data.Count; i++)
                {
                    if (lv == 3)
                    {
                        lv = 0;
                        song++;
                    }
                    if (song == 5)
                    {
                        song = 0;
                        epi++;
                    }
                    if (eachData.Key == "music" + "0" + epi.ToString() + "0" + song.ToString() + "0" + lv.ToString())
                    {
                        info.epiMaxMusicSocre[index] = float.Parse(eachData.Value.Value);
                    }

                    lv++;
                    index++;
                }
                lv = 0;
                song = 0;
                epi = 0;
                index = 0;

            }
        }, (error) => Debug.Log("데이터 불러오기실패"));
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab 로그인 성공");
    }


    void OnLoginFail(PlayFabError error)
    {

        Debug.LogError("PlayFab 로그인 에러!");
        Debug.LogError(error.GenerateErrorReport());
        //로그인이 안되었을경우 게스트로그인

        if (googleLogin == false)
        {
            Debug.Log("게스트 계정 로그인");

            if (!string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                var request = new LoginWithCustomIDRequest
                {
                    CustomId = SystemInfo.deviceUniqueIdentifier,

                    CreateAccount = true,
                };

                PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoignerror);


                info.loginCheck = false;
                Debug.Log("Login");
                StartCoroutine(errorLoginStart());
                Debug.Log("titleiD " + PlayFabSettings.TitleId);
                Debug.Log("myID " + myID);


                //OnLoginGetData();
            }
        }
    }


    void OnLoignerror(PlayFabError error)
    {

        Debug.LogError("PlayFab 로그인 에러!");
        Debug.LogError(error.GenerateErrorReport());
    }
    IEnumerator errorLoginStart()
    {
        yield return new WaitForSeconds(3f);
        LoginError.SetActive(true);
        float color = LoginError.GetComponent<UIPanel>().alpha = 0;
        //Color color = new Color(0, 0, 0, 1);
        float t = 0f;
        float time = 1f;
        while (color < 1)
        {
            t += Time.deltaTime / time;
            color = Mathf.Lerp(0, 1, t);

            LoginError.GetComponent<UIPanel>().alpha = color;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(errorLoginEnd());
    }

    IEnumerator errorLoginEnd()
    {
        yield return new WaitForSeconds(1.5f);

        float color = LoginError.GetComponent<UIPanel>().alpha = 1;
        //Color color = new Color(0, 0, 0, 1);
        float t = 0f;
        float time = 1f;
        while (color > 0)
        {
            t += Time.deltaTime / time;
            color = Mathf.Lerp(1, 0, t);

            LoginError.GetComponent<UIPanel>().alpha = color;
            yield return null;
        }

        LoginError.SetActive(false);
        start.isDraw = true;
    }
    //언락된 아이템 데이터를 가져옵니다.
    void GetPlayFabItem()
    {
        //데이터 불러오기
        var request = new GetUserDataRequest() { PlayFabId = info.myId };

        PlayFabClientAPI.GetUserData(request, (result) =>
        {

            foreach (var eachData in result.Data)
            {
                if (eachData.Key == "unRockItem")
                {
                    for (int i = 0; i < info.itemRock.Length; i++)
                    {
                        if (eachData.Value.Value[i] == 0)
                        {
                            info.itemRock[i] = false;
                        }
                        else
                        {
                            info.itemRock[i] = true;
                        }

                    }
                    break;
                }

            }
            //데이터 저장
        }, (error) => Debug.Log("데이터 불러오기실패"));
    }
    //언락된 에피소드 데이터를 가져옵니다.
    void GetPlayFabEpisode()
    {
        //데이터 불러오기
        var request = new GetUserDataRequest() { PlayFabId = info.myId };

        PlayFabClientAPI.GetUserData(request, (result) =>
        {

            foreach (var eachData in result.Data)
            {
                if (eachData.Key == "unRockEpiSode")
                {
                    for (int i = 0; i < info.epiRock.Length; i++)
                    {
                        if (eachData.Value.Value[i] == 0)
                        {
                            info.epiRock[i] = false;
                        }
                        else
                        {
                            info.epiRock[i] = true;
                        }

                    }
                    break;
                }

            }
            //데이터 저장
        }, (error) => Debug.Log("데이터 불러오기실패"));
    }
}
