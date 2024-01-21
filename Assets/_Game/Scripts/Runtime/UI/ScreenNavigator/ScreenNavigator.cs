using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Modals;
using UnityScreenNavigator.Runtime.Core.Shared.Views;
using UnityScreenNavigator.Runtime.Core.Screens;
using UnityScreenNavigator.Runtime.Foundation;
using Runtime.Message;
using Runtime.Definition;
using Runtime.Common.Singleton;
using Runtime.SceneLoading;
using Runtime.Extensions;
using Cysharp.Threading.Tasks;
using ScreenNavigatorModal = UnityScreenNavigator.Runtime.Core.Modals.Modal;

namespace Runtime.UI
{
    [DisallowMultipleComponent]
    public partial class ScreenNavigator : MonoSingleton<ScreenNavigator>
    {
        #region Members

        [SerializeField] protected Transform containersHolderTransform;
        protected GlobalContainerLayerManager globalContainerLayerManager;
        protected bool isLoading;
        protected bool isBackKeyOperated;
        protected bool isSceneTransitioned;

        #endregion Members

        #region Properties

        protected bool IsOpeningAModal
        {
            get
            {
                var modalContainer =
                    globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
                return modalContainer.Modals.Count > 0;
            }
        }

        protected bool IsOpeningMoreThanAScreen
        {
            get
            {
                var screenContainer =
                    globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
                return screenContainer.Screens.Count > 1;
            }
        }

        public bool IsOpenAWindowAbove => IsOpeningMoreThanAScreen || IsOpeningAModal;

        #endregion Properties

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            SceneManager.RegisterTriggerChangeScene(OnTriggerChangeScene);
            SceneManager.RegisterBeforeChangeScene(OnBeforeChangeScene);
            SceneManager.RegisterBeforeNewSceneAppeared(OnBeforeNewSceneAppeared);
            isSceneTransitioned = true;
            globalContainerLayerManager = this.GetOrAddComponent<GlobalContainerLayerManager>();
            var containerLayers = containersHolderTransform.GetComponentsInChildren<ContainerLayer>();
            foreach (var container in containerLayers)
                container.SetUp(globalContainerLayerManager, container.gameObject.name);
            var screenContainer =
                globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
            if (screenContainer != null)
                screenContainer.Preload(new EmptyScreenData());
        }

        private void OnDestroy()
        {
        }

        #endregion API Methods

        #region Class Methods

        public async UniTask LoadModal(WindowOptions option, ModalData modalData)
        {
            await CheckWaitForFinishLoading();
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            if (modalContainer.Modals.Count == 0 || (!modalContainer.Current.Name.StartsWith(option.resourcePath)))
            {
                isLoading = true;
                await modalContainer.Push(option, modalData);
                isLoading = false;
            }
        }

        public async UniTask LoadSingleModal(WindowOptions option, ModalData modalData)
        {
            await CheckWaitForFinishLoading();
            isLoading = true;
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            while (modalContainer.Modals.Count > 0)
                await modalContainer.Pop(false);
            await modalContainer.Push(option, modalData);
            isLoading = false;
        }

        public void PopTopmostModal()
            => RunPopTopmostModalAsync(false).Forget();

        public void PopAllModals()
            => RunPopAllModalsAsync().Forget();

        public void PopAllAboveModals(ScreenNavigatorModal requestModal)
            => RunPopAllAboveModalsAsync(requestModal).Forget();

        public void PopAllAboveModals(GameObject requestModalGameObject)
        {
            var requestModal = requestModalGameObject.GetComponent<ScreenNavigatorModal>();
            if (requestModal != null)
                PopAllAboveModals(requestModal);
        }

        public async UniTask PopModal(ScreenNavigatorModal poppedModal, bool playAnimation)
        {
            await CheckWaitForFinishLoading();
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            if (modalContainer.Modals.Count > 0)
                await RunPopModalAsync(poppedModal, modalContainer.Modals, modalContainer, playAnimation);
        }

        public async UniTask LoadScreen(WindowOptions option, ScreenData screenData)
        {
            await CheckWaitForFinishLoading();
            var screenContainer =
                globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
            if (screenContainer.Screens.Count == 0 || !screenContainer.Current.Name.StartsWith(option.resourcePath))
            {
                isLoading = true;
                var modalContainer =
                    globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
                while (modalContainer.Modals.Count > 0)
                    await modalContainer.Pop(false);
                await screenContainer.Push(option, screenData);
                isLoading = false;
            }
        }

        public async UniTask PopToRootScreen()
        {
            await CheckWaitForFinishLoading();

            isLoading = true;
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            while (modalContainer.Modals.Count > 0)
                await modalContainer.Pop(false);
            var screenContainer =
                globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
            while (screenContainer.Screens.Count > 1)
                await screenContainer.Pop(false);
            isLoading = false;
        }

        public async UniTask PopScreen(bool playAnimation)
        {
            await CheckWaitForFinishLoading();
            isLoading = true;
            var screenContainer =
                globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
            if (screenContainer.Screens.Count > 1)
                await screenContainer.Pop(playAnimation);
            isLoading = false;
        }

        public bool IsShowingModal(string modalName)
        {
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            return modalContainer.Modals.Count > 0 && modalContainer.Modals.Any(x => x.Name.StartsWith(modalName));
        }

        public bool IsShowingScreen(string screenName)
        {
            var screenContainer =
                globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
            return screenContainer.Screens.Count > 0 && screenContainer.Screens.Any(x => x.Name.StartsWith(screenName));
        }

        public bool IsShowingOnlyScreen(string screenName)
        {
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            if (modalContainer.Modals.Count == 0)
            {
                var screenContainer =
                    globalContainerLayerManager.Find<ScreenContainer>(ContainerKey.SCREEN_CONTAINER_LAYER_NAME);
                return screenContainer.Screens.Count > 0 && screenContainer.Current.Name.StartsWith(screenName);
            }
            else return false;
        }

        protected async UniTask RunPopAllModalsAsync()
        {
            await CheckWaitForFinishLoading();
            isLoading = true;
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            while (modalContainer.Modals.Count > 0)
                await modalContainer.Pop(false);
            isLoading = false;
        }

        protected async UniTask RunPopAllAboveModalsAsync(ScreenNavigatorModal requestModal)
        {
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            if (modalContainer.Modals.Contains(requestModal))
            {
                await CheckWaitForFinishLoading();
                isLoading = true;
                while (!modalContainer.Current.Equals(requestModal))
                    await modalContainer.Pop(false);
                isLoading = false;
            }
        }

        protected async UniTask RunPopModalAsync(ScreenNavigatorModal poppedModal,
            IReadOnlyList<ScreenNavigatorModal> modals,
            ModalContainer modalContainer, bool playAnimation)
        {
            if (poppedModal == modals[modals.Count - 1] == poppedModal)
            {
                isLoading = true;
                await modalContainer.Pop(playAnimation);
                isLoading = false;
            }
            else
            {
                var clonedModals = new List<ScreenNavigatorModal>();
                for (int i = 0; i < modals.Count; i++)
                    clonedModals.Add(modals[i]);

                while (clonedModals[clonedModals.Count - 1] != poppedModal)
                {
                    var waitToPopModalGameObject = clonedModals[clonedModals.Count - 1].gameObject;
                    await UniTask.WaitUntil(() => waitToPopModalGameObject == null);
                    clonedModals.RemoveAt(clonedModals.Count - 1);
                }

                await CheckWaitForFinishLoading();
                isLoading = true;
                await modalContainer.Pop(playAnimation);
                isLoading = false;
            }
        }


        protected async UniTask RunPopTopmostModalAsync(bool playAnimation)
        {
            await CheckWaitForFinishLoading();
            var modalContainer =
                globalContainerLayerManager.Find<ModalContainer>(ContainerKey.MODAL_CONTAINER_LAYER_NAME);
            if (modalContainer.Modals.Count > 0)
            {
                isLoading = true;
                await modalContainer.Pop(playAnimation);
                isLoading = false;
            }
        }
       
        protected void OnTriggerChangeScene()
        {
            isLoading = false;
            isSceneTransitioned = true;
        }

        protected void OnBeforeChangeScene()
        {
            isLoading = false;
            var containerLayers = containersHolderTransform.GetComponentsInChildren<ContainerLayer>();
            foreach (var container in containerLayers)
                container.Cleanup();
        }

        protected void OnBeforeNewSceneAppeared()
        {
            isLoading = false;
            isSceneTransitioned = false;
        }

        protected async UniTask CheckWaitForFinishLoading()
        {
            if (isLoading)
                await UniTask.WaitUntil(() => !isLoading, cancellationToken: this.GetCancellationTokenOnDestroy());
            else
                await UniTask.CompletedTask;
        }

        protected void OnGameStateChanged()
        {
        }

        #endregion Class Methods
    }
}