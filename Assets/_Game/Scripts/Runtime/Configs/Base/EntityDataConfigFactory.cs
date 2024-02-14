using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Definition;
using UnityEngine.AddressableAssets;

namespace Runtime.Config
{
    public class EntityDataConfigFactory
    {
        #region Members

        private readonly Dictionary<EntityType, EntityDataConfigItem[]> _entityConfigItemsDictionary = new();

        #endregion Members

        
        public async UniTask<EntityDataConfigItem[]> GetEntityConfigItems(EntityType entityType, CancellationToken cancellationToken)
        {
            var result = _entityConfigItemsDictionary.TryGetValue(entityType, out EntityDataConfigItem[] entityDataConfigItems);
            if (!result)
            {
                string entityDataConfigAssetName = string.Format(AddressableKey.ENTITY_DATA_CONFIG_ASSET_FORMAT, entityType);
                var entityDataConfig = await Addressables.LoadAssetAsync<EntityDataConfig>(entityDataConfigAssetName).WithCancellation(cancellationToken);
                if (entityDataConfig == null)
                    return null;

                var items = entityDataConfig.items;
                _entityConfigItemsDictionary.TryAdd(entityType, items);
                return items;
            }
            else
            {
                return entityDataConfigItems;
            }
        }
    }
}