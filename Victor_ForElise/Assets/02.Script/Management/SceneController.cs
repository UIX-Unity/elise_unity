using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESceneLocate
{
    Login,
    Title,
    Lobby,
    Ingame
}

public class SceneController : BaseMonoSingleton<SceneController>
{
    private ESceneLocate m_CurSceneLocate;
    public static ESceneLocate CurSceneLocate => Instance.m_CurSceneLocate;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    public void Initialize()
    {
        m_CurSceneLocate = ESceneLocate.Login;
    }

    public static void ChangeScene(ESceneLocate sceneLocate)
    {
        SceneManager.LoadScene(sceneLocate.ToString());
    }
}
