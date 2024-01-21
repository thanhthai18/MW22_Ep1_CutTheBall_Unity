using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Runtime.Manager.Toast.ToastManager;

namespace Runtime.Manager.Toast
{
    public class Toast : MonoBehaviour
    {
        #region Members

        [SerializeField]
        private TextMeshProUGUI _displayText;
        [SerializeField]
        private Image _displayImage;
        [SerializeField]
        private TextMeshProUGUI _displayNumberText;
        [SerializeField]
        private ToastVisualType _toastVisualType;
        private RectTransform _rectTransform;

        #endregion Members

        #region Properties

        public ToastVisualType ToastVisualType => _toastVisualType;
        private Action<Toast> EndToast { get; set; }

        #endregion Properties

        #region Class Methods

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(string toastText, Transform positionTransform, Action<Toast> endToastAction)
        {
            if (_toastVisualType != ToastVisualType.Reward)
            {
                var canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup)
                    canvasGroup.alpha = 1;
                transform.SetParent(positionTransform);
                transform.position = positionTransform.position;
                gameObject.SetActive(true);
                _displayText.text = toastText;
                transform.SetAsLastSibling();
                EndToast = endToastAction;
            }
        }

        public void Init(Sprite spriteResource, string nameResource, long numberResource, Transform positionTransform, Action<Toast> endToastAction)
        {
            if (_toastVisualType == ToastVisualType.Reward)
            {
                var canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup)
                    canvasGroup.alpha = 1;
                transform.SetParent(positionTransform);
                transform.position = positionTransform.position;
                gameObject.SetActive(true);
                _displayText.text = nameResource;
                _displayImage.sprite = spriteResource;
                _displayNumberText.text = "+" + numberResource;
                _displayImage.gameObject.SetActive(true);
                _displayNumberText.gameObject.SetActive(true);
                transform.SetAsLastSibling();
                EndToast = endToastAction;
            }
        }


        public void Finishing()
        {
            EndToast?.Invoke(this);
            gameObject.SetActive(false);
        }

        #endregion Class Methods
    }
}
