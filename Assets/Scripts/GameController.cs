using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Camera mainCamera;


    //Event
    public static event Action eventStartGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance.gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        SetSizeCamera();

        //onClickButtonStart:
        eventStartGame.Invoke();


    }



    void SetSizeCamera()
    {
        float f1, f2;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;
        if (f1 > f2)
        {
            mainCamera.orthographicSize *= f1 / f2;
        }
        GlobalValue.sizeCamera = new Vector2(mainCamera.orthographicSize * 2 * Screen.width * 1.0f / Screen.height, mainCamera.orthographicSize * 2);
    }

}



