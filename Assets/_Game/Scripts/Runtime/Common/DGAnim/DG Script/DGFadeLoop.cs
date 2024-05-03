using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DGFadeLoop : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDisable()
    {
        Color baseColor=spriteRenderer.color;
        baseColor.a = 255f;
        spriteRenderer.color = baseColor;
    }
}
