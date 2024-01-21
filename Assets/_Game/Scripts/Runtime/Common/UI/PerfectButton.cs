using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Runtime.Common.Interface;

namespace Runtime.Common.UI
{
    public class PerfectButton : Button, IClickable
    {
        #region Members

        private static readonly float s_proceedOnClickDelayTime = 0.1f;
        private static readonly float s_preventSpamDelayTime = 0.15f;
        private bool _blockInput = false;

        #endregion Members

        #region API Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _blockInput = false;
        }

        #endregion API Methods

        #region Class Methods

        public void Click()
            => Trigger();

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!_blockInput && interactable)
            {
                _blockInput = true;
                Press();
                StartCoroutine(BlockInputTemporarily());
            }
        }

        protected virtual bool Press()
        {
            if (!IsActive())
                return false;

            StartCoroutine(InvokeOnClickAction());
            return true;
        }

        private IEnumerator InvokeOnClickAction()
        {
            yield return new WaitForSecondsRealtime(s_proceedOnClickDelayTime);
            ProceedAction();
        }

        private IEnumerator BlockInputTemporarily()
        {
            yield return new WaitForSecondsRealtime(s_preventSpamDelayTime);
            _blockInput = false;
        }

        private void ProceedAction()
            => onClick.Invoke();

        #endregion Class Methods

        #region Unity Event Callback Methods

        public void Trigger()
            => ProceedAction();

        #endregion Unity Event Callback Methods
    }
}