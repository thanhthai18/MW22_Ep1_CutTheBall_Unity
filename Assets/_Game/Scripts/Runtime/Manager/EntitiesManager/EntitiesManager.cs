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

        public virtual void CreateDefaultEntityAsync(EntityType entityType, Vector2 spawnPos, Vector2 destinationPos, CancellationToken cancellationToken)
        {
            if (entityType == EntityType.Ball)
            {
                CreateDefautBallAsync(spawnPos, destinationPos, cancellationToken).Forget();
            }
            else if(entityType == EntityType.Boom)
            {
                CreateDefaultBoomAsync(spawnPos, destinationPos, cancellationToken).Forget();
            }
        }
        
        public virtual async UniTask<BallModel> CreateBallByIdAsync(string ballId, Vector2 spawnPosition, CancellationToken cancellationToken)
        {
            var ballModelData = await GameplayDataManager.Instance.GetBallModelDataAsync(ballId, cancellationToken);
            var ballModel = new BallModel(entityUIdCounter++, ballId, ballModelData);
            var ballGameObject = await PoolManager.Instance.Get(ballId, cancellationToken: cancellationToken, false);
            ballGameObject.transform.SetParent(transform);
            ballGameObject.layer = Layer.BALL_LAYER;
            var desinationPosition = Constant.GetRandomStartPosition();
            await BuildEntityGameObjectAsync(ballGameObject, ballModel, spawnPosition, desinationPosition, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BallSpawned));
            return ballModel;
        }
        
        public virtual async UniTask<BoomModel> CreateBoomByIdAsync(string boomId, Vector2 spawnPosition, Vector2 desinationPos, CancellationToken cancellationToken)
        {
            var boomModelData = await GameplayDataManager.Instance.GetBoomModelDataAsync(boomId, cancellationToken);
            var boomModel = new BoomModel(entityUIdCounter++, boomId, boomModelData);
            var boomGameObject = await PoolManager.Instance.Get(boomId, cancellationToken: cancellationToken, false);
            boomGameObject.transform.SetParent(transform);
            boomGameObject.layer = Layer.BOOM_LAYER;
            await BuildEntityGameObjectAsync(boomGameObject, boomModel, spawnPosition, desinationPos, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BoomSpawned));
            return boomModel;
        }
        
        public virtual async UniTask<BallModel> CreateDefautBallAsync(Vector2 spawnPosition, Vector2 destinationPos, CancellationToken cancellationToken)
        {
            var ballId = CurrentBallId;
            var ballModelData = await GameplayDataManager.Instance.GetBallModelDataAsync(ballId, cancellationToken);
            var ballModel = new BallModel(entityUIdCounter++, ballId, ballModelData);
            var ballGameObject = await PoolManager.Instance.Get(ballId, cancellationToken: cancellationToken, false);
            ballGameObject.transform.SetParent(transform);
            ballGameObject.layer = Layer.BALL_LAYER;
            await BuildEntityGameObjectAsync(ballGameObject, ballModel, spawnPosition, destinationPos, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BallSpawned));
            return ballModel;
        }
        
        public virtual async UniTask<BoomModel> CreateDefaultBoomAsync(Vector2 spawnPosition, Vector2 destinationPos, CancellationToken cancellationToken)
        {
            var boomId = CurrentBoomId;
            var boomModelData = await GameplayDataManager.Instance.GetBoomModelDataAsync(boomId, cancellationToken);
            var boomModel = new BoomModel(entityUIdCounter++, boomId, boomModelData);
            var boomGameObject = await PoolManager.Instance.Get(boomId, cancellationToken: cancellationToken, false);
            boomGameObject.transform.SetParent(transform);
            boomGameObject.layer = Layer.BOOM_LAYER;
            await BuildEntityGameObjectAsync(boomGameObject, boomModel, spawnPosition, destinationPos, true, cancellationToken);
            Messenger.Publish(new GameStateChangedMessage(GameStateEventType.BoomSpawned));
            return boomModel;
        }
        
        protected virtual async UniTask BuildEntityGameObjectAsync(GameObject entityGameObject, 
                                                                   EntityModel entityModel, 
                                                                   Vector2 spawnPosition,
                                                                   Vector2 destinationPosition,
                                                                   bool setActive,
                                                                   CancellationToken cancellationToken)
        {
            var ballEntity = entityGameObject.GetComponent<IEntityStrategy>();
            ballEntity.Build(entityModel, spawnPosition, destinationPosition);
            ballEntity.SetActive(setActive);
            await UniTask.CompletedTask;
        }

        #endregion Class Methods
    }
}