using Runtime.Common.Singleton;
using Runtime.Definition;
using Runtime.Message;
using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Manager.Data
{
    public partial class LocalDataManager : PersistentMonoSingleton<LocalDataManager>
    {
        #region Properties

        public string SelectedLanguage => string.IsNullOrEmpty(Singleton.Of<PlayerService>().Player.Setting.selectedLanguage) ? Application.systemLanguage.ToString() : Singleton.Of<PlayerService>().Player.Setting.selectedLanguage;
        public int PlayerSkinId => Singleton.Of<PlayerService>().Player.BasicInfo.PlayerSkinId;

        public string CurrentPlayerId
        {
            get
            {
                string playerID = Singleton.Of<PlayerService>().Player.Auth.PlayerId;
                return playerID;
            }
        }

        public bool HasEnableMusic => Singleton.Of<PlayerService>().Player.Setting.hasEnableMusic;
        public bool HasEnableSound => Singleton.Of<PlayerService>().Player.Setting.hasEnableSound;
        public bool HasLoadServerData => PlayerPrefs.GetInt(Constant.HAS_LOAD_SERVER_DATA, 0) > 0;

        #endregion Properties

        #region Class Methods

        public void SetPlayerSkinId(SkinType skinType)
        {
            var skinTypeId = (int)skinType;
            if (skinTypeId == Singleton.Of<PlayerService>().Player.BasicInfo.PlayerSkinId)
                return;
            Singleton.Of<PlayerService>().Player.BasicInfo.PlayerSkinId = skinTypeId;
            Save();
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.PlayerSkinChanged));
        }

        #endregion Class Methods
    }
}