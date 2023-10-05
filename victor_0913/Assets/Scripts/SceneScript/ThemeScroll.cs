using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeScroll : MonoBehaviour
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

    public Material grayControl;

    [SerializeField] UISprite epSprite;

    [SerializeField] UILabel episode;
    [SerializeField] UILabel episodeTitle;
    [SerializeField] UISprite gear;

    [SerializeField] UISprite episodeImage;

    [SerializeField] ParticleSystem particle;
    [SerializeField] UnityEngine.GameObject circleParticle;

    //회전값 scrollView
    [SerializeField] UnityEngine.GameObject gridObject;
    [SerializeField] UnityEngine.GameObject scrollView;
    [SerializeField] UnityEngine.GameObject[] selectepList;
    [SerializeField] Transform[] positionEpList;
    [SerializeField] public UnityEngine.GameObject[] inappButton;
    public bool check;
    // Use this for initialization
    void Start ()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        check = false;
       
   
        for(int i =0;i<selectepList.Length;i++)
        {
            selectepList[i].GetComponent<BoxCollider>().enabled = false;
        }
        EpiUnRock();
        for (int i = 0; i < info.epiRock.Length; i++)
        {
            Debug.Log("잠금상태확인" + i + ":" + info.epiRock[i]);

        }
    }
    void EpiUnRock()
    {
        for (int i = 1; i < 3; i++)
        {
            if (info.epiRock[i] == true)
            {
                selectepList[i].GetComponent<UISprite>().spriteName = "005_Episode_icon_play";
            }
            else
            {
                selectepList[i].GetComponent<UISprite>().spriteName = "005_Episode_icon_lock";
            }
        }
        
    }
    public void AnimationStart()
    {
        info.buttonControl = true;
    }
    public void AnimationEnd()
    {
        info.buttonControl = false;
    }
    private void OnEnable()
    {
        //gridObject.GetComponent<Transform>().localPosition = new Vector3(transform.position.x, Mathf.Lerp(0f, 300f, destPercent), 0);

        float posY = 0;
        isDrag = false;
        if (destPercent == 0f)
        {
            gridObject.GetComponent<Transform>().localPosition = new Vector2(0, 0);
            posY = 0;
            for(int i= 0;i<positionEpList.Length;i++)
            {
                positionEpList[i].localPosition = new Vector3(positionEpList[i].localPosition.x, posY, positionEpList[i].localPosition.z);
                posY -= 150f;
            }
            //positionEpList[0].localPosition = new Vector3(positionEpList[0].localPosition.x, 0, positionEpList[0].localPosition.z);
            //positionEpList[1].localPosition = new Vector3(positionEpList[1].localPosition.x, -150f, positionEpList[1].localPosition.z);
            //positionEpList[2].localPosition = new Vector3(positionEpList[2].localPosition.x, -300f, positionEpList[2].localPosition.z);
        }
        if (destPercent > 0.33f && destPercent < 34f)
        {
            gridObject.GetComponent<Transform>().localPosition = new Vector2(0, 149f);
            posY = 150f;
            for (int i = 0; i < positionEpList.Length; i++)
            {
                positionEpList[i].localPosition = new Vector3(positionEpList[i].localPosition.x, posY, positionEpList[i].localPosition.z);
                posY -= 150f;
            }
            //positionEpList[0].localPosition = new Vector3(positionEpList[0].localPosition.x, 150f, positionEpList[0].localPosition.z);
            //positionEpList[1].localPosition = new Vector3(positionEpList[1].localPosition.x, 0, positionEpList[1].localPosition.z);
            //positionEpList[2].localPosition = new Vector3(positionEpList[2].localPosition.x, -150f, positionEpList[2].localPosition.z);
        }
        if (destPercent  > 0.66f && destPercent < 0.67f)
        {
            Debug.Log("true");
            gridObject.GetComponent<Transform>().localPosition = new Vector2(0, 299);
            posY = 300f;
            for (int i = 0; i < positionEpList.Length; i++)
            {
                positionEpList[i].localPosition = new Vector3(positionEpList[i].localPosition.x, posY, positionEpList[i].localPosition.z);
                posY -= 150f;
            }
            //positionEpList[0].localPosition = new Vector3(positionEpList[0].localPosition.x, 300f, positionEpList[0].localPosition.z);
            //positionEpList[1].localPosition = new Vector3(positionEpList[1].localPosition.x, 150f, positionEpList[1].localPosition.z);
            //positionEpList[2].localPosition = new Vector3(positionEpList[2].localPosition.x, 0f, positionEpList[2].localPosition.z);
        }
        if (destPercent == 1)
        {
            gridObject.GetComponent<Transform>().localPosition = new Vector2(0, 449);
            posY = 450f;
            for (int i = 0; i < positionEpList.Length; i++)
            {
                positionEpList[i].localPosition = new Vector3(positionEpList[i].localPosition.x, posY, positionEpList[i].localPosition.z);
                posY -= 150f;
            }
         
            //positionEpList[0].localPosition = new Vector3(positionEpList[0].localPosition.x, 300f, positionEpList[0].localPosition.z);
            //positionEpList[1].localPosition = new Vector3(positionEpList[1].localPosition.x, 150f, positionEpList[1].localPosition.z);
            //positionEpList[2].localPosition = new Vector3(positionEpList[2].localPosition.x, 0f, positionEpList[2].localPosition.z);
        }
    }

    private void Update()
    {
        //scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(0, 79);
        //scrollView.GetComponent<Transform>().localPosition = new Vector3(0, -79, 0);
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            oldPos = Input.mousePosition;
            downPos = oldPos;
            check = false;
        }

        if(Input.GetMouseButtonUp(0))
        {
            isDrag = false;
         
            SetCenter();

            check = true;
        }

        if(isDrag)
        { 
            float distance = (oldPos - Input.mousePosition).y;

            percentage = Mathf.Clamp01(percentage + distance / Screen.height);
            oldPos = Input.mousePosition;

            indexTemp = GetCurrentIndex();
            info.episodeIndex = indexTemp;

        

            gridObject.GetComponent<Transform>().localPosition = new Vector3(transform.position.x, Mathf.Lerp(0f, 450f, percentage), 0);

        }
        else
        {
            percentage = Mathf.Lerp(percentage, destPercent, Time.deltaTime * 3);

            gridObject.GetComponent<Transform>().localPosition = new Vector3(transform.position.x, Mathf.Lerp(0f, 450f, percentage), 0);
        }

        UpdateUI();
    }

    private int GetCurrentIndex()
    {
        return (int)Mathf.Round(Mathf.Lerp(0, info.episodes.Length - 1, percentage));
    }

    private void SetCenter()
    {
        destPercent = ((float)GetCurrentIndex()) / (info.episodes.Length - 1);

        
    }

    private void UpdateUI()
    {
        if (index != indexTemp)
        {
            index = indexTemp;
            particle.Play();
        }

        if(info.epiRock[3] == true)
        {
            selectepList[3] = inappButton[0];
        }
        else
        {
            selectepList[3] = inappButton[1];
        }

        for (int i = 0; i < selectepList.Length; i++)
        {
            selectepList[i].GetComponent<BoxCollider>().enabled = false;

        }
        selectepList[index].GetComponent<BoxCollider>().enabled = true;
    
      
        //if (index == 1 || index == 2)
        //{
        //    epSprite.spriteName = "005_Episode_icon_lock";
        //}
        //else
        //{
        //    epSprite.spriteName = "005_Episode_icon_play";
        //}

        //episode.text = "EPISODE" + (GetCurrentIndex() + 1);
        //episodeTitle.text = info.episodes[GetCurrentIndex()].episodeName;

        float angle = Mathf.Lerp(0f, 30 * info.episodes.Length - 1, percentage);
        gear.transform.localEulerAngles = new Vector3(0, 0, -angle);

        episodeImage.spriteName = info.episodes[GetCurrentIndex()].episodeIcon;

        if(info.epiRock[GetCurrentIndex()] == true)
        {
            info.selectedEpisode.episodeIcon = "005_Episode_illust_" + (GetCurrentIndex() + 1) + "_on";
            grayControl.SetFloat("Effect Amount", 1);
        }
        else
        {
            info.selectedEpisode.episodeIcon = "005_Episode_illust_" + (GetCurrentIndex() + 1) + "_off";
            grayControl.SetFloat("Effect Amount", 0);
        }

        episodeImage.GrayScale(grayTest);


        EpiUnRock();

        //if (gridObject.GetComponent<Transform>().localPosition.y < 50 && check == true)
        //{
        //    percentage = 1f;
        //}
        //else if (Equals(gridObject.GetComponent<Transform>().localPosition.y, 150) && check == true)
        //{
        //    percentage = 0.5f;
        //}
        //else if (Equals(gridObject.GetComponent<Transform>().localPosition.y, 300) && check == true)
        //{
        //    percentage = 0f;

        //}

        //if (percentage < 0.1f)
        //{
        //    gridObject.GetComponent<Transform>().localPosition = new Vector3(gridObject.GetComponent<Transform>().localPosition.x, 0f, gridObject.GetComponent<Transform>().localPosition.z);
        //}
        //else if (percentage < 0.6f && percentage > 0.4f)
        //{
        //    gridObject.GetComponent<Transform>().localPosition = new Vector3(gridObject.GetComponent<Transform>().localPosition.x, 150f, gridObject.GetComponent<Transform>().localPosition.z);
        //}
        //else if (percentage >0.9f)
        //{
        //    gridObject.GetComponent<Transform>().localPosition = new Vector3(gridObject.GetComponent<Transform>().localPosition.x, 300f, gridObject.GetComponent<Transform>().localPosition.z);
        //}
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    indexOffset = Mathf.Lerp(indexOffset, destPercent, Time.deltaTime * 3);

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        isDrag = true;
    //        oldPos = Input.mousePosition;
    //        downPos = oldPos;
    //    }

    //    if(Input.GetMouseButtonUp(0))
    //    {
    //        isDrag = false;
    //        SetCenter();
    //    }

    //    if(isDrag)
    //    {
    //        downPos = Input.mousePosition;
    //        offsetY = Mathf.Clamp((downPos - oldPos).y, -1f, 1f);
    //        oldPos = downPos;

    //        indexOffset = Mathf.Clamp((indexOffset -= offsetY * Time.deltaTime * 2f), 0f, 2f);
    //        index = (int)Mathf.Round(indexOffset);
    //    }

    //    UpdateUI();
    //}

    //int GetIndex
    //{
    //    get
    //    {
    //        return (int)Mathf.Round(indexOffset);
    //    }
    //}

    //void SetCenter()
    //{
    //    destPercent = (float)GetIndex / (info.episodes.Length - 1);
    //}

    //void UpdateUI()
    //{
    //    episode.text = "EPISODE" + (index + 1);
    //    episodeTitle.text = info.episodes[index].episodeName;

    //    float angle = Mathf.Lerp(0f, 30 * info.episodes.Length - 1, indexOffset);
    //    gear.transform.localEulerAngles = new Vector3(0, 0, -angle / 3);
    //}
}
