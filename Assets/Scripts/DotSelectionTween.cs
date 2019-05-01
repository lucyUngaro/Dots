using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// A tween that will play whenever a dot is selected and then destroy itself
/// </summary>
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
        transform.position = dot.transform.position;
        TweenColor(clonedDot.GetColor());
        TweenScale();
    }

    // Tween alpha and then delete
    private void TweenColor(Color color)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = color; ;
        sr.DOFade(0f, 0.4f).OnComplete(()=>Destroy(gameObject));
    }

    // Tween scale
    private void TweenScale()
    {
        float originalScale = transform.localScale.x;
        transform.localScale = new Vector2(0f, 0f);
        transform.DOScale(originalScale * 2f, 0.25f).SetEase(Ease.OutQuad);
    }
}
