using Cysharp.Threading.Tasks;
using Runtime.Definition;
using Runtime.Manager.Toast;
using UnityScreenNavigator.Runtime.Core.Modals;
using UnityScreenNavigator.Runtime.Core.Screens;
using UnityScreenNavigator.Runtime.Core.Shared.Views;

namespace Runtime.UI
{
    public partial class ScreenNavigator
    {
        public async UniTask Navigate(NavigationTargetType navigationTargetType, int targetId)
        {
            switch (navigationTargetType)
            {
                case NavigationTargetType.None:
                    //var modalOptions = new WindowOptions(ModalId.DAILY_FORTUNE);
                    //await LoadModal(modalOptions, new EmptyModalData());
                    //var options = new WindowOptions(ScreenId.ORDER_SCREEN);
                    //await LoadScreen(options, new EmptyScreenData());
                    //ToastManager.Instance.Show(structureUnlockedData.unlockedRequimentDescription);
                    break;
                default:
                    break;
            }
        }
        public async UniTask PopToRootScreen(bool playAnimation)
        {
            await CheckWaitForFinishLoading();

            isLoading = true;
            var modalContainer = globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            while (modalContainer.Modals.Count > 0)
                await modalContainer.Pop(false);
            var screenContainer = globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
            while (screenContainer.Screens.Count > 1)
                await screenContainer.Pop(false);
            isLoading = false;
        }
    }
}
