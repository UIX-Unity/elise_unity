using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : BaseMonoSingleton<TitleManager>
{
    [SerializeField]
    private CustomButton m_LobbyBtn;
    public CustomButton lobbyBtn => m_LobbyBtn;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(Initializing());
    }

    private IEnumerator Initializing()
    {
        m_LobbyBtn.onClick.AddListener(OnClickedBtn);
        yield break;
    }

    private void OnClickedBtn()
    {
        SceneController.ChangeScene(ESceneLocate.Lobby);
    }
}
