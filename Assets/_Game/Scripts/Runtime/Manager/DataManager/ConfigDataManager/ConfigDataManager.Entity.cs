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
        
        
        public UniTask<EntityDataConfig> LoadEntityConfig(CancellationToken cancellationToken)
        {
            var assetName = $"{typeof(EntityDataConfig)}";
            Debug.Log(assetName);
            return Load<EntityDataConfig>(assetName, cancellationToken);
        }

        public EntityDataConfig GetEntityConfig() => GetData<EntityDataConfig>();

        public EntityDataConfigItem[] GetBallDataConfigItem()
        {
            var dataConfig = GetEntityConfig();
            if (dataConfig != null)
            {
                return dataConfig.ballDataConfig;
            }

            return null;
        }
        
        public EntityDataConfigItem[] GetBoomDataConfigItem()
        {
            var dataConfig = GetEntityConfig();
            if (dataConfig != null)
            {
                return dataConfig.boomDataConfig;
            }

            return null;
        }




        #endregion Class Methods
    }
}