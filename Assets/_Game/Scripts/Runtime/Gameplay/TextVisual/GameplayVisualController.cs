using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Common.Singleton;
using Runtime.Extensions;
using Runtime.Manager.Pool;
using Runtime.Definition;
using Runtime.AssetLoader;
using Runtime.SceneLoading;
using Cysharp.Threading.Tasks;
using Runtime.Animation;
using Runtime.Config;
using Runtime.Gameplay.Manager;
using Runtime.Localization;
using Runtime.Manager;
using Runtime.Manager.Game;
using Runtime.Manager.Toast;
using Runtime.UI;
using UnityScreenNavigator.Runtime.Core.Shared.Views;
using Runtime.Manager.Data;
using Runtime.Message;
using UnityEngine.AddressableAssets;

namespace Runtime.Gameplay.Visual
{
    public class GameplayVisualController : MonoSingleton<GameplayVisualController>
    {
        #region Members

        [SerializeField] private int _maxDamageVisualsNumber;
        [SerializeField] private int _maxExpVisualsNumber;
        [SerializeField] private int _timeDelayZoomBackInMiliSecond = 1000;
        private int _maxKillStreakBonusVisualsNumber;
        private List<DamageVisual> _damageVisuals;
        private List<ExpVisual> _expVisuals;
        private List<KillStreakBonusVisual> _killStreakBonusVisuals;

        private CancellationTokenSource _cancellationTokenSource;
        private MessageRegistry<GameStateChangedMessage> _gameStateChangedMessageRegistry;

        public Dictionary<string, SpriteAnimation> CachedHeroIdleSpriteAnimations { get; set; } = new();

        #endregion Members

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            _maxKillStreakBonusVisualsNumber = 50;
            _maxDamageVisualsNumber = 50;
            _damageVisuals = new();
            _expVisuals = new();
            _killStreakBonusVisuals = new();
        }

        #endregion API Methods

        #region Class Methods


        private void OnDestroy()
        {
            _gameStateChangedMessageRegistry.Dispose();
        }

        public async UniTask DislayDamageVisual(string damageVisualKey, int damageValue, bool isCrit, Vector2 spawnPosition, CancellationToken token)
        {
            while (_damageVisuals.Count >= _maxDamageVisualsNumber)
            {
                var removedDamageVisual = _damageVisuals[0];
                _damageVisuals.Remove(removedDamageVisual);
                PoolManager.Instance.Remove(removedDamageVisual.gameObject);
            }

            var damageVisualGameObject = await PoolManager.Instance.Get(damageVisualKey, cancellationToken: token);
            var damageVisual = damageVisualGameObject.GetOrAddComponent<DamageVisual>();
            damageVisual.Init(spawnPosition, damageValue, isCrit);
            _damageVisuals.Add(damageVisual);
        }

        public async UniTask DislayDamageVisualMissed(string damageVisualKey, Vector2 spawnPosition, CancellationToken token)
        {
            while (_damageVisuals.Count >= _maxDamageVisualsNumber)
            {
                var removedDamageVisual = _damageVisuals[0];
                _damageVisuals.Remove(removedDamageVisual);
                PoolManager.Instance.Remove(removedDamageVisual.gameObject);
            }

            var damageVisualGameObject = await PoolManager.Instance.Get(damageVisualKey, cancellationToken: token);
            var damageVisual = damageVisualGameObject.GetOrAddComponent<DamageVisual>();
            damageVisual.InitMissedDamage(spawnPosition);
            _damageVisuals.Add(damageVisual);
        }

        public async UniTask DislayKillStreakBonusVisual(string killStreakBonusVisualKey, Vector2 spawnPosition, CancellationToken token)
        {
            while (_killStreakBonusVisuals.Count >= _maxKillStreakBonusVisualsNumber)
            {
                var removedKillStreakBonusVisual = _killStreakBonusVisuals[0];
                _killStreakBonusVisuals.Remove(removedKillStreakBonusVisual);
                PoolManager.Instance.Remove(removedKillStreakBonusVisual.gameObject);
            }

            var killStreakBonusVisualGameObject = await PoolManager.Instance.Get(killStreakBonusVisualKey, cancellationToken: token);
            var killStreakBonusVisual = killStreakBonusVisualGameObject.GetOrAddComponent<KillStreakBonusVisual>();
            killStreakBonusVisual.Init(spawnPosition);
            _killStreakBonusVisuals.Add(killStreakBonusVisual);
        }

        public async UniTask PlayShakeScreenEffectAsync()
        {
            LockViewRegion();
            await CameraManager.Instance.ShakeAsync(ShakeType.Normal, this.GetCancellationTokenOnDestroy());
            UnlockViewRegion();
        }

        public async UniTask GoToPosition(Vector3 targetPosition, bool goBack, Action callBack)
        {
            await UniTask.Delay(1000);
            _cancellationTokenSource = new CancellationTokenSource();
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.GameFlowStopped));
            await CameraManager.Instance.MoveToAsync(targetPosition, _cancellationTokenSource.Token);
            callBack?.Invoke();
        }

        public void HideDamageVisual(DamageVisual damageVisual)
        {
            _damageVisuals.Remove(damageVisual);
            PoolManager.Instance.Remove(damageVisual.gameObject);
        }

        public void HideExpVisual(ExpVisual expVisual)
        {
            _expVisuals.Remove(expVisual);
            PoolManager.Instance.Remove(expVisual.gameObject);
        }

        public void HideKillStreakBonusVisual(KillStreakBonusVisual killStreakBonusVisual)
        {
            _killStreakBonusVisuals.Remove(killStreakBonusVisual);
            PoolManager.Instance.Remove(killStreakBonusVisual.gameObject);
        }

        public void LockViewRegion()
        {
            GameManager.Instance.StopGameFlow();
            ScreenInteractionBlocker.Instance.Block();
            CameraManager.Instance.LockCameraView();
        }

        public void UnlockViewRegion(bool resetAfterCinema = true)
        {
            GameManager.Instance.ContinueGameFlow();
            ScreenInteractionBlocker.Instance.Unblock();
            CameraManager.Instance.UnlockCameraView(resetAfterCinema);
        }

        public async UniTask TranslateCameraAsync(Vector2 translatedPosition, CancellationToken cancellationToken)
            => await CameraManager.Instance.MoveToAsync(translatedPosition, cancellationToken);

        public async UniTask ZoomOutCameraAsync(CancellationToken cancellationToken)
            => await CameraManager.Instance.ZoomToAsync(OrthographicType.ZoomOut, cancellationToken);

        public async UniTask ZoomInCameraAsync(CancellationToken cancellationToken)
            => await CameraManager.Instance.ZoomToAsync(OrthographicType.ZoomIn, cancellationToken);

        public async UniTask ZoomToOriginalCameraAsync(CancellationToken cancellationToken)
            => await CameraManager.Instance.ZoomToAsync(OrthographicType.Normal, cancellationToken);

        public async UniTask ZoomInDeepCameraAsync(CancellationToken cancellationToken)
            => await CameraManager.Instance.ZoomToAsync(OrthographicType.ZoomInDeep, cancellationToken);
        public async UniTask ZoomOutDeepCameraAsync(CancellationToken cancellationToken)
          => await CameraManager.Instance.ZoomToAsync(OrthographicType.ZoomOutDeep, cancellationToken);

        public async UniTask ShakeCameraAsync(CancellationToken cancellationToken)
            => await CameraManager.Instance.ShakeAsync(ShakeType.Normal, cancellationToken);

        private async UniTask PlayFogGateUnlockedAnimationAsync(Vector2 gatePosition, Func<UniTask> actionTask)
        {
            Reset();
            LockViewRegion();
            await CameraManager.Instance.MoveToAsync(gatePosition, this.GetCancellationTokenOnDestroy());
            await ZoomInCameraAsync(this.GetCancellationTokenOnDestroy());
            await ZoomToOriginalCameraAsync(this.GetCancellationTokenOnDestroy());
            await CameraManager.Instance.MoveBackToHeroesGroupAsync(this.GetCancellationTokenOnDestroy());
            UnlockViewRegion();
        }

        private async UniTask PlayUnlockedVisualCameraAnimationAsync(Vector2 gatePosition, Func<UniTask> actionTask, Action actionCustomCall = null)
        {
            Reset();
            actionCustomCall?.Invoke();
            LockViewRegion();
            await CameraManager.Instance.MoveToAsync(gatePosition, this.GetCancellationTokenOnDestroy());
            await UniTask.Delay(_timeDelayZoomBackInMiliSecond, true);
            await actionTask.Invoke();
            await CameraManager.Instance.MoveBackToHeroesGroupAsync(this.GetCancellationTokenOnDestroy());
            UnlockViewRegion();
        }

        public async UniTask PlayUpgradeMiningVisualCameraAnimationAsync(Vector2 gatePosition, float delayOnZoomInCompleted = 1.0f, Action onZoomInCompleted = null, Action onZoomOutBackCompleted = null)
        {
            Reset();
            LockViewRegion();
            await CameraManager.Instance.MoveToAsync(gatePosition, this.GetCancellationTokenOnDestroy());
            await ZoomInCameraAsync(this.GetCancellationTokenOnDestroy());
            onZoomInCompleted?.Invoke();
            await UniTask.Delay((int)(delayOnZoomInCompleted * 1000), true);
            await ZoomToOriginalCameraAsync(this.GetCancellationTokenOnDestroy());
            await CameraManager.Instance.MoveBackToHeroesGroupAsync(this.GetCancellationTokenOnDestroy());
            UnlockViewRegion();
            onZoomOutBackCompleted?.Invoke();
        }

        public async UniTask PlayStartCampCamera()
        {
            Reset();
            LockViewRegion();
            await ZoomOutDeepCameraAsync(this.GetCancellationTokenOnDestroy());
            UnlockViewRegion();
        }

        public async UniTask PlayEndCampCamera()
        {
            Reset();
            LockViewRegion();
            await ZoomToOriginalCameraAsync(this.GetCancellationTokenOnDestroy());
            UnlockViewRegion();
        }

        private RarityType GetDungeonScrollResourceRarityType(ResourceType resourceType)
        {
            var requiredResourceTypeString = resourceType.ToString();
            foreach (RarityType rarityType in Enum.GetValues(typeof(RarityType)))
            {
                string rarityTypeString = rarityType.ToString();
                if (requiredResourceTypeString.EndsWith(rarityTypeString))
                {
                    return rarityType;
                }
            }
            return RarityType.Rare;
        }

        private void MoveBackClick()
        {
            MoveBackAsync().Forget();
        }

        private async UniTask MoveBackAsync()
        {
            await CameraManager.Instance.MoveBackToHeroesGroupAsync(this.GetCancellationTokenOnDestroy());
            Reset();
        }

        private void Reset()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
            UnlockViewRegion();
        }

        public async UniTask<SpriteAnimation> LoadHeroIdleSpriteAnimationsAsync(string heroId)
        {
            var cache = CachedHeroIdleSpriteAnimations;
            if (!cache.ContainsKey(heroId))
            {
                cache.Add(heroId, null);
                var spriteAnimation = await Addressables.LoadAssetAsync<SpriteAnimation>(string.Format(AddressableKey.HERO_IDLE_ANIMATION, heroId))
                    .WithCancellation(this.GetCancellationTokenOnDestroy());
                cache[heroId] = spriteAnimation;
                return spriteAnimation;
            }
            while (cache[heroId] == null)
            {
                await UniTask.DelayFrame(2, PlayerLoopTiming.TimeUpdate);
            }
            
            return cache[heroId];
        }
        
        #endregion Class Methods
    }
}