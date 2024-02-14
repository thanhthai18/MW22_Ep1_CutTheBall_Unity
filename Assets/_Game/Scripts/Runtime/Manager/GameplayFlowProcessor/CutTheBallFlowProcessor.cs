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

namespace Runtime.Gameplay.Manager
{
    public sealed class CutTheBallFlowProcessor : BaseGameplayFlowProcessor<EntitiesManager,
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

        protected override void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            base.OnGameStateChanged(gameStateChangedMessage);
            switch (gameStateChangedMessage.GameStateEventType)
            {
                case GameStateEventType.DataLoaded:
                    //_worldMapTimeManager.HandleDataLoaded();
                    S_LifeManager.HandleDataLoaded();
                    SpawnEntities().Forget();
                    break;

                // case GameStateEventType.HeroTeamPickUpdated:
                //     mapManager.HandleHeroTeamPickUpdated(gameplayFlowCancellationTokenSource.Token);
                //     break;
                //
                // case GameStateEventType.NewDayReset:
                //     mapManager.HandleNewDayReset(gameplayFlowCancellationTokenSource.Token);
                //     break;
                //
                // case GameStateEventType.ReviveMapTriggered:
                //     mapManager.HandleReviveMapTriggered(gameplayFlowCancellationTokenSource.Token);
                //     GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
                //     break;
                //
                // case GameStateEventType.GiveUpWorldMapTriggered:
                //     mapManager.HandleGiveUpMapTriggered(gameplayFlowCancellationTokenSource.Token);
                //     GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
                //     break;
                //
                // case GameStateEventType.CampCleanUp:
                //     var playResult = PlayResult.None;
                //     entitiesManager.HandleCampCleanUp(gameplayFlowCancellationTokenSource.Token, out playResult);
                //     switch (playResult)
                //     {
                //         case PlayResult.LostGame:
                //             HandleGameLost();
                //             break;
                //     }
                //     break;
            }
        }

        private async UniTask SpawnEntities()
        {
            while (S_LifeManager.IsAlive() && !_gameEnd)
            {
                await UniTask.Delay(_ballSpawnIntervalInMilisecond, cancellationToken: gameplayFlowCancellationTokenSource.Token);
                EntityType entityType = Random.value < _bombSpawnChance ? EntityType.Boom : EntityType.Ball;
                S_EntitiesManager.CreateDefaultEntityAsync(entityType, Vector2.zero, gameplayFlowCancellationTokenSource.Token);
            }
        }

        #endregion Class Methods
    }
}