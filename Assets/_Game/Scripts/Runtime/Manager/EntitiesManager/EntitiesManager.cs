using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Runtime.Definition;
using Runtime.Common.Singleton;
using Runtime.Gameplay.EntitySystem;
using Runtime.Manager.Pool;
using Runtime.Message;
using Runtime.Manager.Tick;
using Runtime.Manager.Data;
using Runtime.Gameplay.Map;
using Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityRandom = UnityEngine.Random;

namespace Runtime.Gameplay.Manager
{
    public abstract class EntitiesManager : MonoSingleton<EntitiesManager>
    {
        #region Members

        public static long CurrentBattleIndex = 0;
        protected uint entityUIdCounter;
        private GameObject _arrowGameObject;

        #endregion Members

        #region Properties

        // protected List<EnemyModel> EnemyModels { get; set; }
        // public HeroModel MainHeroModel { get; protected set; }
        // public List<HeroModelTransform> HeroModelTransforms { get; protected set; }
        // public List<HeroModel> DiedHeroModels { get; protected set; }
        // public int NumberOfHeroes => HeroModelTransforms.Count;
        // public bool HasNoEnemies => EnemyModels.Count == 0;

        // public Transform MainHeroTransform
        // {
        //     get
        //     {
        //         var heroModelTransform = HeroModelTransforms.FirstOrDefault(x => x.Model == MainHeroModel);
        //         return heroModelTransform.Transform;
        //     }
        // }

        private List<HeroTombIdentiy> HeroTombIdentities { get; set; }

        #endregion Properties

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            entityUIdCounter = 0;
            // MainHeroModel = null;
            // HeroModelTransforms = new List<HeroModelTransform>();
            // HeroTombIdentities = new List<HeroTombIdentiy>();
            // EnemyModels = new List<EnemyModel>();
            // DiedHeroModels = new List<HeroModel>();
        }

        #endregion API Methods

        #region Class Methods

        // public virtual async UniTask<HeroModel> CreateHeroAsync(string heroId, int heroLevel, MovementStrategyType movementStrategyType,
        //                                                         Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     RemoveHeroTomb(heroId);
        //     RemoveFromDiedHeroesList(heroId);
        //     var heroModelData = await GameplayDataManager.Instance.GetHeroModelDataAsync(heroId, heroLevel, cancellationToken);
        //     var heroModel = new HeroModel(entityUIdCounter++, heroId, spawnPosition, heroModelData, MainHeroModel, movementStrategyType);
        //     var heroGameObject = await PoolManager.Instance.Get(heroId, cancellationToken: cancellationToken, false);
        //     heroGameObject.transform.SetParent(transform);
        //     heroGameObject.layer = Layer.HERO_LAYER;
        //     await BuildHeroEntityGameObjectAsync(heroGameObject, heroModel, spawnPosition, true, cancellationToken);
        //     var removedOldSameDiedHeroModel = DiedHeroModels.FirstOrDefault(x => x.EntityId == heroId);
        //     if (removedOldSameDiedHeroModel != null)
        //         DiedHeroModels.Remove(removedOldSameDiedHeroModel);
        //     SetUpLeaderHeroModel(heroModel);
        //     HeroModelTransforms.Add(new HeroModelTransform(heroModel, heroGameObject.transform));
        //     Messenger.Publish(new GameStateChangedMessage(GameStateEventType.HeroSpawned));
        //     return heroModel;
        // }
        //
        // public virtual async UniTask<HeroModel> CreateHeroSaveAsync(string heroId, int heroLevel, MovementStrategyType movementStrategyType, Vector2 spawnPosition,
        //                                                             Vector2 targetPosition, CancellationToken cancellationToken, Action callback = null)
        // {
        //     var heroModelData = await GameplayDataManager.Instance.GetHeroModelDataAsync(heroId, heroLevel, cancellationToken);
        //     var heroModel = new HeroModel(entityUIdCounter++, heroId, targetPosition, heroModelData, MainHeroModel, movementStrategyType);
        //     var heroGameObject = await PoolManager.Instance.Get(heroId, cancellationToken: cancellationToken, false);
        //     heroGameObject.transform.SetParent(transform);
        //     heroGameObject.layer = Layer.HERO_LAYER;
        //     await BuildHeroEntityGameObjectAsync(heroGameObject, heroModel, spawnPosition, true, cancellationToken);
        //     var removedOldSameDiedHeroModel = DiedHeroModels.FirstOrDefault(x => x.EntityId == heroId);
        //     if (removedOldSameDiedHeroModel != null)
        //         DiedHeroModels.Remove(removedOldSameDiedHeroModel);
        //     HeroModelTransforms.Add(new HeroModelTransform(heroModel, heroGameObject.transform));
        //     callback?.Invoke();
        //     Messenger.Publish(new GameStateChangedMessage(GameStateEventType.HeroSpawned));
        //     return heroModel;
        // }
        //
        // public virtual void RemoveAllHeroes()
        // {
        //     foreach (var heroModelTransform in HeroModelTransforms)
        //     {
        //         RemoveHeroTomb(heroModelTransform.Model.EntityId);
        //         RemoveEntity(heroModelTransform.Transform.gameObject);
        //     }
        //     MainHeroModel = null;
        //     HeroModelTransforms.Clear();
        // }
        //
        // public virtual async UniTask<EnemyModel> CreateEnemyAsync(string enemyId, int enemyLevel, bool isImmortal,
        //                                                           Vector2 spawnPosition, int zoneLevel, float activatedSqrRange, float detectedSqrRange,
        //                                                           bool isElite, float dropEquipmentRate, bool markRespawnable, CancellationToken cancellationToken)
        // {
        //     var enemyModelData = await GameplayDataManager.Instance.GetEnemyModelDataAsync(enemyId, enemyLevel, isElite, activatedSqrRange, detectedSqrRange, cancellationToken);
        //     var enemyModel = new EnemyModel(entityUIdCounter++, enemyId, isImmortal, enemyModelData, zoneLevel, isElite, dropEquipmentRate, markRespawnable);
        //     var enemyGameObject = await PoolManager.Instance.Get(enemyModelData.CharacterVisualId, cancellationToken: cancellationToken, false);
        //     enemyGameObject.transform.SetParent(transform);
        //     enemyGameObject.layer = Layer.ENEMY_LAYER;
        //     await BuildEnemyEntityGameObjectAsync(enemyGameObject, enemyModel, spawnPosition, false, cancellationToken);
        //     EnemyModels.Add(enemyModel);
        //     return enemyModel;
        // }
        //
        // public virtual async UniTask<BossModel> CreateBossAsync(string bossId, int bossLevel, bool isImmortal,
        //                                                         Vector2 spawnPosition, int zoneLevel, float activatedSqrRange, float detectedSqrRange,
        //                                                         float dropEquipmentRate, bool markRespawnable, CancellationToken cancellationToken)
        // {
        //     var bossModelData = await GameplayDataManager.Instance.GetBossModelDataAsync(bossId, bossLevel, activatedSqrRange, detectedSqrRange, cancellationToken);
        //     var bossModel = new BossModel(entityUIdCounter++, bossId, isImmortal, bossModelData, zoneLevel, dropEquipmentRate, markRespawnable);
        //     var bossGameObject = await PoolManager.Instance.Get(bossModelData.CharacterVisualId, cancellationToken: cancellationToken, false);
        //     bossGameObject.transform.SetParent(transform);
        //     bossGameObject.layer = Layer.ENEMY_LAYER;
        //     await BuildBossEntityGameObjectAsync(bossGameObject, bossModel, spawnPosition, false, cancellationToken);
        //     EnemyModels.Add(bossModel);
        //     return bossModel;
        // }
        //
        // public virtual async UniTask<HeroBossModelTransform> CreateHeroBossAsync(string heroId, int heroLevel, float hpScaleFactor, bool isImmortal,
        //                                                                          Vector2 spawnPosition, float activatedSqrRange, float detectedSqrRange, int zoneLevel,
        //                                                                          float dropEquipmentRate, bool markRespawnable, CancellationToken cancellationToken)
        // {
        //     var heroBossStats = await GameplayDataManager.Instance.GetHeroBossStatsAsync(heroId, heroLevel, hpScaleFactor, cancellationToken);
        //     var heroConfigItem = await GameplayDataManager.Instance.GetHeroConfigItem(heroId, cancellationToken);
        //     var skillIdentities = await GameplayDataManager.Instance.GetHeroSkillIdentitiesAsync(heroId, heroLevel, cancellationToken);
        //     var skillModels = await GameplayDataManager.Instance.GetHeroSkillModelsAsync(skillIdentities, cancellationToken);
        //     var bossLevelModel = new BossLevelModel(heroLevel, heroBossStats);
        //     var bossModelData = new BossModelData(heroId, heroId, heroConfigItem.detectedPriority, activatedSqrRange, detectedSqrRange,
        //                                           heroConfigItem.attackType, null, default, -1, skillModels.ToArray(), bossLevelModel);
        //     var heroBossModel = new HeroBossModel(entityUIdCounter++, heroId, hpScaleFactor, isImmortal, bossModelData, zoneLevel, dropEquipmentRate, markRespawnable);
        //     var heroBossGameObject = await PoolManager.Instance.Get(bossModelData.CharacterVisualId, cancellationToken: cancellationToken, false);
        //     heroBossGameObject.transform.SetParent(transform);
        //     heroBossGameObject.layer = Layer.ENEMY_LAYER;
        //     await BuildHeroBossEntityGameObjectAsync(heroBossGameObject, heroBossModel, spawnPosition, true, cancellationToken);
        //     EnemyModels.Add(heroBossModel);
        //     return new HeroBossModelTransform(heroBossModel, heroBossGameObject.transform);
        // }
        //
        // public virtual async UniTask<HeroModel> CreateMyHeroPVPAsync(string heroId, int heroLevel, PVPHeroBuffData pvpHeroBuffData,
        //                                                              Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     RemoveHeroTomb(heroId);
        //     RemoveFromDiedHeroesList(heroId);
        //     var pvpHeroModelData = await GameplayDataManager.Instance.GetPVPHeroModelDataAsync(heroId, heroLevel, pvpHeroBuffData, cancellationToken);
        //     var pvpHeroModel = new PVPHeroModel(entityUIdCounter++, heroId, spawnPosition, pvpHeroModelData, MainHeroModel, MovementStrategyType.Follow, pvpHeroBuffData, false);
        //     var pvpHeroGameObject = await PoolManager.Instance.Get(heroId, cancellationToken: cancellationToken, false);
        //     pvpHeroGameObject.transform.SetParent(transform);
        //     pvpHeroGameObject.layer = Layer.HERO_LAYER;
        //     await BuildHeroEntityGameObjectAsync(pvpHeroGameObject, pvpHeroModel, spawnPosition, true, cancellationToken);
        //     var removedOldSameDiedHeroModel = DiedHeroModels.FirstOrDefault(x => x.EntityId == heroId);
        //     if (removedOldSameDiedHeroModel != null)
        //         DiedHeroModels.Remove(removedOldSameDiedHeroModel);
        //     HeroModelTransforms.Add(new HeroModelTransform(pvpHeroModel, pvpHeroGameObject.transform));
        //     Messenger.Publish(new GameStateChangedMessage(GameStateEventType.HeroSpawned));
        //     return pvpHeroModel;
        // }
        //
        // public virtual async UniTask<HeroModelTransform> CreateOpponentHeroPVPAsync(string heroId, int heroLevel, PVPHeroBuffData pvpHeroBuffData,
        //                                                                             Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     var pvpHeroModelData = await GameplayDataManager.Instance.GetPVPHeroModelDataAsync(heroId, heroLevel, pvpHeroBuffData, cancellationToken);
        //     var pvpHeroModel = new PVPHeroModel(entityUIdCounter++, heroId, spawnPosition, pvpHeroModelData, MainHeroModel, MovementStrategyType.Follow, pvpHeroBuffData, true);
        //     var pvpHeroGameObject = await PoolManager.Instance.Get(heroId, cancellationToken: cancellationToken, false);
        //     pvpHeroGameObject.transform.SetParent(transform);
        //     pvpHeroGameObject.layer = Layer.ENEMY_LAYER;
        //     await BuildOpponentHeroPVPEntityGameObjectAsync(pvpHeroGameObject, pvpHeroModel, spawnPosition, true, cancellationToken);
        //     return new HeroModelTransform(pvpHeroModel, pvpHeroGameObject.transform);
        // }
        //
        // public virtual async UniTask<ObjectModel> CreateObjectAsync(string objectId, ObjectType objectType, Vector2 spawnPosition,
        //                                                             bool markRespawnable, CancellationToken cancellationToken)
        // {
        //     var objectModelData = await GameplayDataManager.Instance.GetObjectModelDataAsync(objectId, cancellationToken);
        //     var objectModel = new ObjectModel(entityUIdCounter++, objectId, markRespawnable, objectType, objectModelData);
        //     var objectGameObject = await PoolManager.Instance.Get(objectId, cancellationToken: cancellationToken, false);
        //     objectGameObject.transform.SetParent(transform);
        //     objectGameObject.layer = Layer.OBJECT_LAYER;
        //     await BuildObjectEntityGameObjectAsync(objectGameObject, objectModel, spawnPosition, false, cancellationToken);
        //     return objectModel;
        // }
        //
        // public virtual async UniTask<GameObject> CreateHeroTombAsync(string heroId, Vector2 spawnPosition, TickTimer tickTimer, CancellationToken cancellationToken)
        // {
        //     var heroTombId = Constant.HERO_TOMB_ID;
        //     var heroTombGameObject = await PoolManager.Instance.Get(heroTombId, cancellationToken: cancellationToken);
        //     heroTombGameObject.transform.position = spawnPosition;
        //     heroTombGameObject.transform.SetParent(transform);
        //     HeroTombIdentities.Add(new HeroTombIdentiy(heroId, heroTombGameObject));
        //     if (tickTimer != null)
        //     {
        //         HeroTomb heroTomb = heroTombGameObject.GetComponent<HeroTomb>();
        //         if (heroTomb)
        //         {
        //             heroTomb.Init(tickTimer);
        //         }
        //     }
        //     return heroTombGameObject;
        // }
        //
        // public virtual async UniTask<GameObject> CreateObjectRelicAsync(string objectRelicId, Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     var objectRelicGameObject = await PoolManager.Instance.Get(objectRelicId, cancellationToken: cancellationToken);
        //     objectRelicGameObject.transform.position = spawnPosition;
        //     objectRelicGameObject.transform.SetParent(transform);
        //     return objectRelicGameObject;
        // }
        //
        // public virtual async UniTask<GameObject> CreateChestAsync(string chestId, Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     var chestGameObject = await PoolManager.Instance.Get(chestId, cancellationToken: cancellationToken);
        //     chestGameObject.transform.position = spawnPosition;
        //     chestGameObject.transform.SetParent(transform);
        //     return chestGameObject;
        // }
        //
        // public virtual async UniTask<GameObject> CreatePortalGateAsync(bool isOutBasePortalGate, Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     var portalGatePrefabId = PortalConstant.PORTAL_GATE_ID;
        //     var portalGateGameObject = await PoolManager.Instance.Get(portalGatePrefabId, cancellationToken: cancellationToken);
        //     var portalGate = portalGateGameObject.GetComponent<PortalGate>();
        //     portalGate.Init(isOutBasePortalGate);
        //     portalGateGameObject.transform.position = spawnPosition;
        //     portalGateGameObject.transform.SetParent(transform);
        //     return portalGateGameObject;
        // }
        //
        // public virtual async UniTask CreatePortalHeroEffectAsync(bool isDisappearEffect, Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     var portalHeroEffectId = isDisappearEffect ? VFXKey.PORTAL_HERO_DISAPPEAR_EFFECT_ID : VFXKey.PORTAL_HERO_APPEAR_EFFECT_ID;
        //     var portalHeroEffectGameObject = await PoolManager.Instance.Get(portalHeroEffectId, cancellationToken: cancellationToken);
        //     portalHeroEffectGameObject.transform.position = spawnPosition;
        //     portalHeroEffectGameObject.transform.SetParent(transform);
        // }
        //
        // public virtual async UniTask CreateImmortalEnemyAppearEffectAsync(Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     var immortalEnemeyEffectId = VFXKey.IMMORTAL_ENEMY_APPEAR_EFFECT_ID;
        //     var immortalEnemyAppearEffectGameObject = await PoolManager.Instance.Get(immortalEnemeyEffectId, cancellationToken: cancellationToken);
        //     immortalEnemyAppearEffectGameObject.transform.position = spawnPosition;
        //     immortalEnemyAppearEffectGameObject.transform.SetParent(transform);
        // }
        //
        // public virtual async UniTask<GameObject> CreateProjectileAsync(string projectileId, Vector2 spawnPosition, CharacterModel creatorModel, CancellationToken cancellationToken)
        // {
        //     var projectileGameObject = await PoolManager.Instance.Get(projectileId, cancellationToken);
        //     projectileGameObject.GetComponent<IProjectile>().Build(creatorModel, spawnPosition);
        //     projectileGameObject.transform.SetParent(transform);
        //     return projectileGameObject;
        // }
        //
        // public virtual void RespawnDiedHeroInstantly(bool isPlayerHero, string diedHeroId, float respawnedMaxHpPercent, CancellationToken cancellationToken)
        // {
        //     if (isPlayerHero)
        //     {
        //         foreach (var diedHeroModel in DiedHeroModels)
        //         {
        //             if (diedHeroModel.EntityId == diedHeroId)
        //             {
        //                 RunRespawnDiedHeroInstantlyAsync(diedHeroModel, respawnedMaxHpPercent, cancellationToken).Forget();
        //                 break;
        //             }
        //         }
        //     }
        // }
        //
        // public virtual Vector2 RespawnBossInstantly(string entityId, CancellationToken cancellationToken)
        //     => Vector2.zero;
        //
        // public void RemoveGameObject(GameObject removedGameObject)
        //     => PoolManager.Instance.Remove(removedGameObject);
        //
        // public void RemoveEntity(GameObject removedEntityGameObject)
        // {
        //     var disposableEntity = removedEntityGameObject.GetComponent<Disposable>();
        //     if (disposableEntity != null)
        //         disposableEntity.Dispose();
        //     RemoveGameObject(removedEntityGameObject);
        // }
        //
        // public List<EnemyModel> GetListEnemyActive()
        // {
        //     List<EnemyModel> result = new List<EnemyModel>();
        //     foreach (var enemyModel in EnemyModels)
        //     {
        //         if (enemyModel.IsActive && !enemyModel.IsDead)
        //             result.Add(enemyModel);
        //     }
        //     return result;
        // }
        //
        // public List<EnemyModel> GetCurrentListEnemyActiveByEnemyId(string entityID)
        // {
        //     List<EnemyModel> result = new List<EnemyModel>();
        //     foreach (var enemyModel in EnemyModels)
        //     {
        //         if (enemyModel.IsActive && !enemyModel.IsDead && string.Equals(enemyModel.EntityId, entityID))
        //             result.Add(enemyModel);
        //     }
        //     return result;
        // }
        //
        // public List<EnemyModel> GetListEnemy()
        // {
        //     List<EnemyModel> result = new List<EnemyModel>();
        //     foreach (var enemyModel in EnemyModels)
        //     {
        //         result.Add(enemyModel);
        //     }
        //     return result;
        // }
        //
        // public virtual void HandleHeroDied(HeroDiedMessage heroDiedMessage, CancellationToken cancellationToken, out PlayResult playResult)
        // {
        //     CreateHeroTombAsync(heroDiedMessage.HeroModel.EntityId, heroDiedMessage.HeroModel.Position, null, cancellationToken).Forget();
        //     var removedHeroModelTransform = HeroModelTransforms.FirstOrDefault(x => x.Model == heroDiedMessage.HeroModel);
        //     HeroModelTransforms.Remove(removedHeroModelTransform);
        //     if (heroDiedMessage.HeroModel.FollowingModel == null)
        //     {
        //         if (HeroModelTransforms.Count > 0)
        //         {
        //             var newFollowingModel = HeroModelTransforms[UnityRandom.Range(0, HeroModelTransforms.Count)].Model;
        //             MainHeroModel = newFollowingModel;
        //             newFollowingModel.SetFollowingModel(null);
        //             foreach (var heroModelTransform in HeroModelTransforms)
        //             {
        //                 if (heroModelTransform.Model != MainHeroModel)
        //                     heroModelTransform.Model.SetFollowingModel(MainHeroModel);
        //             }
        //             DiedHeroModels.Add(heroDiedMessage.HeroModel);
        //             playResult = PlayResult.None;
        //         }
        //         else
        //         {
        //             DiedHeroModels.Clear();
        //             MainHeroModel = null;
        //             playResult = PlayResult.LostGame;
        //         }
        //         DataManager.Transitioned.SetLastHeroStandPosition(heroDiedMessage.HeroModel.Position);
        //     }
        //     else
        //     {
        //         DiedHeroModels.Add(heroDiedMessage.HeroModel);
        //         playResult = PlayResult.None;
        //     }
        // }
        //
        // public virtual List<HeroModelTransform> GetTargetHeroModelTransforms(bool targetPlayerHeroes)
        // {
        //     if (targetPlayerHeroes)
        //         return HeroModelTransforms;
        //     else
        //         return null;
        // }
        //
        // public virtual List<SkillHeroModelTransform> GetSkillHeroModelTransforms(bool isPlayerHero)
        // {
        //     var skillHeroModelTransforms = new List<SkillHeroModelTransform>();
        //     if (isPlayerHero)
        //     {
        //         foreach (var heroModelTransform in HeroModelTransforms)
        //         {
        //             var skillHeroModelTransform = new SkillHeroModelTransform(heroModelTransform.Model, heroModelTransform.Transform);
        //             skillHeroModelTransforms.Add(skillHeroModelTransform);
        //         }
        //     }
        //     return skillHeroModelTransforms;
        // }
        //
        // public virtual List<CharacterModel> GetSkillHeroModelsDied(bool isPlayerHero)
        // {
        //     var skillHeroModelsDied = new List<CharacterModel>();
        //     if (isPlayerHero)
        //         skillHeroModelsDied.AddRange(DiedHeroModels);
        //     return skillHeroModelsDied;
        // }
        //
        // public virtual bool HasSkillHeroDied(bool isPlayerHero)
        // {
        //     if (isPlayerHero)
        //         return DiedHeroModels.Count > 0;
        //     else
        //         return false;
        // }
        //
        // public virtual void HandleEnemyDied(EnemyDiedMessage enemyDiedMessage, CancellationToken cancellationToken)
        //     => EnemyModels.Remove(enemyDiedMessage.EnemyModel);
        //
        // public virtual void HandleObjectDestroyed(ObjectDestroyedMessage objectDestroyedMessage, CancellationToken cancellationToken) { }
        //
        // public async UniTask CreateArrowAsync(string arrowId, Vector2 spawnPosition, CancellationToken cancellationToken)
        // {
        //     RemoveArrow();
        //     _arrowGameObject = await PoolManager.Instance.Get(arrowId, cancellationToken: cancellationToken);
        //     _arrowGameObject.transform.position = spawnPosition;
        // }
        //
        // public void RemoveArrow()
        // {
        //     if (_arrowGameObject != null)
        //     {
        //         PoolManager.Instance.Remove(_arrowGameObject);
        //         _arrowGameObject = null;
        //     }
        // }
        //
        // protected virtual async UniTask BuildHeroEntityGameObjectAsync(GameObject heroGameObject, HeroModel heroModel, Vector2 spawnPosition,
        //                                                                bool setActive, CancellationToken cancellationToken)
        // {
        //     var heroEntity = heroGameObject.GetComponent<IEntity>();
        //     heroEntity.Build(heroModel, spawnPosition);
        //     heroGameObject.SetActive(setActive);
        //     await UniTask.CompletedTask;
        // }
        //
        // protected virtual async UniTask BuildHeroBossEntityGameObjectAsync(GameObject heroBossGameObject, HeroBossModel heroBossModel, Vector2 spawnPosition,
        //                                                                    bool setActive, CancellationToken cancellationToken)
        // {
        //     var heroBossEntity = heroBossGameObject.GetComponent<IEntity>();
        //     heroBossEntity.Build(heroBossModel, spawnPosition);
        //     heroBossGameObject.SetActive(setActive);
        //     await UniTask.CompletedTask;
        // }
        //
        // protected virtual async UniTask BuildOpponentHeroPVPEntityGameObjectAsync(GameObject pvpHeroGameObject, HeroModel heroModel, Vector2 spawnPosition,
        //                                                                           bool setActive, CancellationToken cancellationToken)
        // {
        //     var heroEntity = pvpHeroGameObject.GetComponent<IEntity>();
        //     heroEntity.Build(heroModel, spawnPosition);
        //     pvpHeroGameObject.SetActive(setActive);
        //     await UniTask.CompletedTask;
        // }
        //
        // protected async UniTask BuildEnemyEntityGameObjectAsync(GameObject enemyGameObject, EnemyModel enemyModel, Vector2 spawnPosition,
        //                                                         bool setActive, CancellationToken cancellationToken)
        // {
        //     var enemyEntity = enemyGameObject.GetComponent<IEntity>();
        //     enemyEntity.Build(enemyModel, spawnPosition);
        //     enemyGameObject.SetActive(setActive);
        //     ProximityManager.Instance.Add(enemyEntity);
        //     await UniTask.CompletedTask;
        // }
        //
        // protected async UniTask BuildBossEntityGameObjectAsync(GameObject bossGameObject, BossModel bossModel, Vector2 spawnPosition,
        //                                                        bool setActive, CancellationToken cancellationToken)
        // {
        //     var bossEntity = bossGameObject.GetComponent<IEntity>();
        //     bossEntity.Build(bossModel, spawnPosition);
        //     bossGameObject.SetActive(setActive);
        //     ProximityManager.Instance.Add(bossEntity);
        //     await UniTask.CompletedTask;
        // }
        //
        // protected async UniTask BuildObjectEntityGameObjectAsync(GameObject objectGameObject, ObjectModel objectModel, Vector2 spawnPosition,
        //                                                          bool setActive, CancellationToken cancellationToken)
        // {
        //     var objectEntity = objectGameObject.GetComponent<IEntity>();
        //     objectEntity.Build(objectModel, spawnPosition);
        //     objectGameObject.SetActive(setActive);
        //     ProximityManager.Instance.Add(objectEntity);
        //     await UniTask.CompletedTask;
        // }
        //
        // protected virtual void SetUpLeaderHeroModel(HeroModel justCreatedHeroModel)
        // {
        //     var heroFragment = DataManager.Local.HeroFragments.FirstOrDefault(x=>x.id == justCreatedHeroModel.EntityId);
        //     if (heroFragment.isLeader)
        //         MainHeroModel = justCreatedHeroModel;
        // }
        //
        // protected virtual async UniTask RunRespawnDiedHeroInstantlyAsync(HeroModel diedHeroModel, float respawnedMaxHpPercent, CancellationToken cancellationToken)
        // {
        //     var heroId = diedHeroModel.EntityId;
        //     var heroLevel = diedHeroModel.Level;
        //     var heroMovementStrategyType = MainHeroModel.MovementStrategyType;
        //     var heroSpawnPosition = diedHeroModel.Position;
        //     if (ScreenUtility.IsPositionOffScreen(heroSpawnPosition))
        //     {
        //         if (HeroModelTransforms.Count == 1)
        //         {
        //             var heroSpawnPoints = TransformUtility.GetPositionsAroundPoint(HeroModelTransforms[0].Model.Position);
        //             heroSpawnPosition = heroSpawnPoints[UnityRandom.Range(0, heroSpawnPoints.Count)];
        //         }
        //         else
        //         {
        //             var groupCenterPosition = Vector2.zero;
        //             foreach (var heroModelTransform in HeroModelTransforms)
        //                 groupCenterPosition += heroModelTransform.Model.Position;
        //             groupCenterPosition /= HeroModelTransforms.Count;
        //             heroSpawnPosition = groupCenterPosition;
        //         }
        //     }
        //     var respawnedHeroModel = await CreateHeroAsync(heroId, heroLevel, heroMovementStrategyType, heroSpawnPosition, cancellationToken);
        //     var deductedMaxHpPercent = Mathf.Clamp01(1.0f - respawnedMaxHpPercent);
        //     respawnedHeroModel.DeductHp(deductedMaxHpPercent, DamageSource.None);
        // }
        //
        // private void RemoveHeroTomb(string heroId)
        // {
        //     foreach (var heroTombIdentity in HeroTombIdentities)
        //     {
        //         if (heroTombIdentity.HeroId == heroId)
        //         {
        //             HeroTombIdentities.Remove(heroTombIdentity);
        //             PoolManager.Instance.Remove(heroTombIdentity.HeroTombGameObject);
        //             break;
        //         }
        //     }
        // }
        //
        // private void RemoveFromDiedHeroesList(string heroId)
        // {
        //     foreach (var diedHeroModel in DiedHeroModels)
        //     {
        //         if (diedHeroModel.EntityId == heroId)
        //         {
        //             DiedHeroModels.Remove(diedHeroModel);
        //             break;
        //         }
        //     }
        // }

        #endregion Class Methods
    }

    // public struct HeroModelTransform
    // {
    //     #region Members
    //
    //     public readonly HeroModel Model;
    //     public readonly Transform Transform;
    //
    //     #endregion Members
    //
    //     #region Struct Methods
    //
    //     public HeroModelTransform(HeroModel model, Transform transform)
    //     {
    //         this.Model = model;
    //         this.Transform = transform;
    //     }
    //
    //     #endregion Struct Methods
    // }
    //
    // public struct HeroBossModelTransform
    // {
    //     #region Members
    //
    //     public readonly BossModel Model;
    //     public readonly Transform Transform;
    //
    //     #endregion Members
    //
    //     #region Struct Methods
    //
    //     public HeroBossModelTransform(BossModel model, Transform transform)
    //     {
    //         this.Model = model;
    //         this.Transform = transform;
    //     }
    //
    //     #endregion Struct Methods
    // }

   
    public struct HeroTombIdentiy
    {
        #region Members

        public readonly string HeroId;
        public readonly GameObject HeroTombGameObject;

        #endregion Members

        #region Struct Methods

        public HeroTombIdentiy(string heroId, GameObject heroTombGameObject)
        {
            HeroId = heroId;
            HeroTombGameObject = heroTombGameObject;
        }

        #endregion Struct Methods
    }
}