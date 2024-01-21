using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Runtime.Extensions;
using Runtime.Common.Singleton;
using Cysharp.Threading.Tasks;

namespace Runtime.Manager.Data
{
    public partial class ConfigDataManager : MonoSingleton<ConfigDataManager>
    {
        #region Members

        private Dictionary<string, ScriptableObject> _cachedData;

        #endregion Members

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            _cachedData = new();
        }

        private void OnDestroy()
            => ReleaseAll();

        #endregion API Methods

        #region Class Methods

        private T GetData<T>() where T : ScriptableObject
        {
            if (_cachedData.TryGetValue(typeof(T).ToString(), out ScriptableObject data))
                return data as T;

            return null;
        }

        private T GetData<T>(string assetName) where T : ScriptableObject
        {
            if (_cachedData.TryGetValue(assetName, out ScriptableObject data))
                return data as T;

            return null;
        }

        public async UniTask<T> Load<T>(CancellationToken cancellationToken = default) where T : ScriptableObject
        {
            if (_cachedData.TryGetValue(typeof(T).ToString(), out ScriptableObject data))
                return data as T;

            return await Preload<T>(cancellationToken);
        }

        private async UniTask<T> Preload<T>(CancellationToken cancellationToken) where T : ScriptableObject
        {
            var resourcePath = typeof(T).ToString();
            T data = await Addressables.LoadAssetAsync<T>(resourcePath).WithCancellation(cancellationToken);
            _cachedData.Replace(resourcePath, data);
            return data;
        }

        public async UniTask<T> Load<T>(string assetName, CancellationToken cancellationToken = default, string defaultAssetName = null) where T : ScriptableObject
        {
            if (_cachedData.TryGetValue(assetName, out ScriptableObject data))
                return data as T;

            return await Preload<T>(assetName, cancellationToken, defaultAssetName);
        }

        public async UniTask<T> Preload<T>(string assetName, CancellationToken cancellationToken, string defaultAssetName) where T : ScriptableObject
        {
            T data;
            try
            {
                data = await Addressables.LoadAssetAsync<T>(assetName).WithCancellation(cancellationToken);
            }
            catch
            {
                data = null;
            }

            if (data == null)
                data = await Addressables.LoadAssetAsync<T>(defaultAssetName).WithCancellation(cancellationToken);

            _cachedData.Replace(assetName, data);
            return data;
        }

        private void ReleaseAll()
        {
            foreach (var data in _cachedData.Values)
                Addressables.Release(data);
            _cachedData.Clear();
        }

        #endregion Class Methods
    }
}