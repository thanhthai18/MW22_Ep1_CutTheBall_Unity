using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using Runtime.Common.Singleton;
using Cysharp.Threading.Tasks;
using Runtime.Definition;
using Runtime.Localization;
using Runtime.Message;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Runtime.SceneLoading
{
    [Serializable]
    public struct SceneLoaderData
    {
        #region Members

        public List<SceneLoadingInfo> scenesLoadingInfo;

        #endregion Members
    }

    public class SceneManager : PersistentMonoSingleton<SceneManager>
    {
        #region Members

        private static bool s_isLoadingScene;
        [SerializeField] private SceneLoaderData _sceneLoaderData;
        [SerializeField] private float _sceneLoadingProgressSmoothSpeed = 3.0f;
        [SerializeField] private float _mainLoadScale = 1.0f;
        [SerializeField] private float _progressBarRoundValue = 0;
        [SerializeField] private SceneLoaderScreen _sceneLoaderScreen;
        [SerializeField] private SceneFakeLoaderScreen _sceneFakeLoaderScreen;
        private bool _hasFinishedLoading;
        private float _sceneLoadingProgressSmoothValue;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion Members

        #region Properties

        private float DeltaTime => Time.unscaledDeltaTime;

        /// <summary>
        /// The scene loading info of the scene to be opened.
        /// </summary>
        public SceneLoadingInfo SceneLoadingInfo { get; private set; }

        /// <summary>
        /// The action that will be executed when there is a trigger that will change scene.
        /// </summary>
        private static Action TriggerChangeSceneAction { get; set; }

        /// <summary>
        /// The action that will be executed before the scene loading process takes place.
        /// </summary>
        private static Action BeforeChangeSceneAction { get; set; }

        /// <summary>
        /// The tasks that need to be completed after the old scene has been unloaded, the new scene has been loaded,
        /// but before the new scene is about to appear (before the fade-in takes place).
        /// </summary>
        private static List<Func<CancellationToken, UniTask>> CompletedTaskBeforeNewSceneAppearedTasks { get; set; }

        /// <summary>
        /// The tasks that need to be waited after all of the items in the list CompleteTaskBeforeNewSceneAppearedTasks completed.
        /// </summary>
        private static List<Func<CancellationToken, UniTask>> WaitedTaskBeforeNewSceneAppearedTasks { get; set; }

        #endregion Properties

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            s_isLoadingScene = false;
            _hasFinishedLoading = false;
            _sceneLoadingProgressSmoothValue = 0.0f;
        }

        private void OnEnable()
            => UnitySceneManager.activeSceneChanged += OnActiveSceneChanged;

        private void OnDisable()
            => UnitySceneManager.activeSceneChanged -= OnActiveSceneChanged;

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #endregion API Methods

        #region Class Methods

        public static void ShowFirstSession(Action callback)
        {
            Instance._sceneLoaderScreen.FadeInFirstSession(callback);
        }

        public static async UniTask LoadSceneAsync(string sceneName)
        {
            if (s_isLoadingScene)
                return;
            s_isLoadingScene = true;
            await Instance.StartInternalLoadSceneAsync(sceneName);
            s_isLoadingScene = false;
        }

        public static async UniTask<SceneInstance> LoadAdditiveSceneAsyncOperation(string additiveSceneName)
            => await Addressables.LoadSceneAsync(additiveSceneName, LoadSceneMode.Additive);

        public static async UniTask UnloadAdditiveSceneAsyncOperation(SceneInstance sceneInstance)
            => await Addressables.UnloadSceneAsync(sceneInstance);


        /// <summary>
        /// Load a fake scene with an attached task.
        /// </summary>
        /// <param name="loadSceneTask">The task executed in the middle of the fake loading process.</param>
        /// <param name="loadSceneTaskDelayInMilliseconds">The delay for the task.</param>
        public static async UniTask LoadFakeSceneAsync(Func<UniTask> loadSceneTask = null, int loadSceneTaskDelayInMilliseconds = 500)
            => await Instance.StartInternalSceneFakeLoadAsync(loadSceneTask, loadSceneTaskDelayInMilliseconds);

        public static void RegisterTriggerChangeScene(Action triggerChangeSceneAction)
            => TriggerChangeSceneAction += triggerChangeSceneAction;

        public static void RegisterBeforeChangeScene(Action beforeChangeSceneAction)
            => BeforeChangeSceneAction += beforeChangeSceneAction;

        public static void RegisterCompletedTaskBeforeNewSceneAppeared(Func<CancellationToken, UniTask> completedTaskBeforeNewSceneAppearedTask)
        {
            if (CompletedTaskBeforeNewSceneAppearedTasks == null)
                CompletedTaskBeforeNewSceneAppearedTasks = new List<Func<CancellationToken, UniTask>>();
            CompletedTaskBeforeNewSceneAppearedTasks.Add(completedTaskBeforeNewSceneAppearedTask);
        }

        public static void RegisterWaitedTaskBeforeNewSceneAppeared(Func<CancellationToken, UniTask> waitedTaskBeforeNewSceneAppearedTask)
        {
            if (WaitedTaskBeforeNewSceneAppearedTasks == null)
                WaitedTaskBeforeNewSceneAppearedTasks = new List<Func<CancellationToken, UniTask>>();
            WaitedTaskBeforeNewSceneAppearedTasks.Add(waitedTaskBeforeNewSceneAppearedTask);
        }

        private async UniTask StartInternalLoadSceneAsync(string sceneName)
        {
            if (TriggerChangeSceneAction != null)
            {
                TriggerChangeSceneAction.Invoke();
                TriggerChangeSceneAction = null;
            }

            var sceneOpenedLoadingInfo = GetSceneLoadingInfo(sceneName);
            SceneLoadingInfo = sceneOpenedLoadingInfo;
            _sceneLoaderScreen.UpdateSceneLoadingInfo(SceneLoadingInfo);
            await StartSceneLoadingAsyncOperation();
        }

        private async UniTask StartInternalSceneFakeLoadAsync(Func<UniTask> loadSceneTask, int loadSceneTaskDelayInMilliseconds)
            => await _sceneFakeLoaderScreen.RunSceneFakeLoadAsync(loadSceneTask, loadSceneTaskDelayInMilliseconds);


        private async UniTask StartSceneLoadingAsyncOperation()
        {
            await _sceneLoaderScreen.PrepareLoadingSceneAsync();

            if (BeforeChangeSceneAction != null)
            {
                BeforeChangeSceneAction.Invoke();
                BeforeChangeSceneAction = null;
            }

            await UniTask.Yield(PlayerLoopTiming.Update);

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();

            var sceneLoadingAsyncOperation = Addressables.LoadSceneAsync(SceneLoadingInfo.LoadedSceneName);
            var currentLoadingTime = 0.0f;
            while (!sceneLoadingAsyncOperation.IsDone)
            {
                var status = sceneLoadingAsyncOperation.GetDownloadStatus();
                float completeProgress = Mathf.Clamp01(status.Percent);
                _sceneLoadingProgressSmoothValue = Mathf.Lerp(_sceneLoadingProgressSmoothValue, completeProgress, DeltaTime * _sceneLoadingProgressSmoothSpeed);
                float progressBarValue = _progressBarRoundValue > 0
                    ? (Mathf.Round(_sceneLoadingProgressSmoothValue / _progressBarRoundValue) * _progressBarRoundValue)
                    : _sceneLoadingProgressSmoothValue;
                progressBarValue = progressBarValue * _mainLoadScale;
                _sceneLoaderScreen.UpdateLoadProgress(progressBarValue);
                currentLoadingTime += DeltaTime;
                if (completeProgress >= 1)
                {
                    if (currentLoadingTime > SceneLoadingInfo.sceneLoadingMinTime)
                    {
                        if (!_hasFinishedLoading)
                        {
                            _sceneLoaderScreen.UpdateLoadProgress(1);
                            _hasFinishedLoading = true;
                        }
                    }
                    else await UniTask.Yield(PlayerLoopTiming.Update);
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            _sceneLoaderScreen.UpdateLoadProgress(1);
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
            => ResetWhenSceneChanged();

        private void ResetWhenSceneChanged()
        {
            _hasFinishedLoading = false;
            _sceneLoadingProgressSmoothValue = 0.0f;
            RunCompletedAndWaitedTasksBeforeSceneAppearAsync().Forget();
        }

        private async UniTask RunCompletedAndWaitedTasksBeforeSceneAppearAsync()
        {
            if (CompletedTaskBeforeNewSceneAppearedTasks != null)
            {
                while (CompletedTaskBeforeNewSceneAppearedTasks.Count > 0)
                {
                    await CompletedTaskBeforeNewSceneAppearedTasks[0].Invoke(_cancellationTokenSource.Token);
                    CompletedTaskBeforeNewSceneAppearedTasks.RemoveAt(0);
                }

                CompletedTaskBeforeNewSceneAppearedTasks = null;
            }
            else await UniTask.CompletedTask;

            if (WaitedTaskBeforeNewSceneAppearedTasks != null)
            {
                while (WaitedTaskBeforeNewSceneAppearedTasks.Count > 0)
                {
                    await WaitedTaskBeforeNewSceneAppearedTasks[0].Invoke(_cancellationTokenSource.Token);
                    WaitedTaskBeforeNewSceneAppearedTasks.RemoveAt(0);
                }

                WaitedTaskBeforeNewSceneAppearedTasks = null;
            }
            else await UniTask.CompletedTask;

            await UniTask.Yield(PlayerLoopTiming.Update);

            _sceneLoaderScreen.ResetWhenSceneChanged();
        }

        private SceneLoadingInfo GetSceneLoadingInfo(string sceneName)
        {
            foreach (SceneLoadingInfo sceneLoadingInfo in _sceneLoaderData.scenesLoadingInfo)
            {
                if (sceneName.StartsWith(sceneLoadingInfo.sceneStartNameFormat))
                {
                    sceneLoadingInfo.UpdateLoadedSceneName(sceneName);
                    return sceneLoadingInfo;
                }
            }

            return null;
        }

        #endregion Class Methods
    }
}