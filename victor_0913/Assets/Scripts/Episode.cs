using UnityEngine;


[System.Serializable]
public class Music
{
    public string composer;
    public string name;
    public enum MusicLevel
    {
        EASY = 0,
        NORMAL,
        HARD
    }

    [HideInInspector]
    public string backgroundPath { get { return background.name; } }
    [SerializeField]
    Texture background;
    public TextAsset[] levelBMS = new TextAsset[3];
    public float highlightTime;

    public AudioClip music;

    //난이도별 처음 경험치 획득체크
    public bool[] firstPlay;
    //곡의 최고 진행도
    public float[] maxMusicScore;
    public float[] requireMusicScore;
    public TextAsset this[MusicLevel lv]
    {
        get
        {
            return levelBMS[(int)lv];
        }
    }
}

[CreateAssetMenu(fileName = "NewEpisode", menuName = "Create New Episode")]
public class Episode : ScriptableObject
{
    public string episodeName;
    public UIAtlas episodeAtlas;
    [HideInInspector]
    public string episodeImgPath { get { return episodeImg.name; } }
    [SerializeField]
    UISprite episodeImg;

    public string episodeIcon;

    public Music[] musics;
}