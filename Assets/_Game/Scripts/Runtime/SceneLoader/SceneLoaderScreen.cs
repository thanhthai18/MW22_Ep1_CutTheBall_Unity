using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Tool.Easing;
using TMPro;
using UnityRandom = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Runtime.SceneLoading
{
    public class SceneLoaderScreen : MonoBehaviour
    {
        #region Members

        [Header("=== SETTINGS ===")]
        [SerializeField]
        private float _screenInitialFadeInSpeed = 1.0f;
        [SerializeField]
        private float _startLoadSceneDelay = 1.0f;
        [SerializeField]
        private bool _canChangeBackground;
        [SerializeField]
        private float _backgroundShowTime = 2.0f;
        [SerializeField]
        private float _changeBackgroundDelay = 0.5f;
        [SerializeField]
        private float _backgroundFadeSpeed = 2.0f;
        [SerializeField]
        private float _tipShowTime = 5.0f;
        [SerializeField]
        private float _changeTipDelay = 0.1f;
        [SerializeField]
        private float _tipFadeSpeed = 2.0f;
        [SerializeField]
        [TextArea(2, 2)]
        private string _progressTextFormat = "{0}%";

        [Header("=== UI ELEMENTS ===")]
        [SerializeField]
        private TextMeshProUGUI _progressText;
        [SerializeField]
        private TextMeshProUGUI _tipText;
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private Slider _progressBarSlider;
        [SerializeField]
        private CanvasGroup _backgroundCanvasGroup;
        [SerializeField]
        private CanvasGroup _fadeImageCanvas;
        [SerializeField]
        private CanvasGroup _rootCanvasGroup;

        [SerializeField]
        private TMP_Text firstSessionText;
        [SerializeField]
        private Button firstSessionObjectButton;
        [SerializeField]
        private GameObject tapToContinue;
        [SerializeField]
        private GameObject loadingObject;
        
        private SceneLoadingInfo _sceneLoadingInfo;
        private List<string> _cacheSceneLoadingTips;
        private List<Sprite> _cacheBackgrounds;
        private bool _isTipFadeIn = false;
        private int _currentTipIndex = 0;
        private int _currentBackgroundIndex = 0;

        
        #endregion Members

        #region Properties

        private float DeltaTime => Time.unscaledDeltaTime;

        #endregion Properties

        #region API Methods

        private void Awake()
        {
            _progressBarSlider.value = 0.0f;
            _fadeImageCanvas.alpha = 1;
            _fadeImageCanvas.transform.SetParent(_rootCanvasGroup.transform.parent, false);
            _fadeImageCanvas.transform.SetAsLastSibling();
            firstSessionObjectButton.transform.SetAsLastSibling();
            _rootCanvasGroup.gameObject.SetActive(false);
            firstSessionObjectButton.onClick.AddListener(FadeOutFirstSession);
        }

        #endregion API Methods

        #region Class Methods

        public void UpdateLoadProgress(float value)
        {
            string percent = (value * 100).ToString("F0");
            _progressBarSlider.value = value;
            _progressText.text = string.Format(_progressTextFormat, percent);
        }

        public void ResetWhenSceneChanged()
        {
            var alpha = _tipText.color;
            alpha.a = 0.0f;
            _tipText.color = alpha;
            _isTipFadeIn = true;
            _backgroundCanvasGroup.alpha = 1;
            _fadeImageCanvas.alpha = 1;
            _rootCanvasGroup.gameObject.SetActive(false);
            _fadeImageCanvas.gameObject.SetActive(true);

            if (_sceneLoadingInfo != null)
            {
                FadeInAsync(_sceneLoadingInfo.fadeInDelay, _sceneLoadingInfo.fadeInSpeed,
                                      () => 
                                          _fadeImageCanvas.gameObject.SetActive(false)
                                      ).Forget();
                if (this.firstSessionObjectButton.gameObject.activeInHierarchy)
                {
                    this.firstSessionObjectButton.enabled = true;
                    tapToContinue.SetActive(true);
                    loadingObject.SetActive(false);
                }
            }
            else
            {
                FadeInAsync(0.0f, _screenInitialFadeInSpeed, () => _fadeImageCanvas.gameObject.SetActive(false)).Forget();
            }
        }

        public async UniTask PrepareLoadingSceneAsync()
        {
            UpdateLoadProgress(0);
            float currentStartLoadSceneDelay = 0.0f;
            while (currentStartLoadSceneDelay < _startLoadSceneDelay)
            {
                currentStartLoadSceneDelay += DeltaTime;
                var value = Easing.EaseInOutCubic(0.0f, 1.0f, currentStartLoadSceneDelay / _startLoadSceneDelay);
                _rootCanvasGroup.alpha = value;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        public void UpdateSceneLoadingInfo(SceneLoadingInfo sceneLoadingInfo)
        {
            _sceneLoadingInfo = sceneLoadingInfo;
            _cacheBackgrounds = sceneLoadingInfo.backgrounds;
            if (_canChangeBackground)
            {
                if (_cacheBackgrounds.Count > 1)
                    StartTransitionBackgroundsAsync().Forget();
                else if (_cacheBackgrounds.Count > 0)
                    _backgroundImage.sprite = _cacheBackgrounds[0];
            }

            _cacheSceneLoadingTips = sceneLoadingInfo.sceneLoadingTips;
            if (_cacheSceneLoadingTips.Count > 0)
            {
                _currentTipIndex = UnityRandom.Range(0, _cacheSceneLoadingTips.Count);
                _tipText.text = _cacheSceneLoadingTips[_currentTipIndex];
                StartLoopTipsAsync().Forget();
            }
            else _tipText.text = "";

            _progressText.text = string.Format(_progressTextFormat, 0);
            _rootCanvasGroup.alpha = 0;
            _rootCanvasGroup.gameObject.SetActive(true);
        }

        private async UniTask StartTransitionBackgroundsAsync()
        {
            while (true)
            {
                _backgroundImage.sprite = _cacheBackgrounds[_currentBackgroundIndex];
                while (_backgroundCanvasGroup.alpha < 1.0f)
                {
                    _backgroundCanvasGroup.alpha += DeltaTime * _backgroundFadeSpeed;
                    await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_backgroundShowTime), true, cancellationToken: this.GetCancellationTokenOnDestroy());
                while (_backgroundCanvasGroup.alpha > 0.0f)
                {
                    _backgroundCanvasGroup.alpha -= DeltaTime * _backgroundFadeSpeed;
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }
                _currentBackgroundIndex = (_currentBackgroundIndex + 1) % _cacheBackgrounds.Count;
                await UniTask.Delay(TimeSpan.FromSeconds(_changeBackgroundDelay), true, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        private async UniTask StartLoopTipsAsync()
        {
            Color alpha = _tipText.color;
            if (_isTipFadeIn)
            {
                while (alpha.a < 1.0f)
                {
                    alpha.a += DeltaTime * _tipFadeSpeed;
                    _tipText.color = alpha;
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }
                await StartWaitForNextTipAsync(_tipShowTime);
            }
            else
            {
                while (alpha.a > 0.0f)
                {
                    alpha.a -= DeltaTime * _tipFadeSpeed;
                    _tipText.color = alpha;
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }
                await StartWaitForNextTipAsync(_changeTipDelay);
            }

            if (_isTipFadeIn)
            {
                int previoustipIndex = _currentTipIndex;
                _currentTipIndex = UnityRandom.Range(0, _cacheSceneLoadingTips.Count);
                while (_currentTipIndex == previoustipIndex)
                {
                    _currentTipIndex = UnityRandom.Range(0, _cacheSceneLoadingTips.Count);
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }
                _tipText.text = _cacheSceneLoadingTips[_currentTipIndex];
            }
        }

        private async UniTask StartWaitForNextTipAsync(float delayTime)
        {
            _isTipFadeIn = !_isTipFadeIn;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime), true, cancellationToken: this.GetCancellationTokenOnDestroy());
            await StartLoopTipsAsync();
        }

        private async UniTaskVoid FadeInAsync(float delay, float fadeInSpeed, Action finishAction = null)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), true, cancellationToken: this.GetCancellationTokenOnDestroy());
            if (fadeInSpeed > 0)
            {
                while (_fadeImageCanvas.alpha > 0.0f)
                {
                    _fadeImageCanvas.alpha -= DeltaTime * fadeInSpeed;
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }
            }
            finishAction?.Invoke();
        }

        public void FadeInFirstSession(Action callBackToStartLoadScene)
        {
            this.firstSessionObjectButton.gameObject.SetActive(true);
            this.firstSessionText.DOFade(1f, 3f).SetAutoKill(true).SetUpdate(true).OnComplete(() => {
                this.loadingObject.SetActive(true);
                callBackToStartLoadScene?.Invoke();
            });
        }
        
        public void FadeOutFirstSession()
        {
            this.firstSessionObjectButton.gameObject.SetActive(true);
            this.firstSessionText.DOFade(0f, 0.75f).SetAutoKill(true).SetUpdate(true);
            this.firstSessionObjectButton.image.DOFade(0f, 1f).SetAutoKill(true).SetUpdate(true).OnComplete(() => {
                firstSessionObjectButton.gameObject.SetActive(false);
            });
        }
        
        #endregion Class Methods
    }
}