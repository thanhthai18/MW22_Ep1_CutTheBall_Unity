using System;
using UnityEngine;
using Runtime.Definition;
using Runtime.Gameplay.Map;
using Runtime.Message;
using Runtime.Common.Singleton;
using Runtime.Manager.Data;
using Runtime.Manager.Game;
using Runtime.Manager.Time;
using Cysharp.Threading.Tasks;
using Runtime.UI;
using UnityScreenNavigator.Runtime.Core.Shared.Views;
using Random = UnityEngine.Random;

namespace Runtime.Gameplay.Manager
{
    public class CutTheBallFlowProcessor : BaseGameplayFlowProcessor<EntitiesManager,
                                                                     SplitManager,
                                                                     LifeManager>
    
    {
        #region Members

        //private TimeManager _worldMapTimeManager;
        private bool _gameEnd;
        private readonly int _ballSpawnIntervalInMilisecond = 1000;
        private readonly float _bombSpawnChance = 0.2f;

        #endregion Members

        #region API Methods

        public override GameStateEventType State { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            _gameEnd = false;
            //_worldMapTimeManager = TimeManager.Instance;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #endregion API Methods

        #region Class Methods

        public override void ChangeState(GameStateEventType newState) {
            
            Messenger.Publish(new GameStateChangedMessage(newState));

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

            Messenger.Publish(new GameStateChangedMessage(newState));
        
            Debug.Log($"New state: {newState}");
        }
      
        protected virtual async UniTask SpawnEntities()
        {
            while (LifeManager.IsAlive() && !_gameEnd)
            {
                await UniTask.Delay(_ballSpawnIntervalInMilisecond, cancellationToken: gameplayFlowCancellationTokenSource.Token);
                EntityType entityType = Random.value < _bombSpawnChance ? EntityType.Boom : EntityType.Ball;
                EntitiesManager.CreateDefaultEntityAsync(entityType, Vector2.zero, gameplayFlowCancellationTokenSource.Token);
            }
        }

        #endregion Class Methods
    }
}