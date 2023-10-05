using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//003.ㅇ의 영역

public class IllustControl : MonoBehaviour
{
    GameSetting info;

    //밑에 두개 넣어줄거임
    [SerializeField] UISprite customSprite;
    [SerializeField] UILabel customLabel;

    //스토리 내용
    [SerializeField] StoryMaker item;
    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;

        
        customSprite.atlas = item.illustList[info.itemAtlasList];
        
      

        customSprite.spriteName = item.item[info.itemRandom].illustName;

        customLabel.text = item.item[info.itemRandom].Story;

        info.energyLavel++;
    }
}
