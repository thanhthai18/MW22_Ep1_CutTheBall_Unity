using UnityEngine;
using System.Collections.Generic;
using Runtime.Definition;
using Runtime.Localization;
using Runtime.Manager.Time;
using Runtime.Manager.Toast;
using Runtime.Message;
using Runtime.Sync;
using Cysharp.Threading.Tasks;

namespace Runtime.PlayerManager
{
    public class PlayerService
    {
        #region Members

        private PlayerSaveLoadService<PlayerAuthentication> _authService;
        private PlayerSaveLoadService<PlayerBasicInfo> _basicInfoService;
        private PlayerSaveLoadService<PlayerSetting> _settingService;

        #endregion Members

        #region Properties

        public bool IsAuthenticatedNewPlayer { get; private set; }
        public Player Player { get; private set; }

        #endregion Properties

        #region Class Methods

        public void Init()
        {
            Player = new Player();
            _authService = new PlayerSaveLoadService<PlayerAuthentication>(CollectionName.PLAYER_AUTHENTICATION);
            _basicInfoService = new PlayerSaveLoadService<PlayerBasicInfo>(CollectionName.PLAYER_BASIC_INFO);
            _settingService = new PlayerSaveLoadService<PlayerSetting>(CollectionName.PLAYER_SETTING);
            LoadPlayerFromDevice();
            SaveDataInterval().Forget();
        }

        public async UniTask LoadFromFirestore(string playerId)
        {
            await _authService.Load(playerId);
            var playerAuth = CheckGetAuthDataFromFireStore();
            Player.SetAuth(playerAuth);
            if (!IsAuthenticatedNewPlayer)
            {
                var tasks = new List<UniTask> {
                    _basicInfoService.Load(playerId),
                    _settingService.Load(playerId),
                };
                await UniTask.WhenAll(tasks);
            }
            else
            {
                _basicInfoService.Init(playerId);
                _settingService.Init(playerId);
            }

            var playerBasicInfo = GetDataFromFireStore<PlayerBasicInfo>(_basicInfoService);
            Player.SetBasicInfo(playerBasicInfo);

            var playerSetting = GetDataFromFireStore<PlayerSetting>(_settingService);
            Player.SetSetting(playerSetting);
         
        }

        public void FillPlayerId(string playerId, int authMethod)
        {
            // if (!string.IsNullOrEmpty(Player.Auth.PlayerId) && Player.Auth.LoginMethod > AuthMethod.LOGIN_GUEST)
            //     return;
            //
            // Player.Auth.LoginMethod = authMethod;
            // Player.Auth.PlayerId = playerId;
            // Player.BasicInfo.PlayerId = playerId;
            // Player.Setting.PlayerId = playerId;
            //
            // _authService.Init(playerId);
            // _basicInfoService.Init(playerId);
            // _settingService.Init(playerId);
        }

        public void CheckResetDaily()
        {
            var loginTimeInSeconds = (long)((Player.Auth.LoginTime - Constant.JAN1St1970).TotalSeconds);
            long lastDay = loginTimeInSeconds / Constant.TIME_OF_A_DAY_IN_SECONDS;
            long currentDay = TimeManager.Instance.NowInSeconds / Constant.TIME_OF_A_DAY_IN_SECONDS;
            if (lastDay < currentDay)
            {
                Player.ResetNewDay(TimeManager.Instance.Time);
                Save().Forget();
                //Messenger.Publish(new GameActionNotifiedMessage(GameActionType.Login, "-1", 1));
                Messenger.Publish(new GameStateChangedMessage(GameStateEventType.NewDayReset));
            }
        }

        public void CheckResetWeekly()
        {
            var weekTimeInSeconds = (long)((Player.Auth.WeekTime - Constant.JAN1St1970).TotalSeconds);
            long lastWeek = weekTimeInSeconds / Constant.TIME_OF_A_WEEK_IN_SECONDS;
            long currentWeek = TimeManager.Instance.NowInSeconds / Constant.TIME_OF_A_WEEK_IN_SECONDS;
            if (lastWeek < currentWeek)
            {
                Player.ResetNewWeek(TimeManager.Instance.Time);
                Save().Forget();
                // if ((TimeManager.Instance.Time - Player.Auth.LoginTime).TotalDays < 1.0d)
                //     Messenger.Publish(new GameActionNotifiedMessage(GameActionType.Login, "-1", 1));
                Messenger.Publish(new GameStateChangedMessage(GameStateEventType.NewWeekReset));
            }
        }

        public async UniTask Save(bool isForceToFirestore = false, bool showToast = false)
        {
            _authService.FlagToSave();
            _basicInfoService.FlagToSave();
            _settingService.FlagToSave();

            if (isForceToFirestore)
            {
                SaveToDevice();
                var isLogon = !string.IsNullOrEmpty(Player.Auth.PlayerId);
                if (isLogon && Application.internetReachability != NetworkReachability.NotReachable)
                {
                    bool resultSave = false;
                    resultSave = await _authService.SaveToFirebase(Player.Auth);
                    resultSave &= await _basicInfoService.SaveToFirebase(Player.BasicInfo);
                    resultSave &= await _settingService.SaveToFirebase(Player.Setting);

                    if (showToast)
                    {
                        if (resultSave)
                        {
                            ToastManager.Instance.Show(LocalizationUtils.GetToastLocalized(LocalizeKeys.SAVE_DATA_SUCCESS),
                                                       ToastVisualType.Text);
                        }
                        else
                        {
                            ToastManager.Instance.Show(LocalizationUtils.GetToastLocalized(LocalizeKeys.SAVE_DATA_FAILED),
                                                       ToastVisualType.Text);
                        }
                    }
                }
            }
        }

        private async UniTask SaveDataInterval()
        {
            long timeInterval = 0;
            while (true)
            {
                SaveToDevice();
                timeInterval += Constant.TIME_SAVE_DATA;
                if (timeInterval > 1000)
                {
                    timeInterval = timeInterval % 1000;
                }
                await UniTask.Delay(Constant.TIME_SAVE_DATA, true);
            }
        }

        private void LoadPlayerFromDevice()
        {
            _authService.LoadFromDevice();
            _basicInfoService.LoadFromDevice();
            _settingService.LoadFromDevice();

            var playerAuth = CheckGetAuthDataFromDevice();
            Player.SetAuth(playerAuth);

            var playerBasicInfo = GetDataFromDevice<PlayerBasicInfo>(_basicInfoService);
            Player.SetBasicInfo(playerBasicInfo);

            var playerSetting = GetDataFromDevice<PlayerSetting>(_settingService);
            Player.SetSetting(playerSetting);

            var playerId = Player.Auth.PlayerId;
            
            _authService.Init(playerId);
            _basicInfoService.Init(playerId);
            _settingService.Init(playerId);
        }

        private void SaveToDevice()
        {
            _authService.SaveToDevice(Player.Auth);
            _basicInfoService.SaveToDevice(Player.BasicInfo);
            _settingService.SaveToDevice(Player.Setting);
        }

        private PlayerAuthentication CheckGetAuthDataFromDevice()
        {
            if (_authService.LoadResult.GetSnapshot() != null)
            {
                var outValue = _authService.LoadResult.GetSnapshot();
                IsAuthenticatedNewPlayer = false;
                return outValue;
            }
            else
            {
                var outValue = new PlayerAuthentication();
                outValue.InitNewPlayer();
                _authService.FlagToSave();
                _authService.SaveToDevice(outValue);
                IsAuthenticatedNewPlayer = true;
                return outValue;
            }
        }

        private T GetDataFromDevice<T>(PlayerSaveLoadService<T> saveLoadService) where T : PlayerBase, new()
        {
            if (saveLoadService.LoadResult.GetSnapshot() != null)
            {
                var outValue = saveLoadService.LoadResult.GetSnapshot();
                return outValue;
            }
            else
            {
                var outValue = new T();
                outValue.InitNewPlayer();
                saveLoadService.FlagToSave();
                saveLoadService.SaveToDevice(outValue);
                return outValue;
            }
        }

        private PlayerAuthentication CheckGetAuthDataFromFireStore()
        {
            if (_authService.LoadResult.GetSnapshot() != null)
            {
                var outValue = _authService.LoadResult.GetSnapshot();
                IsAuthenticatedNewPlayer = false;
                return outValue;
            }
            else
            {
                var outValue = new PlayerAuthentication() { PlayerId = _authService.PlayerID };
                outValue.InitNewPlayer();
                _authService.SaveToFirebase(outValue).Forget();
                IsAuthenticatedNewPlayer = true;
                return outValue;
            }
        }

        private T GetDataFromFireStore<T>(PlayerSaveLoadService<T> saveLoadService) where T : PlayerBase, new()
        {
            if (saveLoadService.LoadResult.GetSnapshot() != null)
            {
                var outValue = saveLoadService.LoadResult.GetSnapshot();
                return outValue;
            }
            else
            {
                var outValue = new T() { PlayerId = saveLoadService.PlayerID };
                outValue.InitNewPlayer();
                saveLoadService.SaveToFirebase(outValue).Forget();
                return outValue;
            }
        }

        #endregion Class Methods
    }
}