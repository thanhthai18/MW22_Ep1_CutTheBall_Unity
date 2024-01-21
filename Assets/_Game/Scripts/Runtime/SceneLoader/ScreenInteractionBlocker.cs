using System;
using Runtime.Common.Singleton;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Message;
using Runtime.Definition;
using Runtime.Manager.Toast;

namespace Runtime.Manager
{
    public class ScreenInteractionBlocker : MonoSingleton<ScreenInteractionBlocker>
    {
        #region Members

        [SerializeField]
        private GameObject _blockInteractionPanel;
        [SerializeField]
        private Button _panelInteractButton;
        
        private float _toastDisplayMin;
        private float _toastDisplayTime;

        private Action _clickCallBack;

        #endregion Members

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            _blockInteractionPanel.SetActive(false);
            _panelInteractButton.onClick.AddListener(OnClickPanelInteractButton);
        }

        #endregion API Methods

        #region Class Methods

        private void OnClickPanelInteractButton()
        {
            if (UnityEngine.Time.time - this._toastDisplayTime < this._toastDisplayMin)
            {
                ToastManager.Instance.Show("You request is processing!");
            }
            else
            {
                this._clickCallBack?.Invoke();
            }
        }

        public void Block(float minTime = 0f, Action clickAction = null)
        {
            _panelInteractButton.gameObject.SetActive(true);
            _blockInteractionPanel.SetActive(true);
            this._toastDisplayTime = UnityEngine.Time.time;
            _toastDisplayMin = minTime;
            this._clickCallBack = clickAction;
        }

        public void Unblock()
        {
            _blockInteractionPanel.SetActive(false);
            _panelInteractButton.gameObject.SetActive(false);
            this._clickCallBack = null;
        }
        
        #endregion Class Methods
    }
}