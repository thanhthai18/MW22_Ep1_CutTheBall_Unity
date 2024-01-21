using System;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Extensions;
using TMPro;
using DG.Tweening;

namespace Runtime.UI
{
    public class InitializerLoadingPanel : MonoBehaviour
    {
        #region Members

        [SerializeField]
        private float _disappearFadeOutDuration;
        [SerializeField]
        private CanvasGroup _rootCanvasGroup;
        [SerializeField]
        private Image _progressBarSlider;
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

        public void UpdateLoading(float value)
        {
            var displayValue = Mathf.Floor(value * 100);
            _progressBarText.text = displayValue + "%";
            _progressBarSlider.transform.localScale = new Vector2(value, 1);
        }

        public void FinishLoading()
        {
            _infoPanelGameObject.SetActive(false);
            _tapToStartPanelGameObject.SetActive(true);
        }

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

        #endregion Class Methods
    }
}