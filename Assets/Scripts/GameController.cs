using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Camera mainCamera;


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            float ranX = Random.Range(-GlobalValue.sizeCamera.x * 0.8f, GlobalValue.sizeCamera.x * 0.8f);
            var ball = BallController.instance.SpawnBall(BallController.instance.GetPosSpawn());
        }
    }

}



