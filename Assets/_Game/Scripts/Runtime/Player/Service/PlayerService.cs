using UnityEngine;
using System.Collections.Generic;
using Runtime.Definition;
using Runtime.Localization;
using Runtime.Manager.Data;
using Runtime.Manager.Time;
using Runtime.Manager.Toast;
using Runtime.Message;
using Cysharp.Threading.Tasks;
using Runtime.Sync;

namespace Runtime.PlayerManager
{
    public class PlayerService
    {
        public Player Player { get; set; }
        public bool IsNewPlayer { get; set; }
        public SaveLoadService<PlayerAuthentication> AuthService { get; set; }
        public SaveLoadService<PlayerBasicInfo> BasicInfoService { get; set; }
        public SaveLoadService<PlayerSetting> SettingService { get; set; }
    

        public void Init()
        {
            Player = new Player();
            AuthService = new SaveLoadService<PlayerAuthentication>(CollectionName.PLAYER_AUTHENTICATION);
            BasicInfoService = new SaveLoadService<PlayerBasicInfo>(CollectionName.PLAYER_BASIC_INFO);
            SettingService = new SaveLoadService<PlayerSetting>(CollectionName.PLAYER_SETTING);
            SaveDataInterval().Forget();
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

        #region Create Player

        public async UniTask LoadFromFirestore(string playerId)
        {
            await this.AuthService.Load(playerId);
            IsNewPlayer = GetResultFireStore<PlayerAuthentication>(AuthService, out Player.Auth);
            if (!IsNewPlayer)
            {
                var tasks = new List<UniTask> {
                    this.BasicInfoService.Load(playerId),
                    this.SettingService.Load(playerId),
                };

                await UniTask.WhenAll(tasks);
            }
            else
            {
                BasicInfoService.Init(playerId);
                SettingService.Init(playerId);
            }

            GetResultFireStore<PlayerBasicInfo>(BasicInfoService, out Player.BasicInfo);
            GetResultFireStore<PlayerSetting>(SettingService, out Player.Setting);
        }


        public void FillPlayerId(string playerId, int authMethod)
        {
            if (!string.IsNullOrEmpty(Player.Auth.PlayerId))
            {
                return;
            }

            Player.Auth.LoginMethod = authMethod;
            Player.Auth.PlayerId = playerId;

            Player.BasicInfo.PlayerId = playerId;
            Player.Setting.PlayerId = playerId;
         
            AuthService.Init(playerId);
            BasicInfoService.Init(playerId);
            SettingService.Init(playerId);
        }

        public void LoadPlayerFromLocal()
        {
            AuthService.LoadFromDevice();
            BasicInfoService.LoadFromDevice();
            SettingService.LoadFromDevice();

            IsNewPlayer = GetResult<PlayerAuthentication>(AuthService, out Player.Auth);
            GetResult<PlayerBasicInfo>(BasicInfoService, out Player.BasicInfo);
            GetResult<PlayerSetting>(SettingService, out Player.Setting);

            var playerId = Player.Auth.PlayerId;

            AuthService.Init(playerId);
            BasicInfoService.Init(playerId);
            SettingService.Init(playerId);
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
                Messenger.Publish(new GameStateChangedMessage(GameStateEventType.NewWeekReset));
            }
        }

        public async UniTask Save(bool isForceToFirestore = false, bool showToast = false)
        {
            AuthService.FlagToSave();
            BasicInfoService.FlagToSave();
            SettingService.FlagToSave();

            if (isForceToFirestore)
            {
                SaveToDevice();
                var isLogon = !string.IsNullOrEmpty(Player.Auth.PlayerId);
                if (isLogon && Application.internetReachability != NetworkReachability.NotReachable)
                {
                    bool resultSave = false;
                    resultSave = await AuthService.SaveToFirebase(Player.Auth);
                    resultSave &= await BasicInfoService.SaveToFirebase(Player.BasicInfo);
                    resultSave &= await SettingService.SaveToFirebase(Player.Setting);

                    if (showToast)
                    {
                        if (resultSave)
                        {
                            ToastManager.Instance.Show(
                                LocalizationUtils.GetToastLocalized(LocalizeKeys.SAVE_DATA_SUCCESS),
                                ToastVisualType.Text);
                        }
                        else
                        {
                            ToastManager.Instance.Show(
                                LocalizationUtils.GetToastLocalized(LocalizeKeys.SAVE_DATA_FAILED),
                                ToastVisualType.Text);
                        }
                    }
                }
            }
        }

        private void SaveToDevice()
        {
            AuthService.SaveToDevice(Player.Auth);
            BasicInfoService.SaveToDevice(Player.BasicInfo);
            SettingService.SaveToDevice(Player.Setting);
        }

        #region Generic

        private bool GetResult<T>(SaveLoadService<T> saveLoadService, out T outValue)
            where T : BasePlayerComponent, new()
        {
            if (saveLoadService.LoadResult.GetSnapshot() != null)
            {
                outValue = saveLoadService.LoadResult.GetSnapshot();
                return false;
            }
            else
            {
                outValue = new T();
                outValue.InitNewPlayer();
                saveLoadService.FlagToSave();
                saveLoadService.SaveToDevice(outValue);
                return true;
            }
        }

        private bool GetResultFireStore<T>(SaveLoadService<T> saveLoadService, out T outValue)
            where T : BasePlayerComponent, new()
        {
            if (saveLoadService.LoadResult.GetSnapshot() != null)
            {
                outValue = saveLoadService.LoadResult.GetSnapshot();
                return false;
            }
            else
            {
                outValue = new T() { PlayerId = saveLoadService.PlayerID };
                outValue.InitNewPlayer();
                saveLoadService.SaveToFirebase(outValue);
                return true;
            }
        }

        #endregion

        #endregion

        #region Function

        #endregion
    }
}