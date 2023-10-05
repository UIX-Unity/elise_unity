using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : BaseMonoSingleton<IngameManager>
{
    [SerializeField]
    private MusicInputSystem m_MusicInputSystem;
    public static MusicInputSystem GetMusicInputSystem => Instance.m_MusicInputSystem;

    [SerializeField]
    private MusicNoteChecker m_MusicNoteChecker;
    public static MusicNoteChecker GetMusicNoteChecker => Instance.m_MusicNoteChecker;

    [SerializeField]
    private CustomButton m_RestartBtn;
    public static CustomButton restartBtn => Instance.m_RestartBtn;

    protected override void Awake()
    {
        StartCoroutine(Initializing());
    }

    private IEnumerator Initializing()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        GameManager.Instance.Initialize();
        GameManager.Instance.LoadData(AppManager.GetMusicID);

        m_MusicInputSystem.Initialize();
        m_MusicNoteChecker.Initialize();

        m_MusicInputSystem.inputAction += m_MusicNoteChecker.OnInput;
        m_RestartBtn.onClick.AddListener(Restart);

        GameManager.Instance.PlayGame();

        yield break;
    }

    public void Restart()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        SceneController.ChangeScene(ESceneLocate.Ingame);
    }
}
