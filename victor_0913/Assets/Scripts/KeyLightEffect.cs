using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLightEffect : MonoBehaviour
{
    [SerializeField]
    Color activeClr;
    Color defaultClr;

    UISprite sprite;

    bool isActive;
    float clrVal;

    [SerializeField]
    float tansitionSpd = 3f;
    // Use this for initialization
	void Awake()
    {
        sprite = GetComponent<UISprite>();
        defaultClr = sprite.color;
    }

    public void Active()
    {
        clrVal = 0;
        isActive = true;
    }
    public void DeActive()
    {
        clrVal = 0;
        isActive = false;
    }
	// Update is called once per frame
	void Update ()
    {
        clrVal += Time.deltaTime * tansitionSpd;
        if (isActive)
        {
            sprite.color = Color.Lerp(defaultClr, activeClr, clrVal);
        }
        else
        {
            sprite.color = Color.Lerp(activeClr, defaultClr, clrVal);
        }
    }
}
