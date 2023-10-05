using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageUI : UIBase
{
    [SerializeField]
    private ScrollRect m_ItemScroll;
    public ScrollRect itemScroll => m_ItemScroll;

    [SerializeField]
    private EventTrigger m_ItemScrollEventTrigger;
    public EventTrigger itemScrollEventTrigger => m_ItemScrollEventTrigger;

    [SerializeField]
    private PageNavigatorUI m_PageNavegatorUI;
    public PageNavigatorUI pageNavigatorUI => m_PageNavegatorUI;

    public int prePageIdx { get; private set; }
    public int curPageIdx { get; private set; }
    public int pageCount { get; private set; }

    /// <summary>
    /// onChangePage(PageUI ui, int prePageIdx, int curPageIdx)
    /// </summary>
    public UnityAction<PageUI, int, int> onChangePage;

    public override void Open()
    {
        base.Open();
        pageNavigatorUI.Open();
    }

    public override void Close()
    {
        base.Close();
        pageNavigatorUI.Close();
    }

    public void SetData(int pageCount, UnityAction<PageUI, int, int> onChangePage)
    {
        this.pageCount = pageCount;
        this.onChangePage = onChangePage;
    }

    public void SetPage(int pageIdx)
    {
        prePageIdx = curPageIdx;
        curPageIdx = Mathf.Clamp(pageIdx, 0, Mathf.Max(0, pageCount - 1));
        UpdatePage();
    }

    public void UpdatePage()
    {
        pageNavigatorUI.SetData(pageCount, curPageIdx);
        pageNavigatorUI.UpdateUI();

        onChangePage?.Invoke(this, prePageIdx, curPageIdx);
    }

    private void Awake()
    {
        // 스크롤 이벤트 세팅
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((eventData) =>
        {
            var pointerEventData = eventData as PointerEventData;

            // 왼쪽 페이지 이동
            if (itemScroll.content.anchoredPosition.x >= 100)
            {
                if (curPageIdx > 0)
                {
                    itemScroll.content.anchoredPosition = new Vector2(-1500, itemScroll.content.anchoredPosition.y);
                    prePageIdx = curPageIdx;
                    curPageIdx = Mathf.Max(curPageIdx - 1, 0); 
                    UpdatePage();
                }
            }
            // 오른쪽 페이지 이동
            else if (itemScroll.content.anchoredPosition.x < -100)
            {
                if (curPageIdx < pageCount - 1)
                {
                    itemScroll.content.anchoredPosition = new Vector2(1500, itemScroll.content.anchoredPosition.y);
                    prePageIdx = curPageIdx;
                    curPageIdx = Mathf.Min(curPageIdx + 1, pageCount - 1);
                    UpdatePage();
                }
            }
        });
        itemScrollEventTrigger.triggers.Add(entry);
    }
}
