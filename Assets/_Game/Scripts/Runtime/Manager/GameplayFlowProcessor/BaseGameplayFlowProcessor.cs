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
        
        private bool _gameEnd;
        private readonly int _ballSpawnIntervalInMilisecond = 1000;
        private readonly float _bombSpawnChance = 0.2f;

        #endregion Members

        #region Properties
        
        public abstract GameStateEventType State { get; protected set; }
        public static EM EntitiesManager => entitiesManager;
        public static SM SplitManager => splitManager;
        public static LM LifeManager => lifeManager;
        
        #endregion Properties

        #region API Methods

        protected virtual void Awake()
        {
            entitiesManager = Manager.EntitiesManager.Instance as EM;
            splitManager = Manager.SplitManager.Instance as SM;
            lifeManager = Manager.LifeManager.Instance as LM;
            cameraManager = CameraManager.Instance;
            gameplayFlowCancellationTokenSource = new CancellationTokenSource();
            SceneManager.RegisterBeforeChangeScene(OnBeforeChangeScene);
        }

        protected virtual void OnDestroy()
        {
            gameplayFlowCancellationTokenSource.Cancel();
            gameplayFlowCancellationTokenSource.Dispose();
        }

        #endregion API Methods

        #region Class Methods

        public abstract void ChangeState(GameStateEventType newState);
            
 
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