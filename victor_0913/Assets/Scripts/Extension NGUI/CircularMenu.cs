using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMenu : MonoBehaviour
{
    GameSetting info;

    [SerializeField] UISprite foreground;
    [SerializeField] UISprite background;
    [SerializeField] string[] sprites;

    [SerializeField] UISprite gear;

    [SerializeField] UILabel title;

    [Range(0f, 1f)][SerializeField] float percentage;
    public float Percentage { get { return percentage; } set { percentage = destPercent = value; } }
    float destPercent;

    List<UILabel> subItems = new List<UILabel>();

    [SerializeField] float radius = 0.9f;
    float angleOffset = -34f;

    [SerializeField] float sizeAngle = 90f;
    [SerializeField] float colorAngle = 90f;

    [SerializeField] Color hideColor;
    [SerializeField] Color mainColor;

    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;

        background.atlas = info.selectedEpisode.episodeAtlas;
        foreground.atlas = info.selectedEpisode.episodeAtlas;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = info.selectedEpisode.musics[i].backgroundPath;
        }
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = info.selectedEpisode.musics[i].backgroundPath;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            subItems.Add(transform.GetChild(i).GetComponent<UILabel>());
        }

        for (int i = 0; i < subItems.Count; i++)
        {
            subItems[i].text = info.selectedEpisode.musics[i].name;
        }
    }

    public int GetCurrentIndex()
    {
        return (int)Mathf.Round(Mathf.Lerp(0, subItems.Count - 1, percentage));
    }

    public void SetCenter()
    {
        destPercent = ((float)GetCurrentIndex()) / (subItems.Count - 1);
    }

    // Update is called once per frame
    void Update()
    {
        percentage = Mathf.Lerp(percentage, destPercent, Time.deltaTime * 3);


        float angle = Mathf.Lerp(0, Mathf.Abs(angleOffset) * (subItems.Count - 1), percentage);

        gear.transform.eulerAngles = new Vector3(0, 0, angle);

        // print(angle);
        for (int i = 0; i < subItems.Count; i++)
        {
            subItems[i].transform.position = transform.position + new Vector3(Mathf.Cos((angle + angleOffset * i) * Mathf.Deg2Rad), Mathf.Sin((angle + angleOffset * i) * Mathf.Deg2Rad), 0) * radius;

            float circlePercent = Mathf.Acos((subItems[i].transform.position - transform.position).x / radius) * Mathf.Rad2Deg;
            float circle01 = Mathf.InverseLerp(-sizeAngle, sizeAngle, circlePercent);
            float delta = Mathf.Abs((2f * Mathf.Abs(circle01 - 0.5f) - 1f));
            subItems[i].transform.localScale = Vector3.one * delta;

            circle01 = Mathf.InverseLerp(-colorAngle, colorAngle, circlePercent);
            delta = Mathf.Abs((2f * Mathf.Abs(circle01 - 0.5f) - 1f));
            subItems[i].color = Color.Lerp(hideColor, mainColor, delta);
        }

        title.text = subItems[GetCurrentIndex()].text;

        info.musicIndex = GetCurrentIndex();

        int up = (int)Mathf.Ceil(Mathf.Lerp(0, subItems.Count - 1, percentage));
        int down = (int)Mathf.Floor(Mathf.Lerp(0, subItems.Count - 1, percentage));
        foreground.spriteName = sprites[up];
        background.spriteName = sprites[down];
        foreground.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, Mathf.Repeat(Mathf.Lerp(0, subItems.Count - 1, percentage), 1.0f));
    }
}