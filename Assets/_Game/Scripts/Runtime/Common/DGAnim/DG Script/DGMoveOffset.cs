using DG.Tweening;
using UnityEngine;

public class DGMoveOffset : DGUtilis
{
    public Vector3 offset;
    public override void Play()
    {
        transform.DOMove(offset+transform.position, duration, snapping).OnComplete(() => onComplete?.Invoke()).SetLoops(loops, loopType).SetEase(ease);
    }
}

