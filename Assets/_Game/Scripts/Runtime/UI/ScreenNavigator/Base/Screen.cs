using UnityEngine;
using Runtime.Manager.Toast;
using Runtime.Manager.Game;
using Cysharp.Threading.Tasks;
using ScreenNavigatorScreen = UnityScreenNavigator.Runtime.Core.Screens.Screen;

namespace Runtime.UI
{
    public abstract class Screen<T> : ScreenNavigatorScreen where T : ScreenData
    {
        #region Members

        [SerializeField]
        protected bool isPreloaded;
        [SerializeField]
        protected bool isDisplayedFullScreen = true;
        protected T ownerScreenData;

        #endregion Members

        #region Class Methods

        public override async UniTask InitializeInternal(object arg)
        {
            var screenData = arg as T;
            await Initialize(screenData);
        }

        public virtual async UniTask Initialize(T screenData)
        {
            if (!isPreloaded)
                GameManager.Instance.StopGameFlow();
            ScreenNavigator.Instance.SetUpScreenOnInitialized(isDisplayedFullScreen);
            ownerScreenData = screenData;
            await UniTask.CompletedTask;
        }

        public override UniTask CleanUp()
        {
            if (!isPreloaded)
                GameManager.Instance.ContinueGameFlow();
            ScreenNavigator.Instance.SetUpScreenOnCleanUp();
            if (ownerScreenData != null && ownerScreenData.OnClosedCallbackAction != null)
                ownerScreenData.OnClosedCallbackAction.Invoke();
            return base.CleanUp();
        }

        public virtual void Close(bool playAnimation)
        {
            if (IsActive())
                ScreenNavigator.Instance.PopScreen(playAnimation).Forget();
        }

        public virtual void ShowToast(string toastMessage, ToastVisualType toastVisualType = ToastVisualType.Text)
            => ToastManager.Instance.Show(toastMessage, toastVisualType);

        #endregion Class Methods
    }
}