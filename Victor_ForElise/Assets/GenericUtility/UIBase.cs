using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour {

    public bool isOpened => gameObject.activeInHierarchy;

    [SerializeField]
    private RectTransform m_Rtf;
    public RectTransform rtf { get { return m_Rtf; } set { m_Rtf = value; } }
    
    public virtual void Open()
    { 
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

   
}
