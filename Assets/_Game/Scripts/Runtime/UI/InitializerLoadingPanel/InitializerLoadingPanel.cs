using System;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Extensions;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Runtime.UI
{
    public class InitializerLoadingPanel : MonoBehaviour
    {
        #region Members

        [SerializeField]
        private float _fakeProgressMinDuration;
        [SerializeField]
        private float _fakeProgressMaxDuration;
        [SerializeField]
        private float _disappearFadeOutDuration;
        [SerializeField]
        private CanvasGroup _rootCanvasGroup;
        [SerializeField]
        private Slider _progressBarSlider;
        [SerializeField]
        private TextMeshProUGUI _progressBarText;
        [SerializeField]
        private TextMeshProUGUI _versionText;
        [SerializeField]
        private Button _tapToStartButton;
        [SerializeField]
        private GameObject _infoPanelGameObject;
        [SerializeField]
        private GameObject _tapToStartPanelGameObject;

        #endregion Members

        #region Properties

        private Action DisappearActionCallback { get; set; }

        #endregion Properties

        #region Class Methods

        public void InitLoading(Action disappearActionCallback)
        {
            _versionText.text = $"v-{Application.version}";
            _rootCanvasGroup.SetActive(true);
            _infoPanelGameObject.SetActive(true);
            _tapToStartPanelGameObject.SetActive(false);
            _tapToStartButton.onClick.AddListener(OnClickTapToStartButton);
            DisappearActionCallback = disappearActionCallback;
            UpdateLoading(0);
        }

        public void FinishLoading()
            => RunFakeProgressAsync().Forget();

        private void OnClickTapToStartButton()
        {
            _tapToStartButton.onClick.RemoveAllListeners();
            PlayDisappearAsync();
        }

        private void PlayDisappearAsync()
        {
            DOTween.To(() => 1f, x => { _rootCanvasGroup.alpha = x; }, 0, _disappearFadeOutDuration)
                .OnComplete(() => {
                    _rootCanvasGroup.SetActive(false);
                    DisappearActionCallback?.Invoke();
                }).SetUpdate(true);
        }

        private async UniTask RunFakeProgressAsync()
        {
            UpdateLoading(0.0f);
            var currentDuration = 0.0f;
            var fakeProgressDuration = UnityEngine.Random.Range(_fakeProgressMinDuration, _fakeProgressMaxDuration);
            while (currentDuration < fakeProgressDuration)
            {
                currentDuration += Time.deltaTime;
                UpdateLoading(currentDuration / fakeProgressDuration);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
            UpdateLoading(1.0f);
            _infoPanelGameObject.SetActive(false);
            _tapToStartPanelGameObject.SetActive(true);
        }

        private void UpdateLoading(float value)
        {
            var displayValue = Mathf.Floor(value * 100);
            _progressBarText.text = displayValue + "%";
            _progressBarSlider.value = value;
        }

        #endregion Class Methods
    }
}
