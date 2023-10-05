//using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongScene : MonoBehaviour
{
    private GameSetting info;

    [SerializeField] SongListen song;
    [SerializeField] UISprite diff;
    [SerializeField] Animator ani;
    
    [SerializeField] UISlider slider;
    [SerializeField] UILabel speedLabel;

    //[SerializeField] SkeletonAnimation bone;

    [SerializeField] UISprite gear;


    [SerializeField] Animator customization;
    [SerializeField] UILabel diffLabel;

    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        info.lv = 0;;
        float sliderValue = Mathf.InverseLerp(0.5f, 9f, info.speed);
        slider.value = sliderValue;
    }

    public void SliderValueChange()
    {
        info.speed = Mathf.Lerp(0.5f, 9f, slider.value);
        speedLabel.text = string.Format("{0:0.0}", info.speed);
        //bone.timeScale = info.speed;
        
    }

    public void OpenCustomization()
    {
        customization.gameObject.SetActive(true);
    }

    public void CloseCustomization()
    {
        StartCoroutine(Close());
    }

    private IEnumerator Close()
    {
        customization.Play("ThemeClose");
        yield return new WaitForSeconds(0.6f);
        customization.gameObject.SetActive(false);
    }

    public void SliderPlus()
    {
        slider.value += 0.05f;
    }
    
    public void SliderMinus()
    {
        slider.value -= 0.05f;
    }

    public void Update()
    {
        gear.transform.Rotate(Vector3.forward * Time.deltaTime * info.speed * 50);
    }

    // Use this for initialization
    void Start()
    {
        FadePanel.GetInstance.ScreenFade(true, 1.5f);
        StartCoroutine(Play());
    }

    public void EnterGame()
    {
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        FadePanel.GetInstance.ScreenFade(false, 1.5f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(4);
    }

    private IEnumerator Play()
    {
        yield return new WaitForSeconds(1.2f);
        song.PlayMusic();
    }

    public void EnterLobby()
    {
        StartCoroutine(Lobby());
    }

    private IEnumerator Lobby()
    {
        song.PauseMusic();
        FadePanel.GetInstance.ScreenFade(false, 1);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }

    public void EnterStory()
    {
        StartCoroutine(Story());
    }

    private IEnumerator Story()
    {
        song.PauseMusic();
        FadePanel.GetInstance.ScreenFade(false, 1);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(5);
    }

    public void UpDifficult()
    {
        if(info.lv < 2)
        {
            info.lv++;
        }
        else if(info.lv == 2)
        {
            info.lv = 0;
        }

        diff.spriteName = info.lv.ToString();
        switch(info.lv)
        {
            case 0:
                diffLabel.text = "Easy";
                break;

            case 1:
                diffLabel.text = "Normal";
                break;

            case 2:
                diffLabel.text = "Hard";
                break;
        }
        ani.Play("SongGear");
    }
}