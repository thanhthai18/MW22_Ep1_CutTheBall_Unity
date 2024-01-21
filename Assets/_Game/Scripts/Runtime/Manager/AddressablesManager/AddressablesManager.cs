using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace Runtime.Addressable
{
    public static class AddressablesManager
    {
        #region Class Methods

        public static async UniTask UpdateCatalogsAsync(float progressStart, float progressStep, IProgress<float> progress)
        {
            var catalogs = await Addressables.CheckForCatalogUpdates(true);
            progressStart += progressStep / 2;
            progress.Report(progressStart);

            if (catalogs.Count > 0)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Update {catalogs.Count} catalog(s)...");
                foreach (var catalog in catalogs)
                    Debug.LogWarning($"Update catalog {catalog}");
#endif
                try
                {
                    var resourcesLocators = await Addressables.UpdateCatalogs(true, catalogs, true);
                    foreach (var locator in resourcesLocators)
                    {
                        var keyCount = locator.Keys.Count();
                        foreach (var key in locator.Keys)
                        {
                            var downloadSize = await Addressables.GetDownloadSizeAsync(key);
                            if (downloadSize > 0)
                                await Addressables.DownloadDependenciesAsync(key, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            progress.Report(progressStart + progressStep);
        }

        public static void OnAtlasRequested(string key, Action<SpriteAtlas> onSucceeded)
        {
            var asyncOperation = Addressables.LoadAssetAsync<SpriteAtlas>(key);
            asyncOperation.Completed += handle => {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    onSucceeded(handle.Result);
                else
                    Debug.LogError($"Cannot find {nameof(SpriteAtlas)} with key '{key}'.");
            };
        }

        private static int Count<T>(this IEnumerable<T> collection)
        {
            var count = 0;
            foreach (var x in collection)
                count += 1;
            return count;
        }

        #endregion Class Methods
    }
}