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
    public sealed class WorldGameplayFlowProcessor : BaseGameplayFlowProcessor<WorldEntitiesManager,
                                                                               WorldGameResourceManager>
    {
        #region Members

        [SerializeField]
        private int _worldMapIndex;
        private TimeManager _worldMapTimeManager;
        //private MessageRegistry<ChangeRestStateMessage> _changeRestStateMessageRegistry;

        #endregion Members

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            //DataManager.Transitioned.GoneToWorldMapIndex = _worldMapIndex;
            _worldMapTimeManager = TimeManager.Instance;
            //_changeRestStateMessageRegistry = Messenger.Subscribe<ChangeRestStateMessage>(OnChangeRestState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //_changeRestStateMessageRegistry.Dispose();
        }

        #endregion API Methods

        #region Class Methods

        protected override void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            base.OnGameStateChanged(gameStateChangedMessage);
            // switch (gameStateChangedMessage.GameStateEventType)
            // {
            //     case GameStateEventType.DataLoaded:
            //         _worldMapTimeManager.HandleDataLoaded();
            //         DataManager.Local.ResetResourceUnlockBase();
            //         var battlePassUtil = Singleton.Of<BattlePassUtil>();
            //         if (battlePassUtil.IsReset())
            //             DataManager.Local.ResetBattlePass(battlePassUtil.CurrentSeason());
            //         DataManager.Local.CheckPremiumReward();
            //         break;
            //
            //     case GameStateEventType.HeroTeamPickUpdated:
            //         mapManager.HandleHeroTeamPickUpdated(gameplayFlowCancellationTokenSource.Token);
            //         break;
            //
            //     case GameStateEventType.NewDayReset:
            //         mapManager.HandleNewDayReset(gameplayFlowCancellationTokenSource.Token);
            //         break;
            //
            //     case GameStateEventType.ReviveMapTriggered:
            //         mapManager.HandleReviveMapTriggered(gameplayFlowCancellationTokenSource.Token);
            //         GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
            //         break;
            //
            //     case GameStateEventType.GiveUpWorldMapTriggered:
            //         mapManager.HandleGiveUpMapTriggered(gameplayFlowCancellationTokenSource.Token);
            //         GameManager.Instance.SetGameMomentType(GameMomentType.StartGame);
            //         break;
            //
            //     case GameStateEventType.CampCleanUp:
            //         var playResult = PlayResult.None;
            //         entitiesManager.HandleCampCleanUp(gameplayFlowCancellationTokenSource.Token, out playResult);
            //         switch (playResult)
            //         {
            //             case PlayResult.LostGame:
            //                 HandleGameLost();
            //                 break;
            //         }
            //         break;
            // }
        }

        #endregion Class Methods
    }
}