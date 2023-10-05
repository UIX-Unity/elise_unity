using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityAction beginAction;
    public UnityAction<float> dragAction;
    public UnityAction endAction;

    public Slider slider;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginAction?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragAction?.Invoke(slider.value);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endAction?.Invoke();
    }
}
