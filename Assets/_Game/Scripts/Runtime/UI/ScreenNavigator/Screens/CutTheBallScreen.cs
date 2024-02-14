using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.AssetLoader;
using Runtime.Common.Singleton;
using Runtime.Config;
using Runtime.Definition;
using Runtime.Gameplay.Manager;
using Runtime.Localization;
using Runtime.Manager.Data;
using Runtime.Manager.Pool;
using Runtime.Manager.Time;
using Runtime.Manager.Toast;
using Runtime.Message;
using Runtime.PlayerManager;
using Runtime.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Shared.Views;

namespace Runtime.UI
{
    public partial class CutTheBallScreen : Screen<ScreenData>
    {
        #region Members

        [Header("--- UI ---")]
    

        private MessageRegistry<GameStateChangedMessage> _gameStateChangedMessageRegistry;

        #endregion Members

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            // LocalizationSettings.SelectedLocaleChanged += ChangedLocaleEvent;
            //_gameStateChangedMessageRegistry = Messenger.Subscribe<GameStateChangedMessage>(OnGameStateChanged);
        }

        #endregion API Methods

        #region Class Methods

        public override async UniTask Initialize(ScreenData screenData)
        {
            await base.Initialize(screenData);
         
        }
        
        public override UniTask CleanUp()
        {
            //_gameStateChangedMessageRegistry.Dispose();
            return base.CleanUp();
        }

        private void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            
        }

        #endregion Class Methods
    }
}