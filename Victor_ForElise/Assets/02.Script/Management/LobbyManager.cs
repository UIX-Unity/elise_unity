using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : BaseMonoSingleton<LobbyManager>
{
    [SerializeField]
    private CustomButton m_IngameBtn;
    public CustomButton ingameBtn => m_IngameBtn;

    [SerializeField]
    private string musicID = "Test";

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(Initializing());
    }

    private IEnumerator Initializing()
    {
        m_IngameBtn.onClick.AddListener(OnClickedBtn);
        yield break;
    }

    private void OnClickedBtn()
    {
        AppManager.SetMusicID(musicID);
        SceneController.ChangeScene(ESceneLocate.Ingame);
    }
}
