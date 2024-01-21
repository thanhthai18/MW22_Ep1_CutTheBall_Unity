using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Addressable;
using Runtime.Definition;
using Runtime.Localization;
using Runtime.Manager.Data;
using Runtime.Manager.Game;
using Runtime.Manager.Toast;
using Runtime.Common.Singleton;
using Runtime.Manager.Pool;
using Runtime.PlayerManager;
using Runtime.SceneLoading;
using Runtime.UI;
using UnityEngine;
using UnityEngine.U2D;
using Runtime.Manager.Time;
using UnityEngine.AddressableAssets;

namespace Runtime.Manager.Initialization
{
    public class Initializer : MonoBehaviour, IProgress<float>
    {
        #region Members

        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private InitializerLoadingPanel _initializerLoadingPanel;
        private float _loadingValue;

        #endregion Members

        #region API Methods

        private void Start()
            => InitializeGameAsync().Forget();

        #endregion API Methods

        #region Class Methods

        public void Report(float value)
        {
            _loadingValue = value;
            _initializerLoadingPanel.UpdateLoading(_loadingValue);
        }

        private async UniTask InitializeGameAsync()
        {
            _initializerLoadingPanel.InitLoading(LoadScene);
            Application.targetFrameRate = _targetFrameRate;
            SpriteAtlasManager.atlasRequested += AddressablesManager.OnAtlasRequested;
            await AddressablesManager.UpdateCatalogsAsync(_loadingValue, 0.1f, this);
            Singleton.Of<PlayerService>().Init();
            Singleton.Of<PlayerService>().LoadPlayerFromLocal();
            LocalizationUtils.InitSelectedLocale();

            Report(0.99f);
            _initializerLoadingPanel.FinishLoading();

#if TRACKING
            MktTrackingManager.Instance.TrackFirstOpen(DataManager.Local.CurrentPlayerId);
#endif
        }

        private void LoadScene()
        {
            Destroy(gameObject);
            var loadedSceneName = SceneName.CUT_THE_BALL_SCENE_NAME;
            SceneManager.LoadSceneAsync(loadedSceneName).Forget();
        }

        #endregion Class Methods
    }
}