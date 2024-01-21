using Newtonsoft.Json;
using Runtime.Hash;
using Runtime.Common.Singleton;
using Runtime.PlayerManager;

namespace Runtime.Sync
{
    public sealed class SingleLoadResult<T> where T : BasePlayerComponent
    {
        public SingleLoadResult(string collectionName)
        {
            this.CollectionName = collectionName;
        }

        public string CollectionName { get; }

        public T SnapshotFromFirestore { get; set; }

        public T SnapshotFromDevice { get; set; }

        public bool IsConflicted()
        {
            if (this.SnapshotFromFirestore != null && this.SnapshotFromDevice != null)
            {
                var hashFirestore = Singleton.Of<HashService>().Hash(JsonConvert.SerializeObject(SnapshotFromFirestore));
                var hashDevice = Singleton.Of<HashService>().Hash(JsonConvert.SerializeObject(SnapshotFromDevice));

                if (hashFirestore != hashDevice)
                {
                    return true;
                }
            }

            return false;
        }

        public T GetSnapshot()
        {
            if (this.SnapshotFromDevice != null)
            {
                return this.SnapshotFromDevice;
            }

            if (this.SnapshotFromFirestore != null)
            {
                return this.SnapshotFromFirestore;
            }

            return null;
        }

        public T GetSnapshot(bool isSelectDevice)
        {
            if (isSelectDevice)
            {
                return this.SnapshotFromDevice;
            }

            return this.SnapshotFromFirestore;
        }
    }
}