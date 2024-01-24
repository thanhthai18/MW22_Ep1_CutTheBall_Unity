using Cysharp.Threading.Tasks;
using Runtime.Definition;
using Runtime.Localization;
using Runtime.Common.Singleton;
using Runtime.PlayerManager;
using Runtime.SceneLoading;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Manager.Initialization
{
    public class Initializer : MonoBehaviour
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

        private async UniTask InitializeGameAsync()
        {
            _initializerLoadingPanel.InitLoading(LoadScene);
            Application.targetFrameRate = _targetFrameRate;
            Singleton.Of<PlayerService>().Init();
            //LocalizationUtils.InitSelectedLocale();
            _initializerLoadingPanel.FinishLoading();
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