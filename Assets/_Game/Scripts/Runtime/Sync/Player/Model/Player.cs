using System;
using Runtime.Manager.Data;

namespace Runtime.PlayerManager
{
    public sealed class Player
    {
        #region Members

        private PlayerAuthentication _auth;
        private PlayerBasicInfo _basicInfo;
        private PlayerSetting _setting;

        #endregion Members

        #region Properties

        public PlayerAuthentication Auth => _auth;
        public PlayerBasicInfo BasicInfo => _basicInfo;
        public PlayerSetting Setting => _setting;


        #endregion Properties

        #region Class Methods

        public Player() { }

        public void SetAuth(PlayerAuthentication auth)
            => _auth = auth;

        public void SetBasicInfo(PlayerBasicInfo basicInfo)
            => _basicInfo = basicInfo;

        public void SetSetting(PlayerSetting setting)
            => _setting = setting;

        public void ResetNewDay(DateTime dateTime)
        {
            _auth.ResetDaily(dateTime);
            _basicInfo.ResetDaily(dateTime);
            _setting.ResetDaily(dateTime);
        }

        public void ResetNewWeek(DateTime dateTime)
        {
            _auth.ResetWeekly(dateTime);
            _basicInfo.ResetWeekly(dateTime);
            _setting.ResetWeekly(dateTime);
        }

        #endregion Properties
    }
}