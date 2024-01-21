using System.Linq;
using System.Threading;
using Runtime.Config;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Manager.Data
{
    public partial class ConfigDataManager
    {
        #region Class Methods


        public UniTask<HitObjectConfig> LoadHitObjectConfig(CancellationToken cancellationToken)
        {
            var assetName = $"{typeof(HitObjectConfig)}";
            Debug.Log(assetName);
            return Load<HitObjectConfig>(assetName, cancellationToken);
        }

        public HitObjectConfig GetHitObjectInfo() => GetData<HitObjectConfig>();



        #endregion Class Methods
    }
}