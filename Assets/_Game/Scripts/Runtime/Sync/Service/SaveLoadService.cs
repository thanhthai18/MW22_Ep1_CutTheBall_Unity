using Cysharp.Threading.Tasks;
using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Sync
{
    public sealed class SaveLoadService<T> where T : BasePlayerComponent
    {
        private readonly string _collectionName;
        private string _playerId;

        public string PlayerID => this._playerId;
        public FirestoreSaveService<T> FirestoreSaveService { get; }

        public DeviceSaveService<T> DeviceSaveService { get; }
        public DeviceLoadService<T> DeviceLoadService { get; }

        public SingleLoadResult<T> LoadResult { get; private set; }

        public SaveLoadService(string collectionName)
        {
            this._collectionName = collectionName;

            this.FirestoreSaveService = new FirestoreSaveService<T> { CollectionName = _collectionName };
            this.DeviceSaveService = new DeviceSaveService<T> { CollectionName = _collectionName };

            this.DeviceLoadService = new DeviceLoadService<T> { CollectionName = _collectionName };
            this.LoadResult = new SingleLoadResult<T>(_collectionName);
        }

        public void Init(string playerId)
        {
            this._playerId = playerId;

            this.FirestoreSaveService.PlayerId = playerId;
            this.DeviceSaveService.PlayerId = playerId;

            this.DeviceLoadService.PlayerId = playerId;
        }

        #region Save

        public void FlagToSave()
        {
            this.DeviceSaveService.IsNeedSave = true;
            this.FirestoreSaveService.IsNeedSave = true;
        }

        public async UniTask<bool> SaveToFirebase(T data)
        {
            return await this.FirestoreSaveService.Save(data);
        }

        #endregion

        #region Load

        public async UniTask Load(string playerId)
        {
            Init(playerId);
            this.LoadResult = new SingleLoadResult<T>(this._collectionName);

            await LoadFromFirestore();
        }

        public void LoadFromDevice()
        {
            T snapshotFromDevice = this.DeviceLoadService.Load();
            if (snapshotFromDevice != null)
            {
                this.LoadResult.SnapshotFromDevice = snapshotFromDevice;
            }
        }

        private async UniTask LoadFromFirestore()
        {
            Debug.Log("Load from firestore: " + typeof(T).Name);
            this.FirestoreSaveService.IsNeedSave = true;
        }

        public void SaveToDevice(T data)
        {
            this.DeviceSaveService.Save(data);
        }

        #endregion
    }
}