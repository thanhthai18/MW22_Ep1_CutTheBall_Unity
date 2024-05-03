using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DGImageFade : DGUtilis
{
    [Range(0f, 255f)]
    public float fadeValue;
    public Image image;
    public override void Play()
    {
        image.DOFade(fadeValue, duration).OnComplete(()=>onComplete?.Invoke()).SetLoops(loops).SetEase(ease);
    }
}

