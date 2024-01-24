// using System.Collections.Generic;
// using UnityEngine;
// using UnityScreenNavigator.Runtime.Core.Shared.Views;
// using Runtime.Message;
// using Runtime.Definition;
// using Runtime.Extensions;
// using Cysharp.Threading.Tasks;
// using Runtime.Manager.Game;
// using Runtime.Manager.Data;
// using Runtime.Tracking;
// using Runtime.Gameplay.Map;
// using Runtime.Audio;
//
// #if UNITY_ANDROID
// using Google.Play.Review;
// #endif
//
// #if UNITY_IOS
//     using UnityEngine.iOS;
// #endif
//
// namespace Runtime.UI
// {
//     [DisallowMultipleComponent]
//     public class WorldScreenNavigator : ScreenNavigator
//     {
//         public static bool ShowRateUs = false;
//
//         #region Members
//
//         [SerializeField]
//         private CanvasGroup _headerPanelCanvasGroup;
//         private Stack<bool> _headerPanelVisibilityStatusesStack = new Stack<bool>();
//
//         #endregion Members
//
//         #region Class Methods
//
//         public override void SetUpModalOnInitialized(bool isDisplayedFullScreen)
//         {
//             var isHeaderPanelVisible = !isDisplayedFullScreen;
//             _headerPanelCanvasGroup.SetActive(isHeaderPanelVisible);
//             _headerPanelVisibilityStatusesStack.Push(isHeaderPanelVisible);
//         }
//
//         public override void SetUpModalOnCleanUp()
//         {
//             if (_headerPanelVisibilityStatusesStack.Count > 1)
//             {
//                 _headerPanelVisibilityStatusesStack.Pop();
//                 var isHeaderPanelVisible = _headerPanelVisibilityStatusesStack.Peek();
//                 _headerPanelCanvasGroup.SetActive(isHeaderPanelVisible);
//             }
//             else
//             {
//                 var isHeaderPanelVisible = _headerPanelVisibilityStatusesStack.Pop();
//                 _headerPanelCanvasGroup.SetActive(isHeaderPanelVisible);
//             }
//         }
//
//         public override void SetUpScreenOnInitialized(bool isDisplayedFullScreen)
//             => SetUpModalOnInitialized(isDisplayedFullScreen);
//
//         public override void SetUpScreenOnCleanUp()
//             => SetUpModalOnCleanUp();
//
//         protected override void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
//         {
//             base.OnGameStateChanged(gameStateChangedMessage);
//             if (gameStateChangedMessage.GameStateEventType == GameStateEventType.GameLost)
//             {
//                 var windowOptions = new WindowOptions(ModalId.WORLD_REVIVE, true);
//                 var reviveModalData = new WorldReviveModalData(OnReviveSucceeded, OnGiveUpRevive);
//                 LoadModal(windowOptions, reviveModalData).Forget();
//
// #if TRACKING
//                 string allHero = "";
//                 foreach (var heroFragment in DataManager.Local.HeroFragments)
//                 {
//                     if (heroFragment.isSelected)
//                     {
//                         allHero += heroFragment.id + " ,";
//                     }
//                 }
//
//                 var activeMapAreaIds = MapManager.Instance.GetActiveMapAreaIds();
//                 if (activeMapAreaIds != null && activeMapAreaIds.Count > 0)
//                     FirebaseManager.Instance.TrackDiedEvent(allHero, "world_map", activeMapAreaIds[0]);
// #endif
//             }
//         }
//
//         public void CheckShowRateUs()
//         {
//             if (ShowRateUs)
//             {
//                 var windowOptions = new WindowOptions(ModalId.RATE_US_MODAL, true);
//                 LoadModal(windowOptions).Forget();
//                 ShowRateUs = false;
//             }
//         }
//
//         private void OnGiveUpRevive()
//         {
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.DEFEAT, this.GetCancellationTokenOnDestroy()).Forget();
//             var modalData = new WorldResultModalData(OnGiveUpWorld);
//             var windowOptions = new WindowOptions(ModalId.WORLD_RESULT, true);
//             LoadModal(windowOptions, modalData).Forget();
//         }
//
//         private void OnGiveUpWorld()
//             => Messenger.Publish(new GameStateChangedMessage(GameStateEventType.GiveUpWorldMapTriggered));
//
//         #region Rate US
//
//         public void OpenRating()
//         {
// #if UNITY_ANDROID
//             GoRatingAndroid();
// #elif UNITY_IOS
//         bool isReviewNative = Device.RequestStoreReview();
//         if (!isReviewNative)
//         {
//             var iosAppId = GameConfig.Instance.appleAppId; 
//             var url = $"https://itunes.apple.com/us/app/apple-store/id{iosAppId}?mt=8";
//             Application.OpenURL(url);
//         }
// #endif
//
//         }
//
// #if UNITY_ANDROID
//         private void GoRatingAndroid()
//         {
//             RequestReview().Forget();
//         }
//
//         private async UniTask RequestReview()
//         {
//             var reviewManager = new ReviewManager();
//
//             var requestFlowOperation = reviewManager.RequestReviewFlow();
//             await requestFlowOperation;
//             if (requestFlowOperation.Error != ReviewErrorCode.NoError)
//             {
//                 // Log error. For example, using requestFlowOperation.Error.ToString().
//                 Debug.LogError($"Android rating Request Flow Operation error: {requestFlowOperation.Error.ToString()}");
//                 Application.OpenURL($"market://details?id={Application.identifier}");
//                 await UniTask.CompletedTask;
//                 return;
//             }
//
//             var playReviewInfo = requestFlowOperation.GetResult();
//
//             var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
//             await launchFlowOperation;
//
//             if (launchFlowOperation.Error != ReviewErrorCode.NoError)
//             {
//                 // Log error. For example, using requestFlowOperation.Error.ToString().
//                 Debug.LogError(
//                     $"Android rating Launch Flow Operation error: {requestFlowOperation.Error.ToString()}");
//                 Application.OpenURL($"market://details?id={Application.identifier}");
//                 await UniTask.CompletedTask;
//             }
//             // The flow has finished. The API does not indicate whether the user
//             // reviewed or not, or even whether the review dialog was shown. Thus, no
//             // matter the result, we continue our app flow.
//         }
// #endif
//
//         #endregion
//
//         #endregion Class Methods
//     }
// }