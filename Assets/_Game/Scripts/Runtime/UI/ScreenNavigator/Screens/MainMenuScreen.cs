using System;
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
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Shared.Views;

namespace Runtime.UI
{
    public partial class MainMenuScreen : Screen<ScreenData>
    {
        #region Members

        [Header("--- UI ---")]
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _skinsButton;
        [SerializeField] private Button _rulesButton;
        [SerializeField] private Button _startButton;

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
            Debug.Log("vai ca l");
            _settingsButton.onClick.AddListener(OnClickSettings);
            _skinsButton.onClick.AddListener(OnClickSkins);
            _rulesButton.onClick.AddListener(OnClickRules);
            _startButton.onClick.AddListener(OnClickStart);
        }
        
        public override UniTask CleanUp()
        {
            //_gameStateChangedMessageRegistry.Dispose();
            return base.CleanUp();
        }

        private void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            
        }

        private void OnClickSettings()
        {
            Debug.Log("Settings");
        }
        
        private void OnClickSkins()
        {
            Debug.Log("Skins");
        }
        
        private void OnClickRules()
        {
            Debug.Log("Rules");
        }
        
        private void OnClickStart()
        {
            Debug.Log("Start");
        }
        

        

        #endregion Class Methods
    }
}