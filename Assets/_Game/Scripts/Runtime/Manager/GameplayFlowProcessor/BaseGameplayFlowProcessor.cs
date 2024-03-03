using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Runtime.Message;
using Runtime.Definition;
using Runtime.SceneLoading;
using Runtime.Manager.Game;
using Runtime.Gameplay.EntitySystem;
using Random = UnityEngine.Random;

namespace Runtime.Gameplay.Manager
{
    public class BaseGameplayFlowProcessor : MonoBehaviour
    {
        #region Members

        protected static EntitiesManager entitiesManager;
        protected static SplitManager splitManager;
        protected static LifeManager lifeManager;
        protected CameraManager cameraManager;
        protected CancellationTokenSource gameplayFlowCancellationTokenSource;
        private MessageRegistry<GameStateChangedMessage> _gameStateChangedMessageRegistry;
        
        private bool _gameEnd;
        private readonly int _ballSpawnIntervalInMilisecond = 1000;
        private readonly float _bombSpawnChance = 0.2f;

        #endregion Members

        #region Properties
        
        public GameStateEventType State { get; protected set; }
        public static EntitiesManager EntitiesManager => entitiesManager;
        public static SplitManager SplitManager => splitManager;
        public static LifeManager LifeManager => lifeManager;
        
        #endregion Properties

        #region API Methods

        protected virtual void Awake()
        {
            entitiesManager = EntitiesManager.Instance;
            splitManager = SplitManager.Instance;
            lifeManager = LifeManager.Instance;
            cameraManager = CameraManager.Instance;
            gameplayFlowCancellationTokenSource = new CancellationTokenSource();
            _gameStateChangedMessageRegistry = Messenger.Subscribe<GameStateChangedMessage>(OnGameStateChanged);
            SceneManager.RegisterBeforeChangeScene(OnBeforeChangeScene);
        }

        protected virtual void OnDestroy()
        {
            gameplayFlowCancellationTokenSource.Cancel();
            gameplayFlowCancellationTokenSource.Dispose();
            _gameStateChangedMessageRegistry.Dispose();
        }

        #endregion API Methods

        #region Class Methods

        protected virtual void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            var newState = gameStateChangedMessage.GameStateEventType;
            State = newState;
            switch (newState) {
                case GameStateEventType.GameFlowStopped:
                    //heroesControllerManager.HandleGameFlowStopped();
                    break;
            
                case GameStateEventType.DataLoaded:
                    GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
                    //S_SplitManager.HandleDataLoaded(gameplayFlowCancellationTokenSource.Token);
                    //S_EntitiesManager.HandleDataLoaded(gameplayFlowCancellationTokenSource.Token);
                    //_worldMapTimeManager.HandleDataLoaded();
                    LifeManager.HandleDataLoaded();
                    SpawnEntities().Forget();
                    break;
                
            
                case GameStateEventType.BallSpawned:
                    break;
                
                case GameStateEventType.BoomSpawned:
                    break;
                
                case GameStateEventType.BallExplored:
                    break;
                
                case GameStateEventType.BoomExplored:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        protected virtual async UniTask SpawnEntities()
        {
            while (LifeManager.IsAlive() && !_gameEnd)
            {
                await UniTask.Delay(_ballSpawnIntervalInMilisecond, cancellationToken: gameplayFlowCancellationTokenSource.Token);
                EntityType entityType = Random.value < _bombSpawnChance ? EntityType.Boom : EntityType.Ball;
                var startPos = Constant.GetRandomStartPosition();
                var destinationPos = Constant.GetRandomStartPosition();
                EntitiesManager.CreateDefaultEntityAsync(entityType, startPos, destinationPos, gameplayFlowCancellationTokenSource.Token);
            }
        }
            
 
        // protected virtual void OnObjectDestroyed(ObjectDestroyedMessage objectDestroyedMessage)
        // {
        //     entitiesManager.HandleObjectDestroyed(objectDestroyedMessage, gameplayFlowCancellationTokenSource.Token);
        //     gameResourceManager.HandleObjectDestroyed(objectDestroyedMessage, gameplayFlowCancellationTokenSource.Token);
        // }
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