using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "GameSetting", menuName = "Create New GameSetting")]
public class GameSetting : ScriptableObject
{
    [HideInInspector]
    public Episode selectedEpisode
    {
        get
        {
            return episodes[episodeIndex];
        }
    }
    [HideInInspector]
    public Music selectedMusic
    {
        get
        {
            return selectedEpisode.musics[musicIndex];
        }
    }
    [SerializeField] public Product user;

    public Episode[] episodes;
    
    public int episodeIndex;
    
    public int musicIndex;

    public float speed;

    public int lv;

    public float noteSync;

    public float BPM;

    public float sound;

    public bool debugMode;


    [Header("가지고 있는 전기")]
    //
    public float exp;
    //연출용 exp

    [Header("전기 증가 연출용")]
    public float expSample;

    [Header("노트 판정 범위")]
    [Space]
    [Tooltip("0 : PERFECT  1 : GREAT  2 : Good")]
    public float[] noteRange;
    
    [Header("랭크 판정 범위")]
    [Space]
    [Tooltip("0==S, 1==A, 2==B, 3==C, 4==D, 5==F")]
    public float[] rankRange;
    [Header("1% 당 애너지 획득량")]
    [Space]
    public float rankPlusExp;
    [Header("처음플레이하는 곡 에너지추가 획득량")]
    [Space]
    public float firstExp;
    [Header("로그인 체크 true = 구글로그인 false = 게스트 로그인")]
    [Space]
    public bool loginCheck;
    [Header("스토리 진행도 0 = 엘리스부활 1 ...3.4.5(느낌표: 아이템들)")]
    [Space]
    public int energyLavel;
    [Header("에피소드 음악 진행도([0] 에피소드 1안에있는 곡들의 진행도의 합)")]
    [Space]
    public float[] epiMaxMusicSocre;
    [Header("에피소드 해금 조건 [0]==에피소드 2 해금조건 : 에피1 300% [1]==에피소드3  해금조건 :에피2 300%")]
    [Space]
    public float[] epiUnRockScore;
    [Header("playFab아이디")]
    [Space]
    public string myId;
    [Header("열린에피소드 ( true 열림 , false 닫힘")]
    [Space]
    public bool[] epiRock;
    [Header("열린아이템 ( true 열림 , false 닫힘")]
    [Space]
    public bool[] itemRock;
    [Header("에피소드에 아이템이 몇개가 들어있는지 확인(0+a ~ ?)무조건 에피소드보다 한개 많아야함")]
    [Space]
    public int[] itemEpiList;
    [Header("아이템 열릴때 랜덤한 값을넣어준다.")]
    [Space]
    public int itemRandom;
    [Header("아이템 atlas [값]")]
    [Space]
    public int itemAtlasList;
    [Header("버튼제어 모든버튼에서 사용하고 다른 기능이 가동중일때 다른 모든 버튼의 사용을 정지함 ( true 사용불가능 , false 버튼사용가능")]
    [Space]
    public bool buttonControl;

#if UNITY_EDITOR
    public enum KeySettingEnum
    {
        
        A = KeyCode.A,
        S = KeyCode.S,
        D = KeyCode.D,
        F = KeyCode.F,
        G = KeyCode.G,
        H = KeyCode.H,
        J = KeyCode.J,
        K = KeyCode.K,
        L = KeyCode.L
    }
#endif
#if UNITY_EDITOR
    public KeySettingEnum[] Key;
#endif

}
