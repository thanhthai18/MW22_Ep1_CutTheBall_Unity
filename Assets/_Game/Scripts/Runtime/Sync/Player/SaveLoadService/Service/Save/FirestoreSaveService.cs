using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Runtime.Tool.Hash;
using Runtime.Common.Singleton;
using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Sync
{
    public sealed class FirestoreSaveService<T> where T : PlayerBase
    {
        public string CollectionName { get; set; }
        public string PlayerId { get; set; }
        public bool IsNeedSave { get; set; }
        public string Hash { get; set; }

        public async UniTask<bool> Save(T data)
        {
            if (!IsNeedSave)
            {
                return true;
            }

            string stringData = JsonConvert.SerializeObject(data);
            string hash = Singleton.Of<HashService>().Hash(stringData);
            if (hash == Hash)
            {
                IsNeedSave = false;
                return true;
            }

            Hash = hash;
            IsNeedSave = false;
            return await SaveInternal(data);
        }

        public async UniTask<bool> SaveInternal(T data)
        {
#if UNITY_EDITOR
            Debug.Log($"[SAVE TO FIRESTORE] {typeof(T).Name} ID = {this.PlayerId}");
#endif
            // if (!string.IsNullOrEmpty(this.PlayerId))
            // {
            //     return await FirebaseManager.Instance.SaveDocument(this.CollectionName, this.PlayerId, data);
            // }
            // else
            {
#if UNITY_EDITOR
                Debug.Log("Not have player ID");
#endif
                return false;
            }
        }
    }
}