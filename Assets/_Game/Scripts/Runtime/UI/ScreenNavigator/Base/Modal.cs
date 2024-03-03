using Cysharp.Threading.Tasks;
using Runtime.Definition;
using Runtime.Manager.Data;
using Runtime.Manager.Game;
using Runtime.Manager.Toast;
using UnityEngine;
using ScreenNavigatorModal = UnityScreenNavigator.Runtime.Core.Modals.Modal;

namespace Runtime.UI
{
    public abstract class Modal<T> : ScreenNavigatorModal where T : ModalData
    {
        #region Members

        [SerializeField] protected ModalType modalType = ModalType.Normal;
        [SerializeField] protected bool isDisplayedFullScreen = true;
        protected T ownerModalData;

        #endregion Members

        #region Class Methods

        public override async UniTask InitializeInternal(object arg = null)
        {
            var modalData = arg as T;
            await Initialize(modalData);
        }

        public virtual async UniTask Initialize(T modalData)
        {
            GameManager.Instance.StopGameFlow();

            ScreenNavigator.Instance.SetUpModalOnInitialized(isDisplayedFullScreen);
            ownerModalData = modalData;
            // switch (modalType)
            // {
            //     case ModalType.Positive:
            //         AudioController.Instance.PlaySoundEffectAsync(AudioConstants.POPUP_POSITIVE, this.GetCancellationTokenOnDestroy()).Forget();
            //         break;
            //     case ModalType.Negative:
            //         AudioController.Instance.PlaySoundEffectAsync(AudioConstants.POPUP_NEGATIVE, this.GetCancellationTokenOnDestroy()).Forget();
            //         break;
            //     case ModalType.Revive:
            //         AudioController.Instance.PlaySoundEffectAsync(AudioConstants.REVIVE_PROMPT, this.GetCancellationTokenOnDestroy()).Forget();
            //         break;
            //     default:
            //         AudioController.Instance.PlaySoundEffectAsync(AudioConstants.POPUP_POSITIVE, this.GetCancellationTokenOnDestroy()).Forget();
            //         break;
            // }

            await UniTask.CompletedTask;
        }

        public override UniTask Cleanup()
        {
            GameManager.Instance.ContinueGameFlow();
            ScreenNavigator.Instance.SetUpModalOnCleanUp();
            if (ownerModalData != null && ownerModalData.OnClosedCallbackAction != null)
                ownerModalData.OnClosedCallbackAction.Invoke();
            return base.Cleanup();
        }

        public virtual void Close(bool playAnimation)
        {
            if (IsActive())
                ScreenNavigator.Instance.PopModal(this, playAnimation).Forget();
        }

        public virtual void ShowToast(string toastMessage, ToastVisualType toastVisualType = ToastVisualType.Text)
            => ToastManager.Instance.Show(toastMessage, toastVisualType);

        #endregion Class Methods
    }
}