using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void EnterThemeSelect()
    {
        SceneManager.LoadScene(2);
    }

    public void EnterSongSelect()
    {
        SceneManager.LoadScene(3);
    }

    public void EnterGame()
    {
        SceneManager.LoadScene(4);
    }
}