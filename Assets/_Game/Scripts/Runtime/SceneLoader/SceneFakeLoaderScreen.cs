using System;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Extensions;
using Cysharp.Threading.Tasks;

namespace Runtime.SceneLoading
{
    public class SceneFakeLoaderScreen : MonoBehaviour
    {
        #region Members

        [SerializeField]
        private Image _fadeImage;
        [SerializeField]
        private float _fadeDuration;
        [SerializeField]
        private CanvasGroup _fakeLoadingPanelCanvasGroup;

        #endregion Members

        #region Class Methods

        public async UniTask RunSceneFakeLoadAsync(Func<UniTask> fakeLoadSceneTask, int fakeLoadSceneDelayInMilliseconds)
        {
            _fakeLoadingPanelCanvasGroup.SetActiveWithoutAlpha(true);
            _fakeLoadingPanelCanvasGroup.alpha = 0.0f;

            var currentFadeDuration = 0.0f;
            while (currentFadeDuration < _fadeDuration)
            {
                currentFadeDuration += Time.unscaledDeltaTime;
                _fakeLoadingPanelCanvasGroup.alpha = currentFadeDuration / _fadeDuration;
                await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            }

            if (fakeLoadSceneTask != null)
                await fakeLoadSceneTask.Invoke();
            else
                await UniTask.CompletedTask;
            await UniTask.Delay(fakeLoadSceneDelayInMilliseconds, ignoreTimeScale: true, cancellationToken: this.GetCancellationTokenOnDestroy());

            _fadeImage.gameObject.SetActive(true);
            _fadeImage.canvasRenderer.SetAlpha(1.0f);
            _fakeLoadingPanelCanvasGroup.SetActive(false);

            currentFadeDuration = 0.0f;
            while (currentFadeDuration < _fadeDuration)
            {
                currentFadeDuration += Time.unscaledDeltaTime;
                _fadeImage.canvasRenderer.SetAlpha(1.0f - currentFadeDuration / _fadeDuration);
                await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            }

            _fadeImage.canvasRenderer.SetAlpha(0.0f);
            _fadeImage.gameObject.SetActive(false);
            await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
        }

        #endregion Class Methods
    }
}