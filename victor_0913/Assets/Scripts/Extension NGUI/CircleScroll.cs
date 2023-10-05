using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScroll : MonoBehaviour
{
    UIGrid uiGrid;

    List<Transform> list;

    UIScrollView scrollView;
    UIPanel panel;

    UICenterOnChild center;

    int index = 0;

    public UILabel title;
    public UILabel level;
    public UILabel percent;

    // Use this for initialization
    void Start()
    {
        center = GetComponent<UICenterOnChild>();
        uiGrid = GetComponent<UIGrid>();
        list = uiGrid.GetChildList();
        panel = GetComponentInParent<UIPanel>();
        scrollView = GetComponentInParent<UIScrollView>();
    }

    // FIND CURRENT INDEX
    void LateUpdate()
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].localPosition.y < panel.clipOffset.y + 50 &&
                list[i].localPosition.y > panel.clipOffset.y - 50)
            {
                index = i;
                UpdateLabel();
            }
        }
        Test();

        //for (int i = 0; i < list.Count; i++)
        //{
        //    float oy = Mathf.Abs(list[i].localPosition.y - panel.clipOffset.y);

        //    float s = Mathf.Cos(oy * Mathf.Deg2Rad);
        //    Debug.Log(i + " : " + oy + "[" + s + "]");
        //    if (oy >= 80)
        //        list[i].localPosition = new Vector3(-100, list[i].localPosition.y);
        //    else if (oy >= 180)
        //        list[i].localPosition = new Vector3(-200, list[i].localPosition.y);
        //}
    }

    void UpdateLabel()
    {
        title.text = list[index].GetComponentInChildren<UILabel>().text;
    }

    void Test()
    {
        switch (index)
        {
            case 0:
                list[0].gameObject.SetActive(false);
                list[1].gameObject.SetActive(true);
                list[2].gameObject.SetActive(true);
                list[3].gameObject.SetActive(false);
                list[4].gameObject.SetActive(false);
                list[1].transform.localPosition = new Vector3(20, -200);
                list[2].transform.localPosition = new Vector3(-50, -260);
                break;

            case 1:
                list[0].gameObject.SetActive(true);
                list[1].gameObject.SetActive(false);
                list[2].gameObject.SetActive(true);
                list[3].gameObject.SetActive(true);
                list[4].gameObject.SetActive(false);
                list[0].transform.localPosition = new Vector3(20, -200);
                list[2].transform.localPosition = new Vector3(20, -200);
                list[3].transform.localPosition = new Vector3(-50, -260);
                break;
        }
    }
}
