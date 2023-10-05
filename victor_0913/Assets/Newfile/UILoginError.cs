using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
public class UILoginError : MonoBehaviour
{
    GameSetting info;

    [SerializeField] UnityEngine.GameObject error;
    bool googleLogin;

    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        googleLogin = false;
        if (info.loginCheck == false)
        {
            error.SetActive(true);
        }
        else
        {
            error.SetActive(false);
        }
    }

    public void GoogleLogin()
    {
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
        Social.localUser.Authenticate((bool success) =>
        {
            var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            Debug.Log("Server Auth Code: " + serverAuthCode);

            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                ServerAuthCode = serverAuthCode,
                CreateAccount = true
            }, (result) => {
                Debug.Log(result.PlayFabId);

                info.expSample = info.exp;
                googleLogin = true;
                info.loginCheck = true;
                StartCoroutine(StartSceneClose());
            }, OnLoginFail);

        });

      
    }
    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab 로그인 성공");
    }
    void OnLoginFail(PlayFabError error)
    {
        Debug.LogError("PlayFab 로그인 에러!");
        Debug.LogError(error.GenerateErrorReport());
    }

    IEnumerator StartSceneClose()
    {
        FadePanel.GetInstance.ScreenFade(false, 1);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
