using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    [Range(-0.5f,1.5f)]
    [SerializeField]
    float value;

    UI2DSprite sprite;
	// Use this for initialization
	void Start () {
        sprite = GetComponent<UI2DSprite>();
	}
	
	// Update is called once per frame
	void Update () {
        if(sprite.drawCall != null)
        {
            sprite.drawCall.dynamicMaterial.SetFloat("_Distortion", value);
        }
    }
}
