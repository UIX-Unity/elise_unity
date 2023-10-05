using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollStop : MonoBehaviour
{
    [SerializeField] UIPanel panel;
    [SerializeField] UIGrid gridObject;

    Vector3 constraint;
    bool isDrag;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (panel.transform.localPosition.y < 0)
        //{
        ////gridObject.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        //   panel.transform.localPosition = new Vector3(-400, -3, 0);
        //    panel.clipOffset = new Vector2(0, 3);
        //   // panel.GetComponent<UIScrollView>().ResetPosition();
        //}
        //else if (panel.transform.localPosition.y > 460)
        //{
        //    panel.transform.localPosition = new Vector3(-400, 462, 0);
        //    panel.clipOffset = new Vector2(0, -462);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
        if(isDrag== true)
        {
            panel.transform.localPosition = new Vector3(-400, Mathf.Min(1160, Mathf.Max(-3,panel.transform.localPosition.y)), 0);
            panel.clipOffset = new Vector2(0, Mathf.Max(-1160, Mathf.Min(3, panel.clipOffset.y)));
        }
    }
    
}
