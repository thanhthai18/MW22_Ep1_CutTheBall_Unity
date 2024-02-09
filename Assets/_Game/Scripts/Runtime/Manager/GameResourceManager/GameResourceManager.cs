using System.Linq;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Common.Singleton;
using Runtime.Definition;
using Runtime.Extensions;
using Runtime.Gameplay.Visual;
using Runtime.Manager.Data;
using Runtime.Manager.Toast;
using Runtime.Message;
using Cysharp.Threading.Tasks;

namespace Runtime.Gameplay.Manager
{
    public abstract class GameResourceManager : MonoSingleton<GameResourceManager>
    {
        #region Class Methods

        // public virtual void HandleEnemyDied(EnemyDiedMessage enemyDiedMessage, CancellationToken cancellationToken) { }
        // public virtual void HandleObjectDestroyed(ObjectDestroyedMessage objectDestroyedMessage, CancellationToken cancellationToken) { }
        //
        // public virtual ResourceData CalculatorExpResource(ResourceData resourceData, int zoneLevel)
        // {
        //     if (zoneLevel <= 0)
        //     {
        //         return resourceData;
        //     }
        //     int playerLevel = DataManager.Local.PlayerLevel;
        //     int deltaGreater = playerLevel - zoneLevel;
        //     int deltaLess = zoneLevel - Constant.LESS_THAN_LEVEL_TO_PENALTY_EXP - playerLevel;
        //     int penaltyLevel = Mathf.Max(deltaGreater, deltaLess, 0);
        //     if (penaltyLevel == 0)
        //     {
        //         return resourceData;
        //     }
        //     int expAfterPenalty = Mathf.Max(Mathf.RoundToInt(resourceData.resourceNumber * (1 - penaltyLevel * 0.1f)),0);
        //     return new ResourceData(resourceData.resourceType, resourceData.resourceId, expAfterPenalty);
        // }
        //
        // protected virtual void DropEquipment(float dropEquipmentRate, int zoneLevel, Vector2 position, CancellationToken cancellationToken)
        // {
        //     float randomNumber = Random.Range(0f, 1f);
        //     if (randomNumber <= dropEquipmentRate)
        //     {
        //         EquipmentData equipmentData = CreateNewEquipmentData(zoneLevel);
        //         if (equipmentData != null)
        //         {
        //             SpawnRewardEquipmentVisual(equipmentData, position, cancellationToken);
        //         }
        //     }
        // }
        //
        // protected virtual void SpawnRewardResourceVisual(ResourceData[] resourcesData, Vector2 spawnPosition, CancellationToken cancellationToken, string earnSourceId)
        // {
        //     if (resourcesData.Length > 0)
        //     {
        //         GameplayVisualController.Instance.DislayRewardVisual(resourcesData[0],
        //                                                              spawnPosition,
        //                                                              cancellationToken,
        //                                                              () => CollectResources(resourcesData, earnSourceId)).Forget();
        //     }
        // }
        //
        // protected virtual void SpawnRewardEquipmentVisual(EquipmentData equipmentData, Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     if (equipmentData != null)
        //     {
        //         GameplayVisualController.Instance.DislayRewardEquipmentVisual(equipmentData,
        //                                                                       spawnPosition,
        //                                                                       cancellationToken,
        //                                                                       () => CollectEquipment(equipmentData),
        //                                                                       equipmentData.EquipmentRarity).Forget();
        //     }
        // }
        //
        // protected virtual void SpawnExpVisual(ResourceData[] resourcesData, Vector2 spawnPosition, CancellationToken cancellationToken, string earnSourceId)
        // {
        //     if (resourcesData.Length > 0)
        //     {
        //         GameplayVisualController.Instance.DislayExpVisual(resourcesData[0],
        //                                                           spawnPosition,
        //                                                           cancellationToken,
        //                                                           () => CollectResources(resourcesData, earnSourceId)).Forget();
        //     }
        // }
        //
        // protected virtual void CollectResources(ResourceData[] resourcesData, string earnSourceId)
        // {
        //     foreach (var resourceData in resourcesData)
        //     {
        //         if (resourceData.resourceType != ResourceType.Equipment)
        //         {
        //             DataManager.Local.AddResource(resourceData.resourceType,
        //                                           ResourceEarnSourceType.Gameplay,
        //                                           resourceData.resourceId,
        //                                           resourceData.resourceNumber,
        //                                           earnSourceId: earnSourceId,
        //                                           isShowCommon: false
        //                                           );
        //         }
        //     }
        // }
        //
        // public EquipmentData CreateNewEquipmentData(int zoneLevel)
        // {
        //     //level
        //     var levelForRandom = DataManager.Config.GetLevelsEquipmentLevelDropByZoneLevel(zoneLevel);
        //     if (levelForRandom.Length == 0) return null;
        //     var level = levelForRandom[Random.Range(0, levelForRandom.Length)];
        //
        //     //equipmentType
        //     var equipmentType = (EquipmentType)Random.Range((int)EquipmentType.Weapon, (int)EquipmentType.Boots + 1);
        //
        //     //rarity
        //     var rarityConfigItemForRandom = DataManager.Config.GetEquipmentRarityByLevelDropConfigItemByLevelEquipment(level);
        //     var rarityDataForRandom = rarityConfigItemForRandom.equipmentRarityRateDrop;
        //     var rarityForRandom = rarityDataForRandom.Select(s=>s.rate).ToArray();
        //     int indexRarityOnArray = GameExtensions.ChooseRandomItem(rarityForRandom);
        //     var rarity = rarityDataForRandom[indexRarityOnArray].rarityType;
        //
        //     //stat
        //     var statType = Constant.GetStatTypeByEquipmentType(equipmentType);
        //     var equipmentStat = DataManager.Config.GetEquipmentBaseStatByConfig(level, statType, rarity);
        //
        //     //set
        //     var setEquipments = DataManager.Config.GetSetEquipmentByLevel(level);
        //     var setEquipment = setEquipments[Random.Range(0, setEquipments.Length)];
        //
        //     //SubOpt
        //     var listSubOpts = new List<SubOpt>();
        //     var numberOpt = DataManager.Config.GetOptNumber(rarity);
        //     var listOptIdRateItemClone = DataManager.Config.GetOptIdRateItem(equipmentType).ToList();
        //     for (int i = 0; i < numberOpt; i++)
        //     {
        //         var optRatesForRandom = listOptIdRateItemClone.Select(s=>s.rate).ToArray();
        //         var indexRandom = GameExtensions.ChooseRandomItem(optRatesForRandom);
        //         if (indexRandom == -1) continue;
        //         var optId = listOptIdRateItemClone[indexRandom].optId;
        //         listOptIdRateItemClone.RemoveAt(indexRandom);
        //
        //         var rarityLevelBaseOpt = DataManager.Config.GetOptRarityLevelBase();
        //         var subOpt = new SubOpt(optId, rarityLevelBaseOpt, level);
        //         listSubOpts.Add(subOpt);
        //     }
        //
        //     var equipmentData = new EquipmentData(equipmentType, setEquipment, rarity, level, equipmentStat, listSubOpts);
        //     return equipmentData;
        // }
        //
        // public EquipmentData GenerateEquipmentData(EquipmentType equipmentType, 
        //                                            SetEquipmentType setEquipment,
        //                                            RarityType rarityType)
        // {
        //     var generateLevel = DataManager.Config.GetLevelEquipmentBySetEquipment(setEquipment);
        //     var statType = Constant.GetStatTypeByEquipmentType(equipmentType);
        //     var equipmentStat = DataManager.Config.GetEquipmentBaseStatByConfig(generateLevel, statType, rarityType);
        //     var listSubOpts = new List<SubOpt>();
        //     var numberOpt = DataManager.Config.GetOptNumber(rarityType);
        //     var listOptIdRateItemClone = DataManager.Config.GetOptIdRateItem(equipmentType).ToList();
        //     for (int i = 0; i < numberOpt; i++)
        //     {
        //         var optRatesForRandom = listOptIdRateItemClone.Select(s=>s.rate).ToArray();
        //         var indexRandom = GameExtensions.ChooseRandomItem(optRatesForRandom);
        //         if (indexRandom == -1) continue;
        //         var optId = listOptIdRateItemClone[indexRandom].optId;
        //         listOptIdRateItemClone.RemoveAt(indexRandom);
        //
        //         var rarityLevelBaseOpt = DataManager.Config.GetOptRarityLevelBase();
        //         var subOpt = new SubOpt(optId, rarityLevelBaseOpt, generateLevel);
        //         listSubOpts.Add(subOpt);
        //     }
        //     var equipmentData = new EquipmentData(equipmentType, setEquipment, rarityType, generateLevel, equipmentStat, listSubOpts);
        //     return equipmentData;
        // }
        //
        // private void CollectEquipment(EquipmentData equipmentData)
        // {
        //     DataManager.Local.AddEquipmentData(equipmentData);
        //     ToastManager.Instance.Show(equipmentData);
        // }

        #endregion Class Methods
    }
}
