using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * v1.10 ('20.02.12)
 *  - minCurDirPadding 추가
 */


public class SliceSliderUI : UIBase
{

    public enum Direction
    {
        LeftToRight = 0,
        RightToLeft = 1,
        BottomToTop = 2,
        TopToBottom = 3
    }

    [SerializeField]
    private Image frame;
    private RectTransform frameRtf;

    [SerializeField]
    private Image bar;
    private RectTransform barRtf;

    public Direction dir;
    public RectOffset padding;

    public float minCurDirPadding;

    [Range(0,1)]
    [SerializeField]
    private float m_FillAmount;
    public float fillAmount
    {
        get { return Mathf.Clamp(m_FillAmount, 0, 1); }
        set { m_FillAmount = Mathf.Clamp(value, 0, 1); UpdateUI(); }
    }

    public Color color { get { return bar.color; } set { bar.color = value; } }

    private void OnEnable()
    {
        frameRtf = frame.rectTransform;
        barRtf = bar.rectTransform;

        barRtf.anchorMax = Vector2.one;
        barRtf.anchorMin = Vector2.zero;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!gameObject.activeInHierarchy) return;

        switch(dir)
        {
            case Direction.BottomToTop:
                barRtf.offsetMin = new Vector2(padding.left, padding.bottom);
                barRtf.offsetMax = new Vector2(-padding.right, -(minCurDirPadding + Mathf.Lerp(frameRtf.rect.height - padding.top, padding.top - minCurDirPadding, fillAmount)));
                break;

            case Direction.LeftToRight:
                barRtf.offsetMin = new Vector2(padding.left, padding.bottom);
                barRtf.offsetMax = new Vector2(-(minCurDirPadding + Mathf.Lerp(frameRtf.rect.width - padding.left - minCurDirPadding, padding.right, fillAmount)), -padding.top);
                break;

            case Direction.RightToLeft:
                barRtf.offsetMin = new Vector2(minCurDirPadding + Mathf.Lerp(frameRtf.rect.width - padding.right - minCurDirPadding, padding.left, fillAmount), padding.bottom);
                barRtf.offsetMax = new Vector2(-padding.right, -padding.top);
                break;

            case Direction.TopToBottom:
                barRtf.offsetMin = new Vector2(padding.left, minCurDirPadding + Mathf.Lerp(frameRtf.rect.height - padding.bottom - minCurDirPadding, padding.bottom, fillAmount));
                barRtf.offsetMax = new Vector2(-padding.right, -padding.top);
                break;
        }
    }
}
