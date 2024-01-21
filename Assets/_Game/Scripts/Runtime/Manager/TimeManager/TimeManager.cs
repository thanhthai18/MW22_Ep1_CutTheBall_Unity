using System;
using System.Collections.Generic;
using UnityEngine;
using Runtime.UI;
using Runtime.Common.Singleton;
using Runtime.Manager.Data;
using Runtime.Message;
using Runtime.Definition;
using Runtime.PlayerManager;
using Cysharp.Threading.Tasks;

namespace Runtime.Manager.Time
{
    public sealed class TimeManager : MonoSingleton<TimeManager>
    {
        #region Members

        [SerializeField] private TimeCounter _dayResetTimeCounter;
        [SerializeField] private TimeCounter _weekResetTimeCounter;
        [SerializeField] private TimeCounter _refreshGachaTimeCounter;
        private Dictionary<int, TimeCounter> _flashSaleTimeCounterDictionary = new();

        #endregion Members

        #region Properties

        public long NowInSeconds
        {
            get { return (long)((Time - Constant.JAN1St1970).TotalSeconds); }
        }

        public DateTime Time
        {
            get { return DateTime.UtcNow; }
        }

        public DateTime TimeOnTimeServerGot { get; set; }
        public float RealTimeOnTimeServerGot { get; set; }
        public bool HaveTimeServer { get; set; }

        public double TotalElapsedTimeSinceStartUp
        {
            get
            {
                var elapsedTime = Time - StartUpTime;
                return elapsedTime.TotalSeconds;
            }
        }

        private static DateTime StartUpTime { get; set; } = DateTime.UtcNow;
        private static bool HasSetStartUpTime { get; set; }
        private bool HasStarted { get; set; }

        #endregion Properties

        #region API Methods

#if UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!HasStarted)
                return;

            if (hasFocus)
                RunOnApplicationResponseAsync().Forget();
        }
#else
        private void OnApplicationPause(bool isPause)
        {
            if (!HasStarted)
                return;

            if (!isPause)
                RunOnApplicationResponseAsync().Forget();
        }
#endif

        #endregion API Methods

        #region Class Methods

        public void HandleDataLoaded()
        {
            HasStarted = true;
            StartTickAsync().Forget();
        }

        public DateTime ConverToDateTime(long secondsFromOriginTime)
        {
            var timeSpan = TimeSpan.FromSeconds(secondsFromOriginTime);
            return Constant.JAN1St1970 + timeSpan;
        }

     

        public void CancelLoadTimeFlashSale(int uniquePackId)
        {
            if (_flashSaleTimeCounterDictionary[uniquePackId] != null)
                _flashSaleTimeCounterDictionary[uniquePackId].Dispose();
        }

        private async UniTask RunOnApplicationResponseAsync()
        {
            HaveTimeServer = false;
            Singleton.Of<PlayerService>().CheckResetDaily();
            Singleton.Of<PlayerService>().CheckResetWeekly();
        }

        private async UniTask StartTickAsync()
        {
            if (!HasSetStartUpTime)
            {
                HasSetStartUpTime = true;
                StartUpTime = Time;
            }

            Singleton.Of<PlayerService>().CheckResetDaily();
            Singleton.Of<PlayerService>().CheckResetWeekly();
            InitDayResetTimeCounter();
            InitWeekResetTimeCounter();
        }

        private void InitDayResetTimeCounter()
        {
            var timeRemain = Constant.TIME_OF_A_DAY_IN_SECONDS - NowInSeconds % Constant.TIME_OF_A_DAY_IN_SECONDS;
            _dayResetTimeCounter.Cancel();
            _dayResetTimeCounter.Tick(NowInSeconds + timeRemain, onFinishTimeRemain: OnFinishedDayResetTimeCounter);
        }

        private void InitWeekResetTimeCounter()
        {
            var timeRemain = Constant.TIME_OF_A_WEEK_IN_SECONDS - NowInSeconds % Constant.TIME_OF_A_WEEK_IN_SECONDS;
            _weekResetTimeCounter.Cancel();
            _weekResetTimeCounter.Tick(NowInSeconds + timeRemain, onFinishTimeRemain: OnFinishedWeekResetTimeCounter);
        }

        private void OnFinishedDayResetTimeCounter()
            => CheckFinishDayResetTimeCounterAsync().Forget();

        private void OnFinishedWeekResetTimeCounter()
            => CheckFinishWeekResetTimeCounterAsync().Forget();

        private async UniTask CheckFinishDayResetTimeCounterAsync()
        {
            Singleton.Of<PlayerService>().CheckResetDaily();
            InitDayResetTimeCounter();
        }

        private async UniTask CheckFinishWeekResetTimeCounterAsync()
        {
            Singleton.Of<PlayerService>().CheckResetWeekly();
            InitWeekResetTimeCounter();
        }

        #endregion Class Methods
    }
}