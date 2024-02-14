using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Common.Singleton;
using Runtime.Config;
using Runtime.Definition;
using Runtime.Gameplay.EntitySystem;
using Runtime.Manager.Data;
using Runtime.Message;
using Runtime.SceneLoading;
using UnityEngine;

namespace Runtime.Gameplay.Manager
{
    public abstract class GameplayDataManager : MonoSingleton<GameplayDataManager>
    {
        #region Members

        private Dictionary<string, EntityDataConfigItem> _ballConfigItemsDictionary = new();
        private Dictionary<string, EntityDataConfigItem> _boomConfigItemsDictionary = new();
        
        #endregion Members
        
        public bool IsDataLoaded { get; protected set; }

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            SceneManager.RegisterCompletedTaskBeforeNewSceneAppeared(LoadConfig);
        }

        #endregion API Methods

        #region Class Methods
        
        public async UniTask<EntityModelData> GetBallModelDataAsync(string entityId, CancellationToken cancellationToken)
        {
            if (!_ballConfigItemsDictionary.ContainsKey(entityId))
            {
                var ballDataConfigItems = DataManager.Config.GetBallDataConfigItem();
                _ballConfigItemsDictionary.TryAdd(entityId, ballDataConfigItems.First());
            }

            var ballModelData = new EntityModelData(_ballConfigItemsDictionary[entityId]);
            return ballModelData;
        }
        
        public async UniTask<EntityModelData> GetBoomModelDataAsync(string entityId, CancellationToken cancellationToken)
        {
            if (!_boomConfigItemsDictionary.ContainsKey(entityId))
            {
                var boomDataConfigItems = DataManager.Config.GetBoomDataConfigItem();
                _boomConfigItemsDictionary.TryAdd(entityId, boomDataConfigItems.First());
            }

            var boomModelData = new EntityModelData(_boomConfigItemsDictionary[entityId]);
            return boomModelData;
        }

        // public async UniTask<HeroStats> GetHeroStatsAsync(string heroId, int heroLevel, CancellationToken cancellationToken)
        // {
        //     var heroFragmentConfig = DataManager.Config.GetHeroFragmentInfo();
        //     var heroGroupType = heroFragmentConfig.items.FirstOrDefault(x => x.fragmentId == heroId).groupType;
        //     var heroConfigItem = await GetHeroConfigItem(heroId, cancellationToken);
        //     var heroStats = new HeroStats(heroConfigItem.heroLevelStats);
        //     var skillTreeConfig = DataManager.Config.GetSkillTreeInfo();
        //
        //     // Update hero stats from the hero's current level that's been reached.
        //     var upgradeLevelItem = heroConfigItem.upgradeLevelItems;
        //     for (int i = 0; i < upgradeLevelItem.Length; i++)
        //     {
        //         var statType = upgradeLevelItem[i].levelStatType;
        //         var statValue = upgradeLevelItem[i].levelStatValue * (heroLevel - 1);
        //         heroStats.AddBaseValue(statType, statValue);
        //     }
        //
        //     // Update hero stats from the skill tree.
        //     var firstBranchSkillTreeDataItems = skillTreeConfig.firstBranchItems;
        //     var secondBranchSkillTreeDataItems = skillTreeConfig.secondBranchItems;
        //     var firstBranchHighestUnlocked = DataManager.Local.FirstBranchSkillTreeHighestUnlocked;
        //     var secondBranchHighestUnlocked = DataManager.Local.SecondBranchSkillTreeHighestUnlocked;
        //     for (var i = 0; i <= firstBranchHighestUnlocked; i++)
        //     {
        //         var buffStat = firstBranchSkillTreeDataItems[i].buffStat;
        //         heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //     }
        //     for (var i = 0; i <= secondBranchHighestUnlocked; i++)
        //     {
        //         var buffStat = secondBranchSkillTreeDataItems[i].buffStat;
        //         heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //     }
        //
        //     // Add bonus stat self when increase level.
        //     var upgradeLevelBonusItem = heroConfigItem.upgradeLevelBonusItems;
        //     foreach (var bonusItem in upgradeLevelBonusItem)
        //     {
        //         if (bonusItem.upgradedLevel <= heroLevel)
        //         {
        //             if (bonusItem.buffTargetStatItem.buffTargetType == BuffTargetType.Self)
        //             {
        //                 var buffStat = bonusItem.buffTargetStatItem.buffStatItem;
        //                 heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //             }
        //             if (bonusItem.skillIdentity.skillType != SkillType.None)
        //             {
        //                 var skillDataConfigItem = await _skillConfigFactory.GetSkillConfigItem(bonusItem.skillIdentity.skillType,
        //                                                                                        bonusItem.skillIdentity.skillDataId,
        //                                                                                        cancellationToken);
        //                 if (skillDataConfigItem.buffTargetStatItem.buffTargetType == BuffTargetType.Self)
        //                 {
        //                     var buffStat = skillDataConfigItem.buffTargetStatItem.buffStatItem;
        //                     heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //                 }
        //             }
        //         }
        //     }
        //
        //     // Add stat when uprade hero other.
        //     foreach (var buffStatItem in _buffTargetStatItems)
        //     {
        //         if (buffStatItem.buffTargetType == BuffTargetType.All ||
        //             (buffStatItem.buffTargetType == BuffTargetType.HeroGroup && buffStatItem.heroGroupType == heroGroupType))
        //         {
        //             var buffStat = buffStatItem.buffStatItem;
        //             heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //         }
        //     }
        //
        //     // Set variable: powerIgnoreEquipment.
        //     float heroAtkStatIgnoreEquipment = heroStats.GetStatTotalValue(StatType.AttackDamage);
        //     float heroHpStatIgnoreEquipment = heroStats.GetStatTotalValue(StatType.HealthPoint);
        //     var heroPowerIgnoreEquipment = Mathf.RoundToInt(heroAtkStatIgnoreEquipment * 4 + heroHpStatIgnoreEquipment);
        //     heroStats.SetPowerIgnoreEquipmentValue(heroPowerIgnoreEquipment);
        //
        //     // Add stat of equipment.
        //     for (var i = 0; i < _equipmentStatBuffs.Count; i++)
        //     {
        //         var buffStat = _equipmentStatBuffs[i];
        //         heroStats.AddBonusValue(buffStat.statType, buffStat.value, buffStat.statModifyType);
        //     }
        //
        //     return heroStats;
        // }
        //
        // public async UniTask<HeroStats> GetPVPHeroStatsAsync(string heroId, int heroLevel, PVPHeroBuffData pvpHeroBuffData,
        //                                                      CancellationToken cancellationToken)
        // {
        //     var heroFragmentConfig = DataManager.Config.GetHeroFragmentInfo();
        //     var heroGroupType = heroFragmentConfig.items.FirstOrDefault(x => x.fragmentId == heroId).groupType;
        //     var heroConfigItem = await GetHeroConfigItem(heroId, cancellationToken);
        //     var heroStats = new HeroStats(heroConfigItem.heroLevelStats);
        //     var skillTreeConfig = DataManager.Config.GetSkillTreeInfo();
        //
        //     // Update hero stats from the hero's current level that's been reached.
        //     var upgradeLevelItem = heroConfigItem.upgradeLevelItems;
        //     for (int i = 0; i < upgradeLevelItem.Length; i++)
        //     {
        //         var statType = upgradeLevelItem[i].levelStatType;
        //         var statValue = upgradeLevelItem[i].levelStatValue * (heroLevel - 1);
        //         heroStats.AddBaseValue(statType, statValue);
        //     }
        //
        //     // Update hero stats from the skill tree.
        //     var firstBranchSkillTreeDataItems = skillTreeConfig.firstBranchItems;
        //     var secondBranchSkillTreeDataItems = skillTreeConfig.secondBranchItems;
        //     for (var i = 0; i <= pvpHeroBuffData.firstBranchSkillTreeHighestUnlocked; i++)
        //     {
        //         var buffStat = firstBranchSkillTreeDataItems[i].buffStat;
        //         heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //     }
        //     for (var i = 0; i <= pvpHeroBuffData.secondBranchSkillTreeHighestUnlocked; i++)
        //     {
        //         var buffStat = secondBranchSkillTreeDataItems[i].buffStat;
        //         heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //     }
        //
        //     // Add bonus stat self when increase level.
        //     var upgradeLevelBonusItem = heroConfigItem.upgradeLevelBonusItems;
        //     foreach (var bonusItem in upgradeLevelBonusItem)
        //     {
        //         if (bonusItem.upgradedLevel <= heroLevel)
        //         {
        //             if (bonusItem.buffTargetStatItem.buffTargetType == BuffTargetType.Self)
        //             {
        //                 var buffStat = bonusItem.buffTargetStatItem.buffStatItem;
        //                 heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //             }
        //             if (bonusItem.skillIdentity.skillType != SkillType.None)
        //             {
        //                 var skillDataConfigItem = await _skillConfigFactory.GetSkillConfigItem(bonusItem.skillIdentity.skillType,
        //                                                                                        bonusItem.skillIdentity.skillDataId,
        //                                                                                        cancellationToken);
        //                 if (skillDataConfigItem.buffTargetStatItem.buffTargetType == BuffTargetType.Self)
        //                 {
        //                     var buffStat = skillDataConfigItem.buffTargetStatItem.buffStatItem;
        //                     heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //                 }
        //             }
        //         }
        //     }
        //
        //     // Set variable: powerIgnoreEquipment.
        //     float heroAtkStatIgnoreEquipment = heroStats.GetStatTotalValue(StatType.AttackDamage);
        //     float heroHpStatIgnoreEquipment = heroStats.GetStatTotalValue(StatType.HealthPoint);
        //     var heroPowerIgnoreEquipment = Mathf.RoundToInt(heroAtkStatIgnoreEquipment * 4 + heroHpStatIgnoreEquipment);
        //     heroStats.SetPowerIgnoreEquipmentValue(heroPowerIgnoreEquipment);
        //
        //     // Add stats from slot buff.
        //     if (pvpHeroBuffData.hasSlotBuffStat)
        //     {
        //         var buffStat = pvpHeroBuffData.slotBuffStatItem;
        //         heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //     }
        //
        //     // Add stats from pvp building.
        //     if (pvpHeroBuffData.pvpBuildingBuffItems != null)
        //     {
        //         for (var i = 0; i < pvpHeroBuffData.pvpBuildingBuffItems.Count; i++)
        //         {
        //             var buffStat = pvpHeroBuffData.pvpBuildingBuffItems[i];
        //             heroStats.AddBonusValue(buffStat.statType, buffStat.statValue, buffStat.statModifyType);
        //         }
        //     }
        //
        //     // Add stat of equipment.
        //     if (pvpHeroBuffData.equipmentStatBuffs != null)
        //     {
        //         for (var i = 0; i < pvpHeroBuffData.equipmentStatBuffs.Count; i++)
        //         {
        //             var buffStat = pvpHeroBuffData.equipmentStatBuffs[i];
        //             heroStats.AddBonusValue(buffStat.statType, buffStat.value, buffStat.statModifyType);
        //         }
        //     }
        //
        //     return heroStats;
        // }
        //
        // public async UniTask<HeroStats> GetHeroBossStatsAsync(string heroId, int heroLevel, float hpScaleFactor, CancellationToken cancellationToken)
        // {
        //     var heroFragmentConfig = DataManager.Config.GetHeroFragmentInfo();
        //     var heroGroupType = heroFragmentConfig.items.FirstOrDefault(x => x.fragmentId == heroId).groupType;
        //     var heroConfigItem = await GetHeroConfigItem(heroId, cancellationToken);
        //     var heroStats = new HeroStats(heroConfigItem.heroLevelStats);
        //
        //     // Update hero stats from the hero's current level that's been reached.
        //     var upgradeLevelItem = heroConfigItem.upgradeLevelItems;
        //     for (int i = 0; i < upgradeLevelItem.Length; i++)
        //     {
        //         var statType = upgradeLevelItem[i].levelStatType;
        //         var statValue = upgradeLevelItem[i].levelStatValue * (heroLevel - 1);
        //         heroStats.AddBaseValue(statType, statValue);
        //     }
        //
        //     // Update the hp stat with scale factor.
        //     heroStats.SetScaleBaseValue(StatType.HealthPoint, hpScaleFactor);
        //
        //     return heroStats;
        // }
        //
        // public async UniTask<List<SkillIdentity>> GetHeroSkillIdentitiesAsync(string heroId, int heroLevel, CancellationToken cancellationToken)
        // {
        //     var heroConfigItem = await GetHeroConfigItem(heroId, cancellationToken);
        //     var skillSecond = SkillType.None;
        //     var skillIdentities = new List<SkillIdentity>();
        //     var upgradeLevelBonusItems = heroConfigItem.upgradeLevelBonusItems;
        //     for (int i = upgradeLevelBonusItems.Length - 1; i >= 0; i--)
        //     {
        //         if (upgradeLevelBonusItems[i].skillIdentity.skillType != SkillType.None && upgradeLevelBonusItems[i].upgradedLevel <= heroLevel)
        //         {
        //             if (skillSecond == SkillType.None)
        //             {
        //                 skillSecond = upgradeLevelBonusItems[i].skillIdentity.skillType;
        //                 var skillIdentity = new SkillIdentity(upgradeLevelBonusItems[i].skillIdentity.skillType, upgradeLevelBonusItems[i].skillIdentity.skillDataId);
        //                 skillIdentities.Add(skillIdentity);
        //             }
        //             else
        //             {
        //                 if (skillSecond != upgradeLevelBonusItems[i].skillIdentity.skillType)
        //                 {
        //                     var skillIdentity = new SkillIdentity(upgradeLevelBonusItems[i].skillIdentity.skillType, upgradeLevelBonusItems[i].skillIdentity.skillDataId);
        //                     skillIdentities.Add(skillIdentity);
        //                     break;
        //                 }
        //             }
        //         }
        //     }
        //     return skillIdentities;
        // }
        //
        // public async UniTask<List<SkillModel>> GetHeroSkillModelsAsync(List<SkillIdentity> skillIdentities, CancellationToken cancellationToken)
        // {
        //     var skillModels = new List<SkillModel>();
        //     foreach (var skillIdentity in skillIdentities)
        //     {
        //         var skillDataConfigItem = await _skillConfigFactory.GetSkillConfigItem(skillIdentity.skillType,
        //                                                                                skillIdentity.skillDataId,
        //                                                                                cancellationToken);
        //         if (skillDataConfigItem.buffTargetStatItem.buffTargetType == BuffTargetType.None)
        //         {
        //             var skillData = new SkillData(skillDataConfigItem);
        //             var skillModel = SkillModelFactory.GetSkillModel(skillIdentity.skillType, skillData);
        //             skillModels.Add(skillModel);
        //         }
        //     }
        //     return skillModels;
        // }
        //
        // public async UniTask<long> GetTotalHeroesTeamCombatPowerAsync(CancellationToken cancellationToken)
        // {
        //     long totalHeroStatsPower = 0;
        //     foreach (var item in DataManager.Local.HeroFragments)
        //     {
        //         if (item.isSelected)
        //         {
        //             var heroStats = await GetHeroStatsAsync(item.id, item.level, cancellationToken);
        //             var heroStatsPower = Constant.GetHeroPowerValueAsync(heroStats);
        //             totalHeroStatsPower += heroStatsPower;
        //         }
        //     }
        //     return totalHeroStatsPower;
        // }
        //
        // public async UniTask<long> GetTotalHeroesTeamCombatPowerIgnoreEquipment(CancellationToken cancellationToken)
        // {
        //     long totalHeroPowerIgnoreEquipment = 0;
        //     foreach (var item in DataManager.Local.HeroFragments)
        //     {
        //         if (item.isSelected)
        //         {
        //             var heroStats = await GetHeroStatsAsync(item.id, item.level, cancellationToken);
        //             totalHeroPowerIgnoreEquipment += heroStats.GetPowerIgnoreEquipmentValue();
        //         }
        //     }
        //     return totalHeroPowerIgnoreEquipment;
        // }
        //
       
        //
        // public async UniTask<HeroModelData> GetPVPHeroModelDataAsync(string heroId, int heroLevel,
        //                                                              PVPHeroBuffData pvpHeroBuffData, CancellationToken cancellationToken)
        // {
        //     var heroStats = await GetPVPHeroStatsAsync(heroId, heroLevel, pvpHeroBuffData, cancellationToken);
        //     var heroLevelModel = new HeroLevelModel(heroLevel, heroStats);
        //     var skillIdentities = await GetHeroSkillIdentitiesAsync(heroId, heroLevel, cancellationToken);
        //     var skillModels = await GetHeroSkillModelsAsync(skillIdentities, cancellationToken);
        //     var heroConfigItem = await GetHeroConfigItem(heroId, cancellationToken);
        //     var heroModelData = new HeroModelData(heroId,
        //                                           heroId,
        //                                           heroConfigItem.detectedPriority,
        //                                           heroConfigItem.attackType,
        //                                           Constant.HERO_DIED_RESPAWN_DELAY,
        //                                           skillModels,
        //                                           heroLevelModel);
        //     return heroModelData;
        // }
        //
        // public async UniTask<EnemyModelData> GetEnemyModelDataAsync(string enemyId, int enemyLevel, bool isElite, float activatedSqrRange,
        //                                                             float detectedSqrRange, CancellationToken cancellationToken)
        // {
        //     if (!_enemyConfigItemsDictionary.ContainsKey(enemyId))
        //     {
        //         var enemyConfig = await DataManager.Config.LoadEnemyConfig(enemyId, cancellationToken);
        //         _enemyConfigItemsDictionary.TryAdd(enemyId, enemyConfig.FirstOnlyConfig);
        //     }
        //
        //     EliteMultiplier eliteMultiplier = Constant.DEFAULT_STAT_MULTIPLIER;
        //     float respawnTime = Constant.ENEMY_DIED_RESPAWN_DELAY;
        //     if (isElite)
        //     {
        //         var eliteConfig = await DataManager.Config.LoadEliteConfig(cancellationToken);
        //         eliteMultiplier = eliteConfig.FirstOnlyConfig;
        //         respawnTime = eliteMultiplier.multiplierRespawnTime * respawnTime;
        //     }
        //     var enemyConfigItem = _enemyConfigItemsDictionary[enemyId];
        //     enemyLevel = enemyLevel <= enemyConfigItem.enemyLevelConfigItems.Length
        //                ? enemyLevel
        //                : enemyConfigItem.enemyLevelConfigItems.Length;
        //     var enemyLevelConfigItem = enemyConfigItem.enemyLevelConfigItems.FirstOrDefault(x => x.level == enemyLevel);
        //     var enemyStats = new EnemyStats(enemyLevelConfigItem.enemyLevelStats, eliteMultiplier);
        //     var enemyLevelModel = new EnemyLevelModel(enemyLevel, enemyStats);
        //
        //     return new EnemyModelData(enemyId,
        //                               enemyConfigItem.visualId,
        //                               enemyConfigItem.detectedPriority,
        //                               activatedSqrRange,
        //                               detectedSqrRange,
        //                               enemyConfigItem.attackType,
        //                               enemyConfigItem.diedResourcesData,
        //                               enemyLevelConfigItem.enemyLevelStats.expResourcesData.Multiply(eliteMultiplier.multiplierExp),
        //                               respawnTime,
        //                               enemyLevelModel);
        // }
        //
        // public async UniTask<BossModelData> GetBossModelDataAsync(string bossId, int bossLevel, float activatedSqrRange,
        //                                                           float detectedSqrRange, CancellationToken cancellationToken)
        // {
        //     if (!_bossConfigItemsDictionary.ContainsKey(bossId))
        //     {
        //         var newBossConfig = await DataManager.Config.LoadBossConfig(bossId, cancellationToken);
        //         _bossConfigItemsDictionary.TryAdd(bossId, newBossConfig.FirstOnlyConfig);
        //     }
        //     var bossConfigItem = _bossConfigItemsDictionary[bossId];
        //     bossLevel = bossLevel <= bossConfigItem.bossLevelConfigItems.Length
        //               ? bossLevel
        //               : bossConfigItem.bossLevelConfigItems.Length;
        //     var bossLevelConfigItem = bossConfigItem.bossLevelConfigItems.FirstOrDefault(x => x.level == bossLevel);
        //     var bossStats = new BossStats(bossLevelConfigItem.bossLevelStats);
        //     var bossLevelModel = new BossLevelModel(bossLevel, bossStats);
        //     var skillModels = new List<SkillModel>();
        //     foreach (var skillIdentity in bossLevelConfigItem.SkillIdentities)
        //     {
        //         var skillDataConfigItem = await _skillConfigFactory.GetSkillConfigItem(skillIdentity.skillType,
        //                                                                                skillIdentity.skillDataId,
        //                                                                                cancellationToken);
        //         var skillData = new SkillData(skillDataConfigItem);
        //         var skillModel = SkillModelFactory.GetSkillModel(skillIdentity.skillType, skillData);
        //         skillModels.Add(skillModel);
        //     }
        //     return new BossModelData(bossId,
        //                              bossConfigItem.visualId,
        //                              bossConfigItem.detectedPriority,
        //                              activatedSqrRange,
        //                              detectedSqrRange,
        //                              bossConfigItem.attackType,
        //                              bossConfigItem.diedResourcesData,
        //                              bossLevelConfigItem.bossLevelStats.expResourcesData,
        //                              Constant.BOSS_DIED_RESPAWN_DELAY,
        //                              skillModels.ToArray(),
        //                              bossLevelModel);
        // }
        //
        // public async UniTask<ObjectModelData> GetObjectModelDataAsync(string objectId, CancellationToken cancellationToken)
        // {
        //     if (!_objectConfigItemsDictionary.ContainsKey(objectId))
        //     {
        //         var objectConfig = await DataManager.Config.LoadObjectConfig(cancellationToken);
        //         var newObjectConfigItem = objectConfig.items.FirstOrDefault(x => x.id == objectId);
        //         _objectConfigItemsDictionary.TryAdd(objectId, newObjectConfigItem);
        //     }
        //     var objectConfigItem = _objectConfigItemsDictionary[objectId];
        //     return new ObjectModelData(objectConfigItem.detectedPriority,
        //                                objectConfigItem.hp,
        //                                objectConfigItem.destroyedRespawnDelay,
        //                                objectConfigItem.destroyedResourceData);
        // }
        //
        // public async UniTask<HeroConfigItem> GetHeroConfigItem(string heroId, CancellationToken cancellationToken)
        //     => await _heroConfigFactory.GetHeroConfigItem(heroId, cancellationToken);
        //
        // public async UniTask<SkillDataConfigItem> GetSkillConfigItem(SkillType skillType, int skillDataId, CancellationToken cancellationToken)
        //     => await _skillConfigFactory.GetSkillConfigItem(skillType, skillDataId, cancellationToken);
        //
        // public async UniTask<SkillDataConfigItem[]> GetSkillConfigItems(SkillType skillType, CancellationToken cancellationToken)
        //    => await _skillConfigFactory.GetSkillConfigItems(skillType, cancellationToken);
        //
        // public void AddOtherBuffTargetStatItem(BuffTargetStatItem buffTargetStatItem)
        // {
        //     if (buffTargetStatItem.buffTargetType == BuffTargetType.All ||
        //         buffTargetStatItem.buffTargetType == BuffTargetType.HeroGroup)
        //     {
        //         _buffTargetStatItems.Add(buffTargetStatItem);
        //     }
        // }
        //
        // public void SetCacheEquipmentStatBuff()
        // {
        //     _equipmentStatBuffs = DataManager.Local.GetTotalEquipmentStatBuff();
        // }
        //
        // public void AddBuffStatOnReceiveHero(string heroId, int heroLevel)
        //     => AddBuffTargetStatItemsAsync(heroId, heroLevel, this.GetCancellationTokenOnDestroy()).Forget();
        //
        protected virtual async UniTask LoadConfig(CancellationToken cancellationToken)
        {
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.DataLoaded));
            IsDataLoaded = true;
        }
        //
        // private async UniTask LoadBuffStatItemsAsync(CancellationToken cancellationToken)
        // {
        //     _buffTargetStatItems = new List<BuffTargetStatItem>();
        //     var heroFragments = DataManager.Local.HeroFragments;
        //     foreach (var heroFragment in heroFragments)
        //         await AddBuffTargetStatItemsAsync(heroFragment.id, heroFragment.level, cancellationToken);
        // }
        //
        // private async UniTask AddBuffTargetStatItemsAsync(string heroId, int heroLevel, CancellationToken cancellationToken)
        // {
        //     var heroConfigItem = await GetHeroConfigItem(heroId, cancellationToken);
        //     foreach (var bonusItem in heroConfigItem.upgradeLevelBonusItems)
        //     {
        //         if (bonusItem.upgradedLevel <= heroLevel)
        //         {
        //             if (bonusItem.buffTargetStatItem.buffTargetType == BuffTargetType.All ||
        //                 bonusItem.buffTargetStatItem.buffTargetType == BuffTargetType.HeroGroup)
        //             {
        //                 AddOtherBuffTargetStatItem(bonusItem.buffTargetStatItem);
        //             }
        //
        //             if (bonusItem.skillIdentity.skillType != SkillType.None)
        //             {
        //                 var skillDataConfigItem = await _skillConfigFactory.GetSkillConfigItem(bonusItem.skillIdentity.skillType,
        //                                                                                            bonusItem.skillIdentity.skillDataId,
        //                                                                                            cancellationToken);
        //                 if ((skillDataConfigItem.buffTargetStatItem.buffTargetType == BuffTargetType.All ||
        //                     skillDataConfigItem.buffTargetStatItem.buffTargetType == BuffTargetType.HeroGroup))
        //                 {
        //                     AddOtherBuffTargetStatItem(skillDataConfigItem.buffTargetStatItem);
        //                 }
        //             }
        //         }
        //     }
        // }

        #endregion Class Methods
    }
}