using Cysharp.Threading.Tasks;
using Runtime.Manager.Data;
using Runtime.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Camera mainCamera;
    public MessageRegistry<GameStateChangedMessage> gameStateChangedMessageRegistry;
    private CancellationTokenSource _cancellationTokenSource;

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

    private void OnEnable()
    {
        gameStateChangedMessageRegistry = Messenger.Subscribe<GameStateChangedMessage>(OnGameStateChangedMessageHandle);
    }
    private void OnDisable()
    {
        gameStateChangedMessageRegistry.Dispose();
        _cancellationTokenSource.Dispose();
    }

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        mainCamera = Camera.main;
        SetSizeCamera();

        //onClickButtonStart:
        eventStartGame.Invoke();
        LoadHitObjectConfig().Forget();
    }

    private async UniTaskVoid LoadHitObjectConfig()
    {
        await DataManager.Config.LoadHitObjectConfig(_cancellationTokenSource.Token);
    }

    private void OnGameStateChangedMessageHandle(GameStateChangedMessage mess)
    {
        // if(mess.GameStateEventType == Runtime.Definition.GameStateEventType.AddQuestValue)
        // {
        //     Debug.Log("oke");
        //     var tmp = DataManager.Config.GetHitObjectInfo().items;
        //     Debug.Log(tmp[0].valueAddScore);
        // }
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



