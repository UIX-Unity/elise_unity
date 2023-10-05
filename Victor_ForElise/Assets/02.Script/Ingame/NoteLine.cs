using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteLine : MonoBehaviour
{
    private Tween shadowTween;
    private Tween scaleTween;

    [SerializeField]
    private SpriteRenderer shadowLine;
    [SerializeField]
    private Transform scaleLine;

    [SerializeField]
    private Color color = new Color(1f,1f,1f,0.5f);

    [SerializeField]
    private Transform m_ThisTrf;
    public Transform thisTrf => m_ThisTrf;

    [SerializeField]
    private float ScaleSize = 0f;

    private Vector3 originScale;

    private void Awake()
    {
        if (m_ThisTrf == null)
        {
            m_ThisTrf = transform;
        }

        originScale = scaleLine.localScale;
    }

    public void Initialize(float duration)
    {
        if (shadowLine != null)
        {
            shadowTween.Kill();
        }

        if (scaleTween != null)
        {
            scaleTween.Kill();
        }

        shadowLine.color = color;
        scaleLine.localScale = originScale;

        shadowTween = shadowLine.DOColor(new Color(1f, 1f, 1f, 0f), duration).SetEase(Ease.InSine).From();
        scaleTween = scaleLine.DOScale(ScaleSize, duration).SetEase(Ease.Linear).From();
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
