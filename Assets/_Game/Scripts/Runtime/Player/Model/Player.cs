using System;

namespace Runtime.PlayerManager
{
    public sealed class Player
    {
        public PlayerAuthentication Auth;
        public PlayerBasicInfo BasicInfo;
        public PlayerSetting Setting;

        public int ResultCode { get; set; }

        public Player()
        {
        }

        public void ResetNewDay(DateTime dateTime)
        {
            Auth.ResetDaily(dateTime);
            BasicInfo.ResetDaily(dateTime);
            Setting.ResetDaily(dateTime);
        }

        public void ResetNewWeek(DateTime dateTime)
        {
            Auth.ResetWeekly(dateTime);
            BasicInfo.ResetWeekly(dateTime);
            Setting.ResetWeekly(dateTime);
        }
    }
}