// using System;
// using System.Collections.Generic;
// using CodeStage.AntiCheat.Storage;
// using Cysharp.Threading.Tasks;
// using Newtonsoft.Json;
// using Runtime.Authentication;
// using Runtime.Common.Singleton;
// using Runtime.Definition;
// using Runtime.Manager.Data;
// using Runtime.Manager.Game;
// using Runtime.Manager.TimeServerManager;
// using Runtime.Message;
// using Runtime.PlayerManager;
//
// namespace Runtime.Sync
// {
//     public sealed class SnapshotService : PersistentMonoSingleton<SnapshotService>
//     {
//         #region Members
//
//         private bool _isStart;
//         private DateTime _lastSave;
//
//         #endregion Members
//
//         #region Properties
//
//         public SaveLoadService<PlayerAuth> AuthService { get; set; }
//
//         #endregion Properties
//
//         #region Methods
//
//         public async UniTask InitAsync()
//         {
//             _isStart = false;
//             _lastSave = TimeManager.Instance.Time.AddDays(-1);
//
//             this.AuthService = new SaveLoadService<PlayerAuth>(CollectionName.PLAYER_AUTHENTICATION);
//
//             Messenger.Subscribe<LoadingStatusFinishedMessage>(OnLoadingFinished);
//
//             InitObscuredFile();
//
//             await UniTask.Yield();
//         }
//
//         #endregion Methods
//
//         private void InitObscuredFile()
//         {
//             var password = GameConfig.Instance.password;
// #if UNITY_EDITOR
//             password = string.Empty;
// #endif
//
//             var settings = new ObscuredFileSettings(
//                 new EncryptionSettings(password),
//                 new DeviceLockSettings(),
//                 ObscuredFileLocation.PersistentData,
//                 true, false);
//
//             ObscuredFilePrefs.Init(settings, true);
//         }
//
//         #region Select snapshot
//
//         public async UniTask<SnapshotSelectOutbound> SelectSnapshot(SnapshotSelectInbound inbound)
//         {
//             var result = new SnapshotSelectOutbound();
//
//             await PlayerService.Instance.LoadPlayer(inbound.LoadResult, inbound.IsSelectDevice);
//             Save();
//
//             return result.WithCode(LogicCode.SUCCESS);
//         }
//
//         #endregion
//
//         #region OnClick Save
//
//         public UniTask OnClickSave()
//         {
//             if (!this._isStart)
//             {
//                 return UniTask.CompletedTask;
//             }
//
//             DateTime now = TimeManager.Instance.Time;
//             if ((now - this._lastSave).TotalSeconds < SyncConstant.SAVE_INTERVAL)
//             {
//                 return UniTask.CompletedTask;
//             }
//
//             this._lastSave = now;
//
//             Flush();
//             return UniTask.CompletedTask;
//         }
//
//         private void OnLoadingFinished(LoadingStatusFinishedMessage message)
//         {
//             this._isStart = true;
//         }
//
//         #endregion
//
//         #region Save
//
//         private void Flush()
//         {
//             Player player = PlayerService.Instance.Player;
//             if (player != null && player.Auth != null)
//             {
//                 player.Auth.LocalData = JsonConvert.SerializeObject(LocalDataManager.Instance.ReadOnlySavedLocalData);
//
//                 this.AuthService.Update(player.Auth);
//
//                 ObscuredFilePrefs.Save();
//                 ObscuredPrefs.Save();
//             }
//         }
//
//         public void Save()
//         {
//             this.AuthService.Save();
//         }
//
//         #endregion
//
//         #region Load
//
//         public async UniTask<PlayerLoadResult> Load(string playerId)
//         {
//             var tasks = new List<UniTask> {
//                 this.AuthService.Load(playerId)
//             };
//
//             await UniTask.WhenAll(tasks);
//
//             return new PlayerLoadResult {
//                 AuthResult = this.AuthService.LoadResult
//             };
//         }
//
//         #endregion
//     }
// }