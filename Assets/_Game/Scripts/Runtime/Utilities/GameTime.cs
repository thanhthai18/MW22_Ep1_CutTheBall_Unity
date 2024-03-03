using System;
using UnityEngine;

namespace Runtime.Utilities
{
    public sealed class GameTime
    {
        #region Members

        private static readonly DateTime s_jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime s_serverTimeFromStart;
        private static double s_timeSyncInDouble;

        #endregion Members

        #region Properties

        public static bool IsSyncedTimeWithServer { get; set; }

        public static DateTime ServerUtcNow
        {
            get
            {
                if (!IsSyncedTimeWithServer)
                    return DateTime.UtcNow;
                else
                {
                    var timeAdd = Time.realtimeSinceStartupAsDouble - s_timeSyncInDouble;
                    var timeSpan = TimeSpan.FromSeconds(timeAdd);
                    return s_serverTimeFromStart + timeSpan;
                }
            }
        }

        public static long NowInWeek
        {
            get { return Mathf.CeilToInt((long)(ServerUtcNow - s_jan1St1970).TotalDays / 7f); }
        }

        public static long NowInSeconds
        {
            get { return (long)((ServerUtcNow - s_jan1St1970).TotalSeconds); }
        }

        public static long LocalNowInSeconds
        {
            get { return (long)(DateTime.UtcNow - s_jan1St1970).TotalSeconds; }
        }

        public static long NowInDays
        {
            get { return (long)(ServerUtcNow - s_jan1St1970).TotalDays; }
        }

        public static long NowInMilliseconds
        {
            get { return (long)(ServerUtcNow - s_jan1St1970).TotalMilliseconds; }
        }

        #endregion Properties

        #region Class Methods

        public static void Setup(long serverTimeTotalSecondsFromOriginTime, bool isSync)
        {
            IsSyncedTimeWithServer = isSync;
            var timeSpan = TimeSpan.FromSeconds(serverTimeTotalSecondsFromOriginTime);
            s_serverTimeFromStart = s_jan1St1970 + timeSpan;
            s_timeSyncInDouble = Time.realtimeSinceStartupAsDouble;
        }

        public static DateTime ConverToDateTime(long secondsFromOriginTime)
        {
            var timeSpan = TimeSpan.FromSeconds(secondsFromOriginTime);
            return s_jan1St1970 + timeSpan;
        }

        public static TimeInPart ConverToTimeInPart(long totalSeconds)
            => new TimeInPart(totalSeconds);

        #endregion Class Methods
    }
}