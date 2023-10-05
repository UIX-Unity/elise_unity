using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageObjectControl : MonoBehaviour
{
    GameSetting info;
    public UISprite[] sprite;

    public StoryMaker item;
    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;


        InitIllust();
    }
    void InitIllust()
    {
        int epi = 0;
        int epiMax = 0;
        int list = -1;
        for (int i = 0; i < info.itemRock.Length; i++)
        {
            if(epi ==epiMax)
            {
                list++;
                epiMax += info.itemEpiList[list+1];
            }
            
            if(epi < epiMax)
            {
                if (info.itemRock[i] == true)
                {
                    sprite[i].spriteName = item.item[i].illustName;
                    sprite[i].atlas = item.illustList[list];
                }
                epi++;
            }
            
        }
    }

}
