using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class AppManager : BaseMonoSingleton<AppManager>
{
    //private GameSettingManager m_GameSettingManger;

    // 음악을 선택하면 ID 값 여기에 캐싱
    private string MusicID;
    public static string GetMusicID => Instance.MusicID;
    public static void SetMusicID(string id) { Instance.MusicID = id; } 


    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Initializing());
    }

    private IEnumerator Initializing()
    {
        DataManager.CreateInstance();
        ResourceManager.CreateInstance();

        //m_GameSettingManger = GameSettingManager.CreateInstance();
        yield return null;

        SceneController.ChangeScene(ESceneLocate.Title);
        yield break;
    }

    private void Release()
    {
        DataManager.DestroyInstance();
        ResourceManager.DestroyInstance();
    }

    private void OnApplicationQuit()
    {
        Release();
    }
}
