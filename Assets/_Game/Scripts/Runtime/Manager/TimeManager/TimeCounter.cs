using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Runtime.Manager.Time
{
    public sealed class TimeCounter : MonoBehaviour, IDisposable
    {
        #region Members

        private CancellationTokenSource _timeRemainCancellationTokenSource;

        #endregion Members

        #region API Methods

        private void OnDestroy()
            => Dispose();

        #endregion API Methods

        #region Class Methods

        public void Tick(long endTimeInSeconds, Action onFinishTimeRemain = null)
        {
            var timeRemain = TimeManager.Instance.ConverToDateTime(endTimeInSeconds) - TimeManager.Instance.Time;
            if (timeRemain.TotalSeconds >= 1)
                StartCountdown(endTimeInSeconds, onFinishTimeRemain, _timeRemainCancellationTokenSource.Token).Forget();
            else
                OnFinishCountTimeAsync(onFinishTimeRemain, _timeRemainCancellationTokenSource.Token).Forget();
        }

        public void Cancel()
        {
            if (_timeRemainCancellationTokenSource != null)
            {
                _timeRemainCancellationTokenSource.Cancel();
                _timeRemainCancellationTokenSource.Dispose();
            }
            _timeRemainCancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (_timeRemainCancellationTokenSource != null)
            {
                _timeRemainCancellationTokenSource.Cancel();
                _timeRemainCancellationTokenSource.Dispose();
                _timeRemainCancellationTokenSource = null;
            }
        }

        private async UniTaskVoid OnFinishCountTimeAsync(Action onFinishTimeRemain, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: cancellationToken, ignoreTimeScale: true);
            onFinishTimeRemain?.Invoke();
        }

        private async UniTaskVoid StartCountdown(long endTimeInSeconds, Action onFinishTimeRemain, CancellationToken cancellationToken)
        {
            var timeRemain = TimeManager.Instance.ConverToDateTime(endTimeInSeconds) - TimeManager.Instance.Time;
            while (TimeManager.Instance.NowInSeconds < endTimeInSeconds)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken, ignoreTimeScale: true);
                timeRemain = TimeManager.Instance.ConverToDateTime(endTimeInSeconds) - TimeManager.Instance.Time;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken, ignoreTimeScale: true);
            onFinishTimeRemain?.Invoke();
        }

        #endregion Class Methods
    }
}