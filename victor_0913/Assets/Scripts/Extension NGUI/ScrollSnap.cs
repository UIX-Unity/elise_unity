using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Scroll Snap : [public] Left, Right
/// UpdateTweenScaleOnCenter : Distance Scaling
/// </summary>
public class ScrollSnap : MonoBehaviour {


    UIGrid uiGrid;

    List<Transform> list;

    UIScrollView scrollView;
    UIPanel panel;

    UICenterOnChild center;

    int index;

    public int EpisodeIndex
    {
        get
        {
            return index;
        }
    }

	// Use this for initialization
	void Start ()
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
            if (list[i].localPosition.x < panel.clipOffset.x + 250 &&
                list[i].localPosition.x > panel.clipOffset.x - 250)
            {
                index = i;
                //SongSelectScene.GetInstance.SetTheme(i);
            }
        }
        UpdateTweenScaleOnCenter();
    }
    /// <summary>
    /// LEFT SNAP
    /// </summary>
    public void Left()
    {
        if(index > 0)
        {
            index--;

            center.CenterOn(list[index]);
            //panel.transform.localPosition = -list[index].localPosition;
            //panel.clipOffset = list[index].localPosition;
        }
    }

    /// <summary>
    /// RIGHT SNAP
    /// </summary>
    public void Right()
    {
        if(index < list.Count - 1)
        {
            index++;
            center.CenterOn(list[index]);
            //panel.transform.localPosition = -list[index].localPosition;
            //panel.clipOffset = list[index].localPosition;
        }
    }

    /// <summary>
    /// UPDATE
    /// </summary>
    void UpdateTweenScaleOnCenter()
    {
        for (int i = 0; i < list.Count; i++)
        {
            float ox = Vector3.Distance(list[i].localPosition, panel.clipOffset);

            float s = Mathf.Cos(ox / 13f * Mathf.Deg2Rad);

            list[i].localScale = new Vector3(s, s, s);
        }
    }

    public void MoveToIndex(int theme)
    {
        center.CenterOn(list[theme]);
    }

    public int GetIndex()
    {
        return index;
    }
}