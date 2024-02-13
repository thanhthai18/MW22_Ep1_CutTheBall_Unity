using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Runtime.PlayerManager;

namespace Runtime.Sync
{
    public sealed class FirestoreLoadService<T> where T : PlayerBase
    {
        public string CollectionName { get; set; }
        public string PlayerId { get; set; }

        // public async UniTask<T> Load()
        // {
        //     DocumentSnapshot snapshotData = await FirebaseManager.Instance.GetDocumentSnapshot(this.CollectionName, this.PlayerId);
        //     if (!snapshotData.Exists)
        //         return null;
        //     T snapshot = snapshotData.ConvertTo<T>();
        //     return snapshot;
        // }
        //
        // public async UniTask<string> LoadAsString()
        // {
        //     DocumentSnapshot snapshotData = await FirebaseManager.Instance.GetDocumentSnapshot(this.CollectionName, this.PlayerId);
        //     if (!snapshotData.Exists)
        //         return null;
        //     T snapshot = snapshotData.ConvertTo<T>();
        //     return JsonConvert.SerializeObject(snapshot);
        // }
    }
}