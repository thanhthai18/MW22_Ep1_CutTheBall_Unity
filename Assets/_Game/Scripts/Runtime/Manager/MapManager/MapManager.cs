using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Definition;
using Runtime.Message;
using Runtime.Common.Singleton;
using Runtime.SceneLoading;
using Cysharp.Threading.Tasks;

namespace Runtime.Gameplay.Map
{
    public abstract class MapManager : MonoSingleton<MapManager>
    {
        #region Members

        protected bool hasSpawnedInitialHeroes;

        #endregion Members

        #region Properties

        public virtual Vector2 ClosestChestPoint
            => Vector2.zero;

        #endregion Properties

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            SceneManager.RegisterWaitedTaskBeforeNewSceneAppeared(WaitForInitialHeroesToBeSpawnedAsync);
        }

        #endregion API Methods

        #region Class Methods

        public virtual void HandleDataLoaded(CancellationToken cancellationToken) { }
        public abstract List<string> GetActiveMapAreaIds();
        public virtual bool CheckEnemyCanMoveToPosition(Vector2 moveToPosition) => true;
        public virtual Vector2 GetRestRandomHeroMoveToPosition(Vector3 startPosition) => Vector2.zero;
        public virtual Vector2 GetPositionForQuestToTarget(string entityId, string chunk, string zone) => Vector2.zero;
        public virtual Vector2 GetFogGatePosition(string chunkId, string fogId) => Vector2.zero;
        public virtual Vector2 GetSaveHeroPosition(string chunkId, string saveHeroGateId) => Vector2.zero;
        public virtual Vector2 GetBuildingPosition(string chunkId, string buildingId) => Vector2.zero;
        public virtual Vector2 GetTeleportPortalPosition(string chunkId, string teleportId) => Vector2.zero;


        protected virtual async UniTask SpawnInitialHeroesAsync(CancellationToken cancellationToken)
        {
            hasSpawnedInitialHeroes = true;
            await UniTask.CompletedTask;
        }

        private async UniTask WaitForInitialHeroesToBeSpawnedAsync(CancellationToken cancellationToken)
        {
            while (!hasSpawnedInitialHeroes)
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }

        #endregion Class Methods
    }
}