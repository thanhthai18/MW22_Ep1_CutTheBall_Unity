using System.Threading;
using Runtime.Message;
using UnityEngine;
using Runtime.Common.Singleton;
using Runtime.Definition;
using Cysharp.Threading.Tasks;
using Com.LuisPedroFonseca.ProCamera2D;
using Runtime.Tool.Easing;

namespace Runtime.Gameplay.Manager
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        #region Members

        [SerializeField] [Min(0.01f)] private float _cameraVisualTranslateDuration;
        [SerializeField] [Min(0.01f)] private float _cameraVisualShakeDuration;
        [SerializeField] [Min(0.01f)] private float _cameraVisualShakeDelayBetween;
        [SerializeField] [Min(0.01f)] private float _cameraVisualZoomSpeed;
        [SerializeField] [Min(0.01f)] private int _cameraVisualZoomDelta;
        [SerializeField] [Min(0.01f)] private int _cameraVisualZoomDeepDelta;
        [SerializeField] private ProCamera2D _proCamera;
        private float _originalOrthographicSize;
        private Transform _cachedMainHeroTransform;
        private Transform _cameraFollowedTargetTransform;

        #endregion Members

        #region API Methods

        private void Start()
            => _originalOrthographicSize = _proCamera.OrthographicSize;

        #endregion API Methods

        #region Class Methods

        public virtual void HandleDataLoaded(Transform cameraFollowedTargetTransform)
        {
            _cameraFollowedTargetTransform = cameraFollowedTargetTransform;
            TriggerFollowTarget();
        }

    

     

        public void TriggerUpdate()
            => TriggerFollowTarget();

        public virtual void LockCameraView()
            => _proCamera.LockView();

        public virtual void UnlockCameraView(bool isReset)
            => _proCamera.UnlockView();

        public async UniTask MoveToAsync(Vector2 moveToPosition, CancellationToken cancellationToken)
        {
            var originPosition = new Vector2(_proCamera.LocalPosition.x, _proCamera.LocalPosition.y);
            var moveDirection = (moveToPosition - originPosition).normalized;
            var moveDistance = (moveToPosition - originPosition).magnitude;
            var currentTranslateDuration = 0.0f;
            while (currentTranslateDuration <= _cameraVisualTranslateDuration)
            {
                currentTranslateDuration += Time.unscaledDeltaTime;
                var easeValue = Easing.EaseInOutExpo(0.0f, 1.0f, Mathf.Clamp01(currentTranslateDuration / _cameraVisualTranslateDuration));
                var interpolationValue = Mathf.Lerp(0, moveDistance, easeValue);
                var movePosition = originPosition + moveDirection * interpolationValue;
                _proCamera.MoveCameraInstantlyToPosition(movePosition);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
            _proCamera.MoveCameraInstantlyToPosition(moveToPosition);
        }

        public async UniTask ZoomToAsync(OrthographicType orthographicType, CancellationToken cancellationToken)
        {
            var hasDoneZooming = false;
            var currentZoomDelta = 0.0f;
            float orthographicCurrent = this._proCamera.OrthographicSize;
            float orthographicTarget = this._originalOrthographicSize * (int)orthographicType / 100.0f;
            int factor = orthographicCurrent > orthographicTarget ? -1 : 1;
            while (!hasDoneZooming)
            {
                currentZoomDelta += Time.unscaledDeltaTime * _cameraVisualZoomSpeed * factor;
                var newOrthographicSize = orthographicCurrent + currentZoomDelta;
                hasDoneZooming = factor > 0 ? newOrthographicSize >= orthographicTarget : newOrthographicSize <= orthographicTarget;
                _proCamera.SetOrthographicSize(newOrthographicSize);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
            _proCamera.SetOrthographicSize(orthographicTarget);
        }

        public async UniTask ShakeAsync(ShakeType shakeType, CancellationToken cancellationToken)
        {
            var currentShakeDuration = 0.0f;
            var originPosition = new Vector2(_proCamera.LocalPosition.x, _proCamera.LocalPosition.y);
            float shakeZoneValue = (int)shakeType / 100.0f;
            while (currentShakeDuration <= _cameraVisualShakeDuration)
            {
                currentShakeDuration += _cameraVisualShakeDelayBetween;
                var shakePosition = originPosition + new Vector2(Random.Range(-shakeZoneValue, shakeZoneValue),Random.Range(-shakeZoneValue, shakeZoneValue));
                _proCamera.MoveCameraInstantlyToPosition(shakePosition);
                await UniTask.Delay((int)(_cameraVisualShakeDelayBetween * 1000), ignoreTimeScale: true, cancellationToken: cancellationToken);
            }
            _proCamera.MoveCameraInstantlyToPosition(originPosition);
        }

        public async UniTask MoveBackToHeroesGroupAsync(CancellationToken cancellationToken)
        {
            var moveToPosition = _cameraFollowedTargetTransform.position;
            await MoveToAsync(moveToPosition, cancellationToken);
        }

        public void StopFollowTarget()
            => _proCamera.SetUpdateStatus(false);

        private void TriggerFollowTarget()
        {
            _proCamera.RemoveAllCameraTargets();
            _proCamera.AddCameraTarget(_cameraFollowedTargetTransform);
            _proCamera.CenterOnTargets();
            _proCamera.SetUpdateStatus(true);
        }

        #endregion Class Methods
    }
}