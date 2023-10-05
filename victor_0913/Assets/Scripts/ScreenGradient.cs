using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class ScreenGradient : MonoBehaviour
{
    UITexture texture;
    [SerializeField]
    float value;
    // Use this for initialization
    void Awake()
    {
        texture = gameObject.GetComponent<UITexture>();
    }

    // Update is called once per frame
    void Update()
    {
        if (texture != null) texture.drawCall.dynamicMaterial.SetFloat("_Pow", value);
    }
}