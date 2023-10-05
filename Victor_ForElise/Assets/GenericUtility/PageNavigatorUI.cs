using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PageNavigatorUI : UIBase
{
    [SerializeField]
    private PageNavigatorUI_Item m_ItemPrefab;
    public PageNavigatorUI_Item itemPrefab => m_ItemPrefab;

    [SerializeField]
    private RectTransform m_Contents;
    public RectTransform contents => m_Contents;

    private UIItemCreator<PageNavigatorUI_Item> itemCreator;

    public UnityAction<int, PageNavigatorUI_Item> onUpdateItem;
    public UnityAction onEndUpdateItem;

    public int pageCount { get; private set; }
    public int curPageIdx { get; private set; }

    public void Awake()
    {
        itemCreator = new UIItemCreator<PageNavigatorUI_Item>();
        itemCreator.SetData(contents, itemPrefab);

        itemPrefab.Close();
    }

    public void SetData(int pageCount, int curPageIdx)
    {
        this.pageCount = pageCount;
        this.curPageIdx = curPageIdx;
    }

    public override void Open()
    {
        base.Open();

        UpdateUI();
    }

    public void UpdateUI()
    {
        itemCreator.UpdateItems(pageCount, UpdateItem, EndUpdateItem);
    }

    private void UpdateItem(int idx, PageNavigatorUI_Item item)
    {
        item.SetData(this, (idx == curPageIdx));
        item.Open();
        item.UpdateUI();

        onUpdateItem?.Invoke(idx, item);
    }

    private void EndUpdateItem()
    {
        onEndUpdateItem?.Invoke();
    }
}
