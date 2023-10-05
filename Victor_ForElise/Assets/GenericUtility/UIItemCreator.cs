using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class UIItemCreator<T> where T : UIBase
{
    public delegate bool Condition(T item);
    public delegate void UpdateItem(int idx, T item);
    public delegate void EndUpdateItems();

    public T itemPrefab { get; private set; }

    public RectTransform contentRtf { get; private set; }

    private List<T> items = new List<T>();
    public int ItemCount
    {
        get
        {
            if (items == null)
            {
                return 0;
            }
            return items.Count;
        }
    }

    public void SetData(RectTransform contentRtf, T itemPrefab)
    {
        this.contentRtf = contentRtf;
        this.itemPrefab = itemPrefab;
    }

    public void UpdateItems(int count, UpdateItem onUpdateItem, EndUpdateItems onEndUpdateItems = null)
    {
        itemPrefab.gameObject.SetActive(false);

        for (int i = 0; i < count; i++)
        {

            if (items.Count > i)
            {
                onUpdateItem(i, items[i]);
            }
            else
            {
                var newItem = GameObject.Instantiate(itemPrefab, contentRtf.transform);
                newItem.transform.localPosition = Vector3.zero;
                items.Add(newItem);

                onUpdateItem(i, items[i]);
            }
        }

        for (int i = count; i < items.Count; i++)
        {
            items[i].Close();
        }

        if (onEndUpdateItems != null)
        {
            onEndUpdateItems();
        }
    }

    public IEnumerator UpdateAniItems(int count, UpdateItem onUpdateItem, EndUpdateItems onEndUpdateItems = null)
    {
        itemPrefab.gameObject.SetActive(false);

        for (int i = 0; i < count; i++)
        {

            if (items.Count > i)
            {
                onUpdateItem(i, items[i]);
            }
            else
            {
                var newItem = GameObject.Instantiate(itemPrefab, contentRtf.transform);

                newItem.transform.localPosition = Vector3.zero;
                items.Add(newItem);

                onUpdateItem(i, items[i]);
            }

            yield return new WaitForSeconds(0.5f);
        }


        for (int i = count; i < items.Count; i++)
        {
            items[i].Close();
        }

        if (onEndUpdateItems != null)
        {
            onEndUpdateItems();
        }

        yield break;
    }

    public void UpdateItemEmpty(int count, UpdateItem OnUpdateEmpty, EndUpdateItems onEndUpdateItems = null)
    {

        itemPrefab.gameObject.SetActive(false);
        for (int i = 0; i < count; i++)
        {
            if (items.Count > i)
            {

            }
            else
            {
                var newItem = GameObject.Instantiate(itemPrefab, contentRtf.transform);
                newItem.transform.localPosition = Vector3.zero;
                items.Add(newItem);

                OnUpdateEmpty(i, items[i]);
            }

        }

        if (onEndUpdateItems != null)
        {
            onEndUpdateItems();
        }
    }

    public void UpdateItemSlotClose(int count)
    {

        for (int i = count; i < items.Count; i++)
        {
            items[i].Close();
        }

    }

    public T GetItem(int idx)
    {
        if (idx >= 0 && items.Count > idx)
        {
            return items[idx];
        }
        return null;
    }

    public T FindItem(Condition condition)
    {
        foreach (var i in items)
        {
            if (condition(i))
            {
                return i;
            }
        }
        return null;
    }

    public bool TryGetItem(int idx, out T item)
    {
        if (idx >= 0 && items.Count > idx)
        {
            item = items[idx];
            return true;
        }
        item = null;
        return false;
    }

    public bool TryFindItem(Condition condition, out T item)
    {
        foreach (var i in items)
        {
            if (condition(i))
            {
                item = i;
                return true;
            }
        }

        item = null;
        return false;
    }

    public void CloseAllItems()
    {
        foreach (var i in items)
        {
            i.Close();
        }
    }

    public void Release()
    {
        foreach (var i in items)
        {
            GameObject.DestroyImmediate(i.gameObject);
        }
        items.Clear();

        itemPrefab = null;
        contentRtf = null;
    }
}