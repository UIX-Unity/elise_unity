using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class EnegryLavel : MonoBehaviour
{
    GameSetting info;
    [SerializeField] UnityEngine.GameObject uiEvent;
    //관안에있는 엘리스와 이펙트
    [SerializeField] UnityEngine.GameObject[] elise;

  
    //exp초기화
    [SerializeField] startMainScript inti;
    //
   

    // Start is called before the first frame update
    void Start()
    {

        info = Resources.Load("GameSetting") as GameSetting;

        if (ItemMaxCheck() == false)
        {
            if (info.exp == 100)
            {
                uiEvent.SetActive(true);

            }
        }
        else
        {
            uiEvent.SetActive(false);
        }
        if (info.energyLavel == 0)
        {
            elise[0].SetActive(true);
            uiEvent.transform.localPosition = new Vector3(20, 765, 0);
        }
        else
        {
            elise[1].SetActive(true);
            uiEvent.transform.localPosition = new Vector3(258, 552,0);
            if (info.exp < 30)
            {
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal", true);
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal2", false);
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal3", false);
            }
            else if (info.exp > 30 && info.exp < 70)
            {
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal", false);
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal2", true);
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal3", false);
            }
            else
            {
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal", false);
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal2", false);
                elise[1].GetComponent<Animator>().SetBool("eliseIdeal3", true);

            }
        }
    }
#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            elise[0].SetActive(true);
            info.exp = 100f;
            info.expSample = 100f;
            uiEvent.SetActive(true);
        }
    }
    
#endif

    public void UiButtonClick()
    {
        if(info.buttonControl == false)
        {
            //아이템 max치일때 수정해야함
            //에너지 레벨이랑 열린에피소드에 아이템개수랑 맞을때 로 바ㅜ꺼야함
            if (ItemMaxCheck() ==true)
            {
                return;
            }
            GetRondomItem();
            StartCoroutine(StartSceneClose());
        }
    }
    public bool ItemMaxCheck()
    {
        //시작
        int result = 0;
        //끝
       
        for (int i = 0; i < info.epiRock.Length; i++)
        {
            if (true == info.epiRock[i])
            {
                result += info.itemEpiList[i+1];
                
            }
        }
        //얻을수있는 아이템이 없을경우 true반환
        if(info.energyLavel == result)
        {
            info.energyLavel = result;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GetRondomItem()
    {
        int max = 0;
        if(info.energyLavel != 0)
        {
            //에피소드 범위설정
            info.itemRandom = GetRandomInt(0, info.epiRock.Length);
        }
        else
        {
            info.itemRock[0] = true;
            info.itemRandom = 0;
            info.itemAtlasList = 0;
        }
    }
    //중복제거
    public int GetRandomInt( int min, int max)
    {
        int random = 0;
        bool isSame = false;
        //시작
        int result = 0;
        //끝
        int result1 = 0;

        while (true)
        {
            
            random = Random.Range(min, max);
            if (info.epiRock[random] == true)
            {
                for(int i=0;i <=random;i++)
                {
                    result += info.itemEpiList[i];

                    result1 += info.itemEpiList[i + 1];
                }
                
                       
                for (int i = result; i < result1; i++)
                {
                    if (info.itemRock[i] == false)
                    {
                        isSame = true;
                    }
                }
            }

            if (isSame == true)
            {
                isSame = false;
                info.itemAtlasList = random;
                break;
            }
            else
            {
                result = 0;
                result1 = 0;
            }

        }
        while (true)
        {
            random = Random.Range(result,result1);
            Debug.Log("random : "+random);
            if (info.itemRock[random] == false)
            {

                isSame = true;
                info.itemRock[random] = true;
                break;
            }
            //에피소드 락
            
        }

        return random;
    }

    IEnumerator StartSceneClose()
    { 
        //에너지 0으로 초기화하고 플레이펩에 저장시켜줌
        inti.expInti();
        inti.SetPlayFabItem();
        info.buttonControl = true;
        FadePanel.GetInstance.ScreenFade(false, 1);
        yield return new WaitForSeconds(2f);
        info.buttonControl = false;
        SceneManager.LoadScene(5);
    }
}
