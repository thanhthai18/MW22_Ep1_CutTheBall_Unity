using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMouse : MonoBehaviour
{
    private static EffectMouse Ins;

    public ParticleSystem eff;
    public Camera mainCam;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Ins != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            eff.transform.position = mousePos;
            eff.Stop();
            eff.Play();
        }
    }
}
