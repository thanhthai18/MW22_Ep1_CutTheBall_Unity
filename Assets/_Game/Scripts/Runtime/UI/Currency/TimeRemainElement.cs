using System;
using System.Threading;
using UnityEngine;
using Runtime.Utilities;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Runtime.UI
{
    public class TimeRemainElement : MonoBehaviour, IDisposable
    {
        #region Members

        private const string DEFAULT_NORMAL_COLOR_HEX = "#ffffff";
        private const string DEFAULT_WARNING_COLOR_HEX = "#f72900";
        private const int WARNING_HOURS = 3;

        [SerializeField]
        private string _formatKey;
        [SerializeField]
        private TextMeshProUGUI _timeRemainText;
        [SerializeField]
        private bool _useCustomColor;
        [SerializeField]
        [ShowIf(nameof(_useCustomColor))]
        private string _warningColorHex;
        [SerializeField]
        [ShowIf(nameof(_useCustomColor))]
        private string _normalColorHex;
        [SerializeField]
        [ShowIf(nameof(_useCustomColor))]
        private string _textTitleColor;

        private CancellationTokenSource _timeRemainCancellationTokenSource;
        private Action<long> _onCount;

        private TimeFormatType _timeRemainFormatType;
        private string _formatKeyString;

        #endregion Members

        #region Class Methods

        public void ExtraSetup(Action<long> onCount)
        {
            _onCount = onCount;
        }

        public void SetTimeRemain(long endTimeInSeconds, TimeFormatType formatType = TimeFormatType.AutoDetect, Action onFinishTimeRemain = null)
        {
            //_formatKeyString = string.IsNullOrEmpty(_formatKey) ? "{0}" : LocalizationManager.GetLocalize(LocalizeTable.GENERAL, _formatKey);
            _timeRemainFormatType = formatType;

            var timeRemain = GameTime.ConverToDateTime(endTimeInSeconds) - GameTime.ServerUtcNow;
            if(_timeRemainText)
                _timeRemainText.text = GetTextRemain((long)timeRemain.TotalSeconds, formatType);

            _timeRemainCancellationTokenSource?.Cancel();
            if (timeRemain.TotalSeconds >= 1)
            {
                _timeRemainCancellationTokenSource = new CancellationTokenSource();
                StartCountdown(endTimeInSeconds, formatType, onFinishTimeRemain, _timeRemainCancellationTokenSource.Token).Forget();
            }   
            else
            {
                _timeRemainCancellationTokenSource = new CancellationTokenSource();
                OnFinishCountTimeAsync(onFinishTimeRemain, _timeRemainCancellationTokenSource.Token).Forget();
            }
        }

        private async UniTaskVoid OnFinishCountTimeAsync(Action onFinishTimeRemain, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token, ignoreTimeScale: true);
            onFinishTimeRemain?.Invoke();
        }

        private async UniTaskVoid StartCountdown(long endTimeInSeconds, TimeFormatType formatType, Action onFinishTimeRemain, CancellationToken token)
        {
            var timeRemain = GameTime.ConverToDateTime(endTimeInSeconds) - GameTime.ServerUtcNow;
            _onCount?.Invoke((long)timeRemain.TotalSeconds);

            while (GameTime.NowInSeconds < endTimeInSeconds)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token, ignoreTimeScale: true);
                timeRemain = GameTime.ConverToDateTime(endTimeInSeconds) - GameTime.ServerUtcNow;
                _onCount?.Invoke((long)timeRemain.TotalSeconds);
                if (_timeRemainText)
                    _timeRemainText.text = GetTextRemain((long)timeRemain.TotalSeconds, formatType);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token, ignoreTimeScale: true);
            onFinishTimeRemain?.Invoke();
        }

        private string GetTextRemain(long timeRemain, TimeFormatType formatType)
        {
            var (timeFormat, isWarning) = TimeUtility.FormatTimeDuration(timeRemain, formatType, WARNING_HOURS);
            var returnString = string.Empty;

            if (_useCustomColor)
            {
                if (isWarning)
                    returnString = string.IsNullOrEmpty(_warningColorHex) ? $"<color=red>{timeFormat}</color>" : $"<color={_warningColorHex}>{timeFormat}</color>";
                else
                    returnString = string.IsNullOrEmpty(_normalColorHex) ? $"<color=green>{timeFormat}</color>" : $"<color={_normalColorHex}>{timeFormat}</color>";
            }
            else
            {
                if (isWarning)
                    returnString = string.IsNullOrEmpty(DEFAULT_WARNING_COLOR_HEX) ? $"<color=red>{timeFormat}</color>" : $"<color={DEFAULT_WARNING_COLOR_HEX}>{timeFormat}</color>";
                else
                    returnString = string.IsNullOrEmpty(DEFAULT_NORMAL_COLOR_HEX) ? $"<color=green>{timeFormat}</color>" : $"<color={DEFAULT_NORMAL_COLOR_HEX}>{timeFormat}</color>";
            }

            if(_formatKeyString != null)
                return string.Format(_formatKeyString, returnString, _textTitleColor);
            return returnString;
        }

        public void Dispose()
        {
            _timeRemainCancellationTokenSource?.Cancel();
            if (_timeRemainText)
                _timeRemainText.text = GetTextRemain(0, _timeRemainFormatType);
        }

        #endregion Class Methods
    }
}