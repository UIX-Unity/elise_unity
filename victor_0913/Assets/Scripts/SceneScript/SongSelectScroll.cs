using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelectScroll : MonoBehaviour
{
    GameSetting info;

    public bool isDrag;
    public Vector3 oldPos;
    public Vector3 downPos;

    public float offsetY;

    public int index;
    public int indexTemp;

    public float test;

    public float percentage;
    public float destPercent;

    public bool grayTest;

    [SerializeField] UISprite selectSongImage;

    [SerializeField] UILabel episode;
    [SerializeField] UILabel episodeTitle;
    [SerializeField] UISprite gear;

    [SerializeField] UISprite episodeImage;

    [SerializeField] UISprite[] songListGear;
    [SerializeField] UILabel[] songListLabel;
    //음악추가
    //음악 에피소드 리스트
    [SerializeField] UnityEngine.GameObject[] epList;
    [SerializeField] UILabel[] songName;

    public bool eplistchStart;
    //전음악을 끌때 사용하는ㄴ 변수
    public int endIndex;

    [Header("색상정보")]
    [SerializeField] Color topColor;
    [SerializeField] Color bottomColor;

    [SerializeField] Color selectTop;
    [SerializeField] Color selectBottom;


    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem textParticle;

    [SerializeField] int[] yPosition;

    [SerializeField] UnityEngine.GameObject speedPanel;

    // Use this for initialization
    void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;

        //GetComponent<Animation>().Play("AlphaAni");

        songName[0].text = songListLabel[0].text;
        songName[1].text = info.episodes[info.episodeIndex].musics[0].composer;

        //songListSound[index].SetActive(true);
        //epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].SetActive(true);
        //songListSound[index].GetComponent<AudioSource>().volume = MainMusic.GetInstance.sound;
        //epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].GetComponent<AudioSource>().volume = MainMusic.GetInstance.sound;
    }
    void OnEnable()
    {
        //    index = 0;
        //    indexTemp = 0;
        //    endIndex = 0;
        //    gear.transform.Rotate(0f, 0f, 0f);
        epList[info.episodeIndex].SetActive(true);
        epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].SetActive(true);
        epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].GetComponent<AudioSource>().volume = MainMusic.GetInstance.sound;epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].SetActive(true);

    }
    void OnDisable()
    {
        epList[info.episodeIndex].GetComponent<EpSoundListControl>().EndMusic(endIndex);;
        //epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[endIndex].SetActive(false);
    }
    private void Update()
    {   
        if(speedPanel.activeSelf)
        {
            isDrag = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            oldPos = Input.mousePosition;
            downPos = oldPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            SetCenter();
        }

        if (isDrag)
        {
            float distance = (oldPos - Input.mousePosition).y;

            percentage = Mathf.Clamp01(percentage + distance / Screen.height);
            oldPos = Input.mousePosition;

            endIndex = indexTemp;
            indexTemp = GetCurrentIndex();
            info.musicIndex = indexTemp;


        }
        else
        {
            percentage = Mathf.Lerp(percentage, destPercent, Time.deltaTime * 3);
        }

        UpdateUI();
    }

    private int GetCurrentIndex()
    {

        return (int)Mathf.Round(Mathf.Lerp(0, info.episodes[info.episodeIndex].musics.Length - 1, percentage));
    }

    private void SetCenter()
    {
        destPercent = ((float)GetCurrentIndex()) / (info.episodes[info.episodeIndex].musics.Length - 1);
    }

    private void UpdateUI()
    {
        if (index != indexTemp)
        {
         
            epList[info.episodeIndex].GetComponent<EpSoundListControl>().StopMusic(endIndex);
            
         
            //epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[endIndex].SetActive(false);
            //songListSound[endIndex].SetActive(false);

            index = indexTemp;
            particle.Play();
            textParticle.transform.localPosition = new Vector3(textParticle.transform.localPosition.x, yPosition[index]);

            //추가된 소스 음악선택시 음악재생
            //StartCoroutine(Play());
            //songListSound[index].SetActive(true);
            songName[0].text = songListLabel[index].text;
            songName[1].text = info.episodes[info.episodeIndex].musics[index].composer;

            epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].SetActive(true);
            epList[info.episodeIndex].GetComponent<EpSoundListControl>().PlayMusic(index);
               
            //songListSound[index].GetComponent<AudioSource>().volume = MainMusic.GetInstance.sound;
            //epList[info.episodeIndex].GetComponent<EpSoundListControl>().songListSound[index].GetComponent<AudioSource>().volume = MainMusic.GetInstance.sound;


        }

        float angle = Mathf.Lerp(0f, 30 * info.episodes[info.episodeIndex].musics.Length - 1, percentage);
        gear.transform.localEulerAngles = new Vector3(0, 0, -angle);

        for (int i = 0; i < info.episodes[info.episodeIndex].musics.Length; i++)
        {
            songListGear[i].spriteName = "mini_off";
            songListLabel[i].gradientTop = topColor;
            songListLabel[i].gradientBottom = bottomColor;
        }


        //곡선택 일러스트
        for (int i = 0; i < info.episodes[info.episodeIndex].musics.Length; i++)
        {
            selectSongImage.spriteName = "illust_00"+ 1.ToString();
        }
       
        songListGear[indexTemp].spriteName = "mini_on";
        songListLabel[indexTemp].gradientTop = selectTop;
        songListLabel[indexTemp].gradientBottom = selectBottom;
    }
    //private IEnumerator PlayMusic()
    //{
    //    songListSound[index].GetComponent<AudioSource>().volume = 0f;
    //    songListSound[index].GetComponent<AudioSource>().Play();

    //    float timer = 0f;

    //    while (timer <= 1f)
    //    {
    //        timer += Time.deltaTime;

    //        songListSound[index].GetComponent<AudioSource>().volume = Mathf.Lerp(0f, MainMusic.GetInstance.sound, timer / 2);
    //        yield return null;
    //    }
    //}
}