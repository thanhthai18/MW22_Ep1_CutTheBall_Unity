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
    public abstract class BaseGameplayFlowProcessor<EM, SM, LM> : MonoBehaviour where EM : EntitiesManager
                                                                                where SM : SplitManager
                                                                                where LM : LifeManager

    {
        #region Members

        protected static EM entitiesManager;
        protected static SM splitManager;
        protected static LM lifeManager;
        protected CameraManager cameraManager;
        protected CancellationTokenSource gameplayFlowCancellationTokenSource;
        protected MessageRegistry<GameStateChangedMessage> gameStateChangedMessageRegistry;

        #endregion Members

        #region Properties

        public static EM S_EntitiesManager => entitiesManager;
        public static SM S_SplitManager => splitManager;
        public static LM S_LifeManager => lifeManager;
        
        #endregion Properties

        #region API Methods

        protected virtual void Awake()
        {
            entitiesManager = EntitiesManager.Instance as EM;
            splitManager = SplitManager.Instance as SM;
            lifeManager = LifeManager.Instance as LM;
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
            switch (gameStateChangedMessage.GameStateEventType)
            {
                case GameStateEventType.GameFlowStopped:
                    //heroesControllerManager.HandleGameFlowStopped();
                    break;
            
                case GameStateEventType.DataLoaded:
                    GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
                    //S_SplitManager.HandleDataLoaded(gameplayFlowCancellationTokenSource.Token);
                    //S_EntitiesManager.HandleDataLoaded(gameplayFlowCancellationTokenSource.Token);
                    break;
            
                case GameStateEventType.BallSpawned:
                    break;
                
                case GameStateEventType.BoomSpawned:
                    break;
                
                case GameStateEventType.BallExplored:
                    break;
                
                case GameStateEventType.BoomExplored:
                    break;
            }
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