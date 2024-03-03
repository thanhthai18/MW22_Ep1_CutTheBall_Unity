using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Runtime.UI
{
    public class QuitModal : Modal<QuitModalData>
    {
        #region Members

        [SerializeField]
        private TextMeshProUGUI _contentText;
        [SerializeField]
        protected Button btnCancel;
        [SerializeField]
        protected Button btnConfirm;
        protected bool isClosing;
        protected bool canStopGameFlow;

        #endregion Members

        #region Class Methods

        public override async UniTask Initialize(QuitModalData modalData)
        {
            await base.Initialize(modalData);
            _contentText.text = modalData.content;
            btnConfirm.onClick.AddListener(OnConfirm);
            btnCancel.onClick.AddListener(OnCancel);
            isClosing = false;
        }

        protected virtual void OnConfirm()
        {
            if (!isClosing)
            {
                isClosing = true;
                ownerModalData.confirmAction?.Invoke();
                Application.Quit();
            }
        }

        protected virtual void OnCancel()
        {
            if (!isClosing)
            {
                isClosing = true;
                ownerModalData.cancelAction?.Invoke();
                Close(false);
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