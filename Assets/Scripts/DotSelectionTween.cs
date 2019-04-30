using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotSelectionTween : MonoBehaviour
{
    Dot clonedDot;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("BackgroundEffects");
    }

    public void StartTween(Dot dot)
    {
        clonedDot = dot;
        dot.currentSelectionTween = this;
        transform.position = dot.transform.position;
        TweenColor(clonedDot.GetColor());
        TweenScale();
    }

    // Set to the same color as the dot
    private void TweenColor(Color color)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = color; ;
        sr.DOFade(0f, 0.4f).OnComplete(SelfDestruct);
    }

    // Tween scale, then delete
    private void TweenScale()
    {
        float originalScale = transform.localScale.x;
        transform.localScale = new Vector2(0f, 0f);
        transform.DOScale(originalScale * 2f, 0.25f).SetEase(Ease.OutQuad);
    }

    private void SelfDestruct()
    {
        clonedDot.currentSelectionTween = null;
        Destroy(gameObject);
    }
}
