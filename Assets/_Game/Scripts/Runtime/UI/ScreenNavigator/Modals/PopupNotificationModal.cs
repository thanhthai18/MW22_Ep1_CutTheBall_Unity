using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using Runtime.Localization;

namespace Runtime.UI
{
    public class PopupNotificationModal : Modal<PopupNotificationModalData>
    {
        #region Members

        [SerializeField]
        private TextMeshProUGUI _contentText;     
        [SerializeField]
        protected Button btnFirstOption;
        [SerializeField]
        protected TextMeshProUGUI firstOptionText;
        [SerializeField]
        protected Button btnSecondOption;
        [SerializeField]
        protected TextMeshProUGUI secondOptionText;
        protected bool isClosing;
        protected bool canStopGameFlow;

        #endregion Members

        #region Class Methods

        public override async UniTask Initialize(PopupNotificationModalData modalData)
        {
            await base.Initialize(modalData);
            _contentText.text = modalData.content;

            btnFirstOption.gameObject.SetActive(false);
            btnSecondOption.gameObject.SetActive(false);

            if (modalData.firstOption == null && modalData.secondOption == null)
            {
                firstOptionText.text = LocalizationUtils.GetGeneralLocalized(LocalizeKeys.GOT_IT);
                btnFirstOption.gameObject.SetActive(true);
                btnFirstOption.onClick.RemoveAllListeners();
                btnFirstOption.onClick.AddListener(OnClose);
            }

            if(modalData.firstOption != null)
            {
                firstOptionText.text = modalData.firstOption.content;
                btnFirstOption.gameObject.SetActive(true);
                btnFirstOption.onClick.RemoveAllListeners();
                btnFirstOption.onClick.AddListener(() => OnOptionClick(modalData.firstOption.callback));
            }

            if(modalData.secondOption != null)
            {
                secondOptionText.text = modalData.secondOption.content;
                this.btnSecondOption.gameObject.SetActive(true);
                btnSecondOption.onClick.RemoveAllListeners();
                btnSecondOption.onClick.AddListener(() => OnOptionClick(modalData.secondOption.callback));
            }
            
            isClosing = false;
        }

        protected virtual void OnOptionClick(Action callbackOnClickOption)
        {
            if (!isClosing)
            {
                isClosing = true;
                Close(false);
                callbackOnClickOption?.Invoke();
            }
        }

        protected virtual void OnClose()
        {
            if (!isClosing)
            {
                isClosing = true;
                Close(false);
            }
        }

        #endregion Class Methods
    }
}