using System.IO;
using Newtonsoft.Json;
using Runtime.Hash;
using Runtime.Manager.Data;
using Runtime.Common.Singleton;
using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Sync
{
    public sealed class DeviceSaveService<T> where T : BasePlayerComponent
    {
        public string CollectionName { get; set; }
        public string PlayerId { get; set; }
        public bool IsNeedSave { get; set; }
        private string Hash { get; set; }

        public void Save(T data)
        {
            if (!IsNeedSave)
                return;
            
            string stringData = JsonConvert.SerializeObject(data);
            string hash = Singleton.Of<HashService>().Hash(stringData);
            if (hash == Hash)
            {
                IsNeedSave = false;
                return;
            }

            Hash = hash;
            SaveInternal(stringData);
            IsNeedSave = false;
            data.PublishChangeData();
        }
        
        private void SaveInternal(string data)
        {
            // var stringData = JsonConvert.SerializeObject(data);
#if UNITY_EDITOR
            Debug.Log("Data Save of " + typeof(T).Name + ": " + data);
#endif
            SaveToDevice(data);
        }

        public void SaveToDevice(string data)
        {
            var path = Application.persistentDataPath + "/Save/" + typeof(T).Name + ".gw";
            string dataText = RijndaelCryptoAlgorithm.Encrypt(data);
            File.WriteAllText(path, dataText);
        }
    }
}