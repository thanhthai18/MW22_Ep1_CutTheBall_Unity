using System;
using System.IO;
using Runtime.Common.Singleton;
using Runtime.Definition;
using Runtime.Tool.JsonConverter;
using Runtime.Manager.Data;
using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Sync
{
    public sealed class DeviceLoadService<T> where T : PlayerBase
    {
        public string CollectionName { get; set; }
        public string PlayerId { get; set; }
        
        public T Load()
        {
            var data = LoadFromDevice();
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            try
            {
                T snapshot = Singleton.Of<JsonService>().Deserialize<T>(data);
                return snapshot;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }
        
        private string LoadFromDevice()
        {
            var path = Application.persistentDataPath + "/Save/" + typeof(T).Name + ".gw";
            
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                try
                {
                    text = RijndaelCryptoAlgorithm.Decrypt(text);
                }
                catch (Exception e)
                {
                    File.Delete(path);
                    File.Create(path);
#if UNITY_EDITOR
                    Debug.LogError("Exception: " + e.Message);
#endif
                }

#if UNITY_EDITOR
                Debug.Log("Data Load of " + typeof(T).Name + ": "  + text);
#endif
                
                return text;
            }
            else
            {
                var directoryFolder = Application.persistentDataPath + Constant.DATA_SAVED_FOLDER;
                 if (!Directory.Exists(directoryFolder))
                     Directory.CreateDirectory(directoryFolder);
                return string.Empty;
            }
        }
    }
}