using System.Collections.Generic;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Shared.Views;
using Runtime.Message;
using Runtime.Definition;
using Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Runtime.Manager.Game;
using Runtime.Manager.Data;
using Runtime.Gameplay.Map;

namespace Runtime.UI
{
    [DisallowMultipleComponent]
    public class WorldScreenNavigator : ScreenNavigator
    {
        #region Members

        [SerializeField]
        private CanvasGroup _headerPanelCanvasGroup;
        private Stack<bool> _headerPanelVisibilityStatusesStack = new Stack<bool>();

        #endregion Members

        #region Class Methods

        public override void SetUpModalOnInitialized(bool isDisplayedFullScreen)
        {
            var isHeaderPanelVisible = !isDisplayedFullScreen;
            _headerPanelCanvasGroup.SetActive(isHeaderPanelVisible);
            _headerPanelVisibilityStatusesStack.Push(isHeaderPanelVisible);
        }

        public override void SetUpModalOnCleanUp()
        {
            if (_headerPanelVisibilityStatusesStack.Count > 1)
            {
                _headerPanelVisibilityStatusesStack.Pop();
                var isHeaderPanelVisible = _headerPanelVisibilityStatusesStack.Peek();
                _headerPanelCanvasGroup.SetActive(isHeaderPanelVisible);
            }
            else
            {
                var isHeaderPanelVisible = _headerPanelVisibilityStatusesStack.Pop();
                _headerPanelCanvasGroup.SetActive(isHeaderPanelVisible);
            }
        }

        public override void SetUpScreenOnInitialized(bool isDisplayedFullScreen)
            => SetUpModalOnInitialized(isDisplayedFullScreen);

        public override void SetUpScreenOnCleanUp()
            => SetUpModalOnCleanUp();

        #endregion Class Methods
    }
}