using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DGUtilis : MonoBehaviour
{
    public bool playOnStart;
    public float duration = 1;
    public bool snapping;
    public int loops;
    public LoopType loopType;
    public Ease ease;
    public UnityEvent onComplete;

    protected virtual void Start()
    {
        if(playOnStart) Play();
    }

    public abstract void Play();
}
