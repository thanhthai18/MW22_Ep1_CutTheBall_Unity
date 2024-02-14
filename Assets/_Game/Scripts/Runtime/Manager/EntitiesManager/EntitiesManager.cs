using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Runtime.Definition;
using Runtime.Common.Singleton;
using Runtime.Gameplay.EntitySystem;
using Runtime.Manager.Pool;
using Runtime.Message;
using Runtime.Manager.Data;
using Cysharp.Threading.Tasks;

namespace Runtime.Gameplay.Manager
{
    public class EntitiesManager : MonoSingleton<EntitiesManager>
    {
        #region Members

        protected uint entityUIdCounter;

        #endregion Members

        #region Properties

        protected List<BallModel> BallModels { get; set; }
        protected List<BallModel> BoomModels { get; set; }
        protected string CurrentBallId { get; private set; }
        protected string CurrentBoomId { get; private set; }

        #endregion Properties

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            entityUIdCounter = 0;
            BallModels = new();
            BoomModels = new();
            CurrentBallId = Constant.GetBallId(DataManager.Local.PlayerSkinId);
            CurrentBoomId = Constant.GetBoomId();
        }

        #endregion API Methods

        #region Class Methods

        public virtual void CreateDefaultEntityAsync(EntityType entityType, Vector2 spawnPos, CancellationToken cancellationToken)
        {
            if (entityType == EntityType.Ball)
            {
                CreateDefautBallAsync(spawnPos, cancellationToken).Forget();
            }
            else if(entityType == EntityType.Boom)
            {
                CreateDefaultBoomAsync(spawnPos, cancellationToken).Forget();
            }
        }
        
        public virtual async UniTask<BallModel> CreateBallByIdAsync(string ballId, Vector2 spawnPosition, CancellationToken cancellationToken)
        {
            var ballModelData = await GameplayDataManager.Instance.GetBallModelDataAsync(ballId, cancellationToken);
            var ballModel = EntityModelFactory.GetEntityModel(EntityType.Ball, entityUIdCounter++, ballId, ballModelData) as BallModel;
            var ballGameObject = await PoolManager.Instance.Get(ballId, cancellationToken: cancellationToken, false);
            ballGameObject.transform.SetParent(transform);
            ballGameObject.layer = Layer.BALL_LAYER;
            await BuildEntityGameObjectAsync(ballGameObject, ballModel, spawnPosition, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BallSpawned));
            return ballModel;
        }
        
        public virtual async UniTask<BoomModel> CreateBoomByIdAsync(string boomId, Vector2 spawnPosition, CancellationToken cancellationToken)
        {
            var boomModelData = await GameplayDataManager.Instance.GetBoomModelDataAsync(boomId, cancellationToken);
            var boomModel = EntityModelFactory.GetEntityModel(EntityType.Boom, entityUIdCounter++, boomId, boomModelData) as BoomModel;
            var boomGameObject = await PoolManager.Instance.Get(boomId, cancellationToken: cancellationToken, false);
            boomGameObject.transform.SetParent(transform);
            boomGameObject.layer = Layer.BOOM_LAYER;
            await BuildEntityGameObjectAsync(boomGameObject, boomModel, spawnPosition, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BoomSpawned));
            return boomModel;
        }
        
        public virtual async UniTask<BallModel> CreateDefautBallAsync(Vector2 spawnPosition, CancellationToken cancellationToken)
        {
            var ballId = CurrentBallId;
            var ballModelData = await GameplayDataManager.Instance.GetBallModelDataAsync(ballId, cancellationToken);
            var ballModel = EntityModelFactory.GetEntityModel(EntityType.Ball, entityUIdCounter++, ballId, ballModelData) as BallModel;
            var ballGameObject = await PoolManager.Instance.Get(ballId, cancellationToken: cancellationToken, false);
            ballGameObject.transform.SetParent(transform);
            ballGameObject.layer = Layer.BALL_LAYER;
            await BuildEntityGameObjectAsync(ballGameObject, ballModel, spawnPosition, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BallSpawned));
            return ballModel;
        }
        
        public virtual async UniTask<BoomModel> CreateDefaultBoomAsync(Vector2 spawnPosition, CancellationToken cancellationToken)
        {
            var boomId = CurrentBoomId;
            var boomModelData = await GameplayDataManager.Instance.GetBoomModelDataAsync(boomId, cancellationToken);
            var boomModel = EntityModelFactory.GetEntityModel(EntityType.Boom, entityUIdCounter++, boomId, boomModelData) as BoomModel;
            var boomGameObject = await PoolManager.Instance.Get(boomId, cancellationToken: cancellationToken, false);
            boomGameObject.transform.SetParent(transform);
            boomGameObject.layer = Layer.BOOM_LAYER;
            await BuildEntityGameObjectAsync(boomGameObject, boomModel, spawnPosition, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BoomSpawned));
            return boomModel;
        }
        
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
       
        protected virtual async UniTask BuildEntityGameObjectAsync(GameObject entityGameObject, EntityModel entityModel, Vector2 spawnPosition,
                                                                       bool setActive, CancellationToken cancellationToken)
        {
            var ballEntity = entityGameObject.GetComponent<IEntityStrategy>();
            ballEntity.Build(entityModel, spawnPosition);
            ballEntity.SetActive(setActive);
            await UniTask.CompletedTask;
        }

        #endregion Class Methods
    }
}