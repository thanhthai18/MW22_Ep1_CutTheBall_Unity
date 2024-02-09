using System.Threading;
using UnityEngine;
using Runtime.Message;
using Runtime.Definition;
using Runtime.SceneLoading;
using Runtime.Gameplay.Map;
using Runtime.Manager.Game;
using Runtime.Common.Singleton;
using Runtime.Gameplay.EntitySystem;

namespace Runtime.Gameplay.Manager
{
    public abstract class BaseGameplayFlowProcessor<EM, GM, MM> : MonoBehaviour where EM : EntitiesManager
                                                                                where GM : GameResourceManager
                                                                                where MM : MapManager

    {
        #region Members

        protected EM entitiesManager;
        protected GM gameResourceManager;
        protected MM mapManager;
        protected CameraManager cameraManager;
        protected CancellationTokenSource gameplayFlowCancellationTokenSource;
        protected MessageRegistry<GameStateChangedMessage> gameStateChangedMessageRegistry;

        #endregion Members

        #region API Methods

        protected virtual void Awake()
        {
            entitiesManager = EntitiesManager.Instance as EM;
            gameResourceManager = GameResourceManager.Instance as GM;
            mapManager = MapManager.Instance as MM;
            cameraManager = CameraManager.Instance;
            gameplayFlowCancellationTokenSource = new CancellationTokenSource();
            gameStateChangedMessageRegistry = Messenger.Subscribe<GameStateChangedMessage>(OnGameStateChanged);
            SceneManager.RegisterBeforeChangeScene(OnBeforeChangeScene);
        }

        protected virtual void OnDestroy()
        {
            gameplayFlowCancellationTokenSource.Cancel();
            gameplayFlowCancellationTokenSource.Dispose();
            gameStateChangedMessageRegistry.Dispose();
        }

        #endregion API Methods

        #region Class Methods

        // protected virtual void OnObjectDestroyed(ObjectDestroyedMessage objectDestroyedMessage)
        // {
        //     entitiesManager.HandleObjectDestroyed(objectDestroyedMessage, gameplayFlowCancellationTokenSource.Token);
        //     gameResourceManager.HandleObjectDestroyed(objectDestroyedMessage, gameplayFlowCancellationTokenSource.Token);
        // }

        protected virtual void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            // switch (gameStateChangedMessage.GameStateEventType)
            // {
            //     case GameStateEventType.GameFlowStopped:
            //         heroesControllerManager.HandleGameFlowStopped();
            //         break;
            //
            //     case GameStateEventType.DataLoaded:
            //         GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
            //         cameraManager.HandleDataLoaded(heroesControllerManager.HeroesGroupCenterHolder);
            //         mapManager.HandleDataLoaded(gameplayFlowCancellationTokenSource.Token);
            //         InitQuestOnHandleDataLoaded();
            //         break;
            //
            //     case GameStateEventType.HeroSpawned:
            //         heroesControllerManager.HandleHeroesSpawned();
            //         navigationManager.HandleHeroesSpawned();
            //         proximityManager.HandleHeroesSpawned();
            //         cameraManager.HandleHeroesSpawned();
            //         break;
            // }
        }

        // protected virtual void OnGateEventTriggered(GateEventTriggeredMessage gateEventTriggeredMessage)
        //     => mapManager.HandleGateEventTriggered(gateEventTriggeredMessage);
        //
        // protected virtual void OnGameActionNotified(GameActionNotifiedMessage gameActionNotifiedMessage)
        //     => questManager.HandleQuestChanged(gameActionNotifiedMessage);
        //
        // protected virtual void OnHeroesControllingStatusUpdated(HeroesControllingStatusUpdatedMessage heroesControllingStatusUpdatedMessage)
        //     => cameraManager.HandleHeroesControlingStatusUpdated(heroesControllingStatusUpdatedMessage);
        //
        // protected virtual void HandleGameLost()
        // {
        //     GameManager.Instance.SetGameMomentType(GameMomentType.LostGame);
        //     Messenger.Publish(new GameStateChangedMessage(GameStateEventType.GameLost));
        // }
        //
        // protected virtual void InitQuestOnHandleDataLoaded()
        //     => questManager.HandleDataLoaded();

        protected virtual void OnBeforeChangeScene()
        {
            var disposableEntities = FindObjectsOfType<Disposable>(true);
            foreach (var disposableEntity in disposableEntities)
                disposableEntity.Dispose();
        }

        #endregion Class Methods
    }
}