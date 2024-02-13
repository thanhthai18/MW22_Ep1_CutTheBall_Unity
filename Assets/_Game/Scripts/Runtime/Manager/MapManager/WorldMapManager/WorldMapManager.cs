using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Shared.Views;
using Runtime.Definition;
using Runtime.Gameplay.Manager;
using Runtime.Manager.Data;
using Runtime.Manager.Pool;
using Runtime.Message;
using Runtime.Manager.Tick;
using Runtime.Utilities;
using Runtime.Extensions;
using Runtime.Localization;
using Runtime.Manager.Toast;
using Runtime.Gameplay.EntitySystem;
using Runtime.UI;
using Runtime.SceneLoading;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using UnityRandom = UnityEngine.Random;

namespace Runtime.Gameplay.Map
{
    public sealed class WorldMapManager : MapManager
    {
        #region Members

        [Header("--- All WORLD MAP CHUNK SCENE PARTS ---")]
        [SerializeField]
        private WorldMapChunkScenePart[] _worldMapChunkSceneParts;

        [Header("--- WORLD MAP BEACH CHUNK SCENE PART ---")]
        [SerializeField]
        private WorldMapChunkScenePart _worldMapBeachChunkScenePart;

        [Header("--- WORLD MAP BASE CHUNK SCENE PART ---")]
        [SerializeField]
        private WorldMapChunkScenePart _worldMapBaseChunkScenePart;

        [SerializeField]
        [ReadOnly]
        private List<WorldMapChunk> _activeWorldMapChunks;

        private List<Vector2> _beachRandomHeroSpawnPositions;
        private List<Vector2> _baseRandomHeroSpawnPositions;
        private Vector2[] _restRandomHeroMoveToPositions;
        private Vector2 _basePortalGatePosition;
        private ConvexPolygonArea _baseNoTrespassArea;
        private int _countdownRespawnRandomChestTickId;
        private int _spawnRandomChestTickId;
        private int _spawnBossChestTickId;
        private GameObject _presentBossChestGameObject;
        private GameObject _presentRandomChestGameObject;
        private GameObject _outBasePortalGameObject;
        private GameObject _inBasePortalGameObject;
        private Vector2 _lastOutBasePortalGatePosition;
        private bool _isSpawningReviveHeroes;
        
        #endregion Members

        #region Properties

//         public override Vector2 ClosestChestPoint
//         {
//             get
//             {
//                 var closestChestPoint = Vector2.zero;
//                 float closestSqrDistance = float.MaxValue;
//                 var heroModelTransforms = EntitiesManager.Instance.HeroModelTransforms;
//                 var groupCenterPosition = Vector2.zero;
//
//                 foreach (var heroModelTransform in heroModelTransforms)
//                     groupCenterPosition += heroModelTransform.Model.Position;
//                 groupCenterPosition /= heroModelTransforms.Count;
//
//                 foreach (var activeWorldMapChunk in _activeWorldMapChunks)
//                 {
//                     if (activeWorldMapChunk.ChestSpawnPositions == null)
//                         continue;
//
//                     for (int i = 0; i < activeWorldMapChunk.ChestSpawnPositions.Length; i++)
//                     {
//                         var isChestPointOffScreen = IsChestPointOffScreen(activeWorldMapChunk.ChestSpawnPositions[i]);
//                         if (isChestPointOffScreen)
//                         {
//                             var sqrDistanceBetween = (groupCenterPosition - activeWorldMapChunk.ChestSpawnPositions[i]).sqrMagnitude;
//                             if (closestSqrDistance > sqrDistanceBetween)
//                             {
//                                 closestSqrDistance = sqrDistanceBetween;
//                                 closestChestPoint = activeWorldMapChunk.ChestSpawnPositions[i];
//                             }
//                         }
//                     }
//                 }
//
//                 return closestChestPoint;
//             }
//         }
//
//         public bool HasOutBasePortal => _outBasePortalGameObject != null;
//         public bool HasInBasePortal => _inBasePortalGameObject != null;
//         public Vector2 OutBasePortalPosition => _outBasePortalGameObject.transform.position;
//         public Vector2 InBasePortalPosition => _inBasePortalGameObject.transform.position;
//
//         #endregion Properties
//
//         #region API Methods
//
//         protected override void Awake()
//         {
//             base.Awake();
//             _activeWorldMapChunks = new List<WorldMapChunk>();
//             foreach (var worldMapChunkScenePart in _worldMapChunkSceneParts)
//             {
//                 var worldMapChunk = worldMapChunkScenePart.MapArea;
//                 worldMapChunk.Init(this);
//             }
//         }
//
//         #endregion API Methods
//
//         #region Class Methods
//
//         public void AddActiveMapChunk(WorldMapChunk worldMapChunk)
//             => _activeWorldMapChunks.Add(worldMapChunk);
//
//         public void RemoveActiveMapChunk(WorldMapChunk worldMapChunk)
//             => _activeWorldMapChunks.Remove(worldMapChunk);
//
//         public void AddBaseRandomHeroSpawnPositions(List<Vector2> baseRandomHeroSpawnPositions)
//             => _baseRandomHeroSpawnPositions = baseRandomHeroSpawnPositions;
//
//         public void AddBeachRandomHeroSpawnPositions(List<Vector2> beachRandomHeroSpawnPositions)
//             => _beachRandomHeroSpawnPositions = beachRandomHeroSpawnPositions;
//
//         public void SetBasePortalGatePosition(Vector2 basePortalGatePosition)
//             => _basePortalGatePosition = basePortalGatePosition;
//
//         public override void HandleDataLoaded(CancellationToken cancellationToken)
//         {
//             base.HandleDataLoaded(cancellationToken);
//
//             foreach (var worldMapChunkScenePart in _worldMapChunkSceneParts)
//                 worldMapChunkScenePart.Hide();
//
//             var hasGoneToDungeon = DataManager.Transitioned.HasGoneToDungeon;
//             if (hasGoneToDungeon)
//             {
//                 var isOnDungeonScrollMode = DataManager.Transitioned.IsOnDungeonScrollMode;
//                 if (isOnDungeonScrollMode)
//                 {
//                     _worldMapBaseChunkScenePart.Show();
//                 }
//                 else
//                 {
//                     var dungeonGoneToBelongedMapAreaId = DataManager.Transitioned.DungeonGoneToBelongedMapAreaId;
//                     var shownMapChunk = GetWorlMapChunkScenePart(dungeonGoneToBelongedMapAreaId);
//                     shownMapChunk.Show();
//                 }
//                 DataManager.Transitioned.DungeonGoneToBelongedMapAreaId = "";
//             }
//             else
//             {
//                 var hasUnlockedBaseMainBuilding = DataManager.Local.HasUnlockedBaseMainBuilding;
//                 if (hasUnlockedBaseMainBuilding)
//                 {
//                     _worldMapBaseChunkScenePart.Show();
//                     HandleBaseMainBuildingFeatureUnlocked();
//                 }
//                 else _worldMapBeachChunkScenePart.Show();
//             }
//
//             SpawnInitialStuffAsync(cancellationToken).Forget();
//         }
//
//         public override void HandleEnemyDied(EnemyDiedMessage enemyDiedMessage, CancellationToken cancellationToken)
//         {
//             base.HandleEnemyDied(enemyDiedMessage, cancellationToken);
//             if (enemyDiedMessage.IsBoss)
//             {
//                 var chestSpawnPoint = enemyDiedMessage.EnemyModel.Position;
//                 SpawnBossChest(chestSpawnPoint, cancellationToken);
//             }
//         }
//
//         public override List<CustomNavigationObstacle> GetNavigationObstacles()
//         {
//             var navigationObstacles = new List<CustomNavigationObstacle>();
//             foreach (var activeWorldMapChunk in _activeWorldMapChunks)
//                 navigationObstacles.AddRange(activeWorldMapChunk.NavigationObstacles);
//             return navigationObstacles;
//         }
//
//         public override List<string> GetActiveMapAreaIds()
//         {
//             var activeMapAreaIds = new List<string>();
//             foreach (var activeWorldMapChunk in _activeWorldMapChunks)
//                 activeMapAreaIds.Add(activeWorldMapChunk.Id);
//             return activeMapAreaIds;
//         }
//
//         public override bool CheckEnemyCanMoveToPosition(Vector2 moveToPosition)
//             => !_baseNoTrespassArea.IsPositionInArea(moveToPosition);
//
//         public override Vector2 GetRestRandomHeroMoveToPosition(Vector3 startPosition)
//         {
//             var restRandomHeroMoveToPositionIndex = UnityRandom.Range(0, _restRandomHeroMoveToPositions.Length);
//             return _restRandomHeroMoveToPositions[restRandomHeroMoveToPositionIndex];
//         }
//
//         public void HandleHeroesGoToRestStation(ChangeRestStateMessage gateEventTriggeredMessage)
//             => LoadRestStationData(gateEventTriggeredMessage.RestId);
//
//         public void HandleBaseMainBuildingFeatureUnlocked()
//             => LoadRestStationData(Constant.BASE_REST_STATION_ID);
//
//         public void HandlePortalButtonTriggered(CancellationToken cancellationToken)
//         {
//             var heroesGroupCenterPosition = GetHeroesGroupCenterPosition();
//             var outbasePortalGatePositin = heroesGroupCenterPosition + Vector2.up * PortalConstant.OUT_BASE_PORTAL_GATE_HEIGHT_OFFSET;
//             SpawnOutAndInBasePortalGatesAsync(outbasePortalGatePositin, cancellationToken).Forget();
//         }
//
//         public void HandleHeroTeamPickUpdated(CancellationToken cancellationToken)
//             => HandleHeroTeamPickUpdatedAsync(cancellationToken).Forget();
//
//         public void HandleSingleHeroSpawned(SingleHeroSpawnedMessage singleHeroSpawnMessage, CancellationToken cancellationToken)
//             => HandleSingleHeroSpawnedAsync(singleHeroSpawnMessage.HeroId, singleHeroSpawnMessage.Position, cancellationToken, singleHeroSpawnMessage.Callback).Forget();
//
//         public void HandleOutBasePortalGateTriggered(CancellationToken cancellationToken)
//             => RunTeleportToInBaseAsync(cancellationToken).Forget();
//
//         public void HandleOutBasePortalGateInMinimapTriggered(CancellationToken cancellationToken)
//         {
//             var isInRestStation = HeroesControlManager.Instance.IsInRestStation;
//             if (isInRestStation)
//                 HandleInBasePortalGateTriggered(cancellationToken);
//             else
//                 RunTeleportToPosition(_outBasePortalGameObject.transform.position);
//         }
//
//         public void HandleInBasePortalGateTriggered(CancellationToken cancellationToken)
//             => RunTeleportToOutBaseAsync(cancellationToken).Forget();
//
//         public void HandleInBasePortalGateInMinimapTriggered(CancellationToken cancellationToken)
//         {
//             var isInRestStation = HeroesControlManager.Instance.IsInRestStation;
//             if (isInRestStation)
//                 RunTeleportToPosition(_inBasePortalGameObject.transform.position);
//             else
//                 HandleOutBasePortalGateTriggered(cancellationToken);
//         }
//
//         public void HandleTeleportationSelected(string chunkId, string portalId, CancellationToken cancellationToken)
//             => RunTeleportToPortalAsync(chunkId, portalId, cancellationToken).Forget();
//
//         public void RunTeleportToPosition(Vector2 teleportPosition)
//             => RunTeleportToPositionAsync(teleportPosition, this.GetCancellationTokenOnDestroy()).Forget();
//
//         public void HandleNewDayReset(CancellationToken cancellationToken)
//             => SpawnRandomChest(cancellationToken);
//
//         public void HandleReviveMapTriggered(CancellationToken cancellationToken)
//             => SpawnRevivedHeroesAsync(cancellationToken).Forget();
//
//         public void HandleGiveUpMapTriggered(CancellationToken cancellationToken)
//         {
//             Func<UniTask> loadFakeSceneTask = async () => await SpawnInitialHeroesAsync(cancellationToken);
//             SceneManager.LoadFakeSceneAsync(loadFakeSceneTask).Forget();
//         }
//
//         public List<TeleportGateData> GetTeleportGatesData()
//         {
//             var worldTeleportGatesData = new List<TeleportGateData>();
//             foreach (var worldMapChunkScenePart in _worldMapChunkSceneParts)
//             {
//                 var chunkTeleportGatesData = worldMapChunkScenePart.MapArea.GetTeleportGatesData();
//                 if (chunkTeleportGatesData != null)
//                     worldTeleportGatesData.AddRange(chunkTeleportGatesData);
//             }
//             return worldTeleportGatesData;
//         }
//
//         public List<DungeonGateData> GetDungeonGatesData()
//         {
//             var worldDungeonGatesData = new List<DungeonGateData>();
//             foreach (var worldMapChunkScenePart in _worldMapChunkSceneParts)
//             {
//                 var chunkDungeonGatesData = worldMapChunkScenePart.MapArea.GetDungeonGatesData();
//                 if (chunkDungeonGatesData != null)
//                     worldDungeonGatesData.AddRange(chunkDungeonGatesData);
//             }
//             return worldDungeonGatesData;
//         }
//
//         public override Vector2 GetPositionForQuestToTarget(string entityId, string chunk, string zone)
//         {
//             var listEnemy = EntitiesManager.Instance.GetCurrentListEnemyActiveByEnemyId(entityId);
//             Vector2 nearestEnemy = Vector2.zero;
//             float distanceCurrentSqr = float.MaxValue;
//             if (listEnemy.Count > 0)
//             {
//                 if (EntitiesManager.Instance.MainHeroTransform != null)
//                 {
//                     Vector2 mainPos = EntitiesManager.Instance.MainHeroTransform.position;
//                     foreach (var enemy in listEnemy)
//                     {
//                         float distanceSqr = (enemy.Position - mainPos).sqrMagnitude;
//                         if (distanceSqr < distanceCurrentSqr)
//                         {
//                             nearestEnemy = enemy.Position;
//                             distanceCurrentSqr = distanceSqr;
//                         }
//                     }
//                     return nearestEnemy;
//                 }
//                 else return Vector2.zero;
//             }
//
//             if (!string.IsNullOrEmpty(chunk))
//             {
//                 var chunkWorldMap = GetWorldMapChunk(chunk);
//                 if (chunkWorldMap != null)
//                 {
//                     return chunkWorldMap.GetEntityZonePosition(zone);
//                 }
//             }
//
//             return Vector2.zero;
//         }
//
//         public override Vector2 GetFogGatePosition(string chunkId, string fogId)
//         {
//             var chunk = GetWorldMapChunk(chunkId);
//             var fogGatePosition = chunk.GetFogGatePosition(fogId);
//             return fogGatePosition;
//         }
//
//         public override Vector2 GetSaveHeroPosition(string chunkId, string saveHeroGateId)
//         {
//             var chunk = GetWorldMapChunk(chunkId);
//             var saveHeroPosition = chunk.GetSaveHeroGatePosition(saveHeroGateId);
//             return saveHeroPosition;
//         }
//
//         public override Vector2 GetBuildingPosition(string chunkId, string buildingId)
//         {
//             var chunk = GetWorldMapChunk(chunkId);
//             var buildingPosition = chunk.GetBuildingGatePosition(buildingId);
//             return buildingPosition;
//         }
//
//         public override Vector2 GetTeleportPortalPosition(string chunkId, string teleportId)
//         {
//             var chunk = GetWorldMapChunk(chunkId);
//             var teleportGatePosition = chunk.GetTeleportGatePosition(teleportId);
//             return teleportGatePosition;
//         }
//
//         protected override async UniTask SpawnInitialHeroesAsync(CancellationToken cancellationToken)
//         {
//             List<Vector2> spawnPoints = null;
//             var movementStrategyType = MovementStrategyType.Spread;
//             var hasGoneToDungeon = DataManager.Transitioned.HasGoneToDungeon;
//             if (hasGoneToDungeon)
//             {
//                 DataManager.Transitioned.HasGoneToDungeon = false;
//                 var isOnDungeonScrollMode = DataManager.Transitioned.IsOnDungeonScrollMode;
//                 movementStrategyType = isOnDungeonScrollMode ? MovementStrategyType.Spread : MovementStrategyType.Follow;
//                 spawnPoints = DataManager.Transitioned.LastStandDungeonPositions;
//                 HeroesControlManager.Instance.SetInitialInRestStationStatus(isOnDungeonScrollMode);
//             }
//             else
//             {
//                 var hasUnlockedBaseMainBuilding = DataManager.Local.HasUnlockedBaseMainBuilding;
//                 movementStrategyType = hasUnlockedBaseMainBuilding ? MovementStrategyType.Spread : MovementStrategyType.Follow;
//                 spawnPoints = hasUnlockedBaseMainBuilding ? _baseRandomHeroSpawnPositions : _beachRandomHeroSpawnPositions;
//                 HeroesControlManager.Instance.SetInitialInRestStationStatus(hasUnlockedBaseMainBuilding);
//             }
//
//             var heroFragments = DataManager.Local.HeroFragments;
//             var randomSpawnPointIndex = UnityRandom.Range(0, spawnPoints.Count);
//             heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                    heroFragmentData.level,
//                                                                    movementStrategyType,
//                                                                    spawnPoints[++randomSpawnPointIndex % spawnPoints.Count],
//                                                                    cancellationToken);
//                 }
//             }
//             await base.SpawnInitialHeroesAsync(cancellationToken);
//         }
//
//         private WorldMapChunk GetWorldMapChunk(string worldMapChunkId)
//         {
//             foreach (var worldMapChunkScenePart in _worldMapChunkSceneParts)
//             {
//                 if (string.Equals(worldMapChunkScenePart.MapAreaId, worldMapChunkId))
//                     return worldMapChunkScenePart.MapArea;
//             }
// #if UNITY_EDITOR
//             Debug.LogError("Get world map chunk with id " + worldMapChunkId + " is NULL");
// #endif
//             return null;
//         }
//
//         private WorldMapChunkScenePart GetWorlMapChunkScenePart(string worldMapChunkId)
//         {
//             foreach (var worldMapChunkScenePart in _worldMapChunkSceneParts)
//             {
//                 if (string.Equals(worldMapChunkScenePart.MapAreaId, worldMapChunkId))
//                     return worldMapChunkScenePart;
//             }
// #if UNITY_EDITOR
//             Debug.LogError("Get world map chunk scene part with id " + worldMapChunkId + " is NULL");
// #endif
//             return null;
//         }
//
//         private async UniTask SpawnInitialStuffAsync(CancellationToken cancellationToken)
//         {
//             await SpawnInitialHeroesAsync(cancellationToken);
//             await SpawnPortalGatesAsync(cancellationToken);
//             SpawnRandomChest(cancellationToken);
//         }
//
//         private void SpawnBossChest(Vector2 chestSpawnPoint, CancellationToken cancellationToken)
//         {
//             var canSpawnChest = DataManager.Local.CanSpawnChest;
//             if (canSpawnChest)
//             {
//                 RemovePresentBossChest();
//                 SpawnBossChestAsync(chestSpawnPoint, cancellationToken).Forget();
//             }
//         }
//
//         private void SpawnRandomChest(CancellationToken cancellationToken)
//         {
//             if (_countdownRespawnRandomChestTickId != 0)
//                 TickManager.Instance.RemoveUnownedTimer(_countdownRespawnRandomChestTickId);
//
//             if (_presentRandomChestGameObject == null)
//                 CountdownSpawnRandomChest(cancellationToken);
//         }
//
//         private async UniTask SpawnPortalGatesAsync(CancellationToken cancellationToken)
//         {
//             var hasOutBasePortalGate = DataManager.Local.HasOutBasePortalGate;
//             if (hasOutBasePortalGate)
//             {
//                 var savedOutBasePortalGatePosition = DataManager.Local.SavedOutBasePortalGatePosition;
//                 await SpawnOutAndInBasePortalGatesAsync(savedOutBasePortalGatePosition, cancellationToken);
//             }
//         }
//
//         private async UniTask SpawnRevivedHeroesAsync(CancellationToken cancellationToken)
//         {
//             if (!_isSpawningReviveHeroes && EntitiesManager.Instance.NumberOfHeroes == 0)
//             {
//                 _isSpawningReviveHeroes = true;
//                 var movementStrategyType = MovementStrategyType.Follow;
//                 var heroFragments = DataManager.Local.HeroFragments;
//                 var spawnCenterPoint = DataManager.Transitioned.LastHeroStandPosition;
//                 var spawnPoints = TransformUtility.GetPositionsAroundPoint(spawnCenterPoint);
//                 var randomSpawnPointIndex = UnityRandom.Range(0, spawnPoints.Count);
//                 heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//                 foreach (var heroFragmentData in heroFragments)
//                 {
//                     if (heroFragmentData.isSelected)
//                     {
//                         await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                        heroFragmentData.level,
//                                                                        movementStrategyType,
//                                                                        spawnPoints[++randomSpawnPointIndex % spawnPoints.Count],
//                                                                        cancellationToken);
//                     }
//                 }
//                 _isSpawningReviveHeroes = false;
//             }
//         }
//
//         private async UniTask RunTeleportToInBaseAsync(CancellationToken cancellationToken)
//         {
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_OUT, cancellationToken).Forget();
//
//             LoadRestStationData(Constant.BASE_REST_STATION_ID);
//
//             // Lock camera view.
//             GameplayVisualController.Instance.LockViewRegion();
//
//             // Wait for the camera to zoom out from the out-base portal gate.
//             await GameplayVisualController.Instance.ZoomOutCameraAsync(cancellationToken);
//
//             // Create portal hero disappear effects in all remaining heroes.
//             foreach (var heroModelTransform in EntitiesManager.Instance.HeroModelTransforms)
//                 await EntitiesManager.Instance.CreatePortalHeroEffectAsync(true, heroModelTransform.Model.Position, cancellationToken);
//
//             // Wait a bit before removing (deleting) all heroes from the screen.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_DISAPPEAR_HEROES_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Handle characters before disppearing heroes.
//             TeleportHandleCharactersBeforeDisappearHeroes();
//
//             // Wait a bit for the portal hero disappear effects to complete.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_TELEPORT_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Wait for the camera to move back to the in-base portal gate.
//             await GameplayVisualController.Instance.TranslateCameraAsync(_inBasePortalGameObject.transform.position, cancellationToken);
//
//             ProximityManager.Instance.AddConditionPositionCheckVisible(_inBasePortalGameObject.transform.position, true);
//
//             // Wait for the camera to zoom in to the in-base portal gate.
//             await GameplayVisualController.Instance.ZoomToOriginalCameraAsync(cancellationToken);
//
//             // Wait a bit before showing the hero appear effects.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_SHOWING_HERO_APPEAR_EFFECTS_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Create portal hero appear effects in all heroes (the dead ones also included).
//             var heroFragments = DataManager.Local.HeroFragments;
//             heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//             var spawnPoints = TransformUtility.GetPositionsAroundPoint(_inBasePortalGameObject.transform.position);
//             var randomSpawnPointIndex = UnityRandom.Range(0, spawnPoints.Count);
//             var tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     var portalHeroAppearEffectPosition = spawnPoints[++tempSpawnPointIndex % spawnPoints.Count];
//                     await EntitiesManager.Instance.CreatePortalHeroEffectAsync(false, portalHeroAppearEffectPosition, cancellationToken);
//                 }
//             }
//
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_IN, cancellationToken).Forget();
//
//             // Wait a bit for the portal hero appear effects play and then respawn all heroes.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_RESPAWNED_IN_BASE_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Spawn all heroes back.
//             tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                    heroFragmentData.level,
//                                                                    MovementStrategyType.Spread,
//                                                                    spawnPoints[++tempSpawnPointIndex % spawnPoints.Count],
//                                                                    cancellationToken);
//                 }
//             }
//             // Unlock camera view.
//             GameplayVisualController.Instance.UnlockViewRegion(false);
//         }
//
//         private async UniTask RunTeleportToOutBaseAsync(CancellationToken cancellationToken)
//         {
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_OUT, cancellationToken).Forget();
//
//             // Lock camera view.
//             GameplayVisualController.Instance.LockViewRegion();
//
//             // Clear the saved out base portal gate position.
//             DataManager.Local.ClearOutBasePortalGatePosition();
//
//             // Wait for the camera to zoom out from the in-base portal gate.
//             await GameplayVisualController.Instance.ZoomOutCameraAsync(cancellationToken);
//
//             // Create portal hero disappear effects in all heroes.
//             foreach (var heroModelTransform in EntitiesManager.Instance.HeroModelTransforms)
//                 await EntitiesManager.Instance.CreatePortalHeroEffectAsync(true, heroModelTransform.Model.Position, cancellationToken);
//
//             // Wait a bit before removing (deleting) all heroes from the screen.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_DISAPPEAR_HEROES_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Handle characters before disppearing heroes.
//             TeleportHandleCharactersBeforeDisappearHeroes();
//
//             // Wait a bit for the portal hero disappear effects to complete.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_TELEPORT_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Remove the out base portal gate game object.
//             if (_outBasePortalGameObject != null)
//             {
//                 PoolManager.Instance.Remove(_outBasePortalGameObject);
//                 _outBasePortalGameObject = null;
//             }
//
//             // Remove the in base portal gate game object.
//             PoolManager.Instance.Remove(_inBasePortalGameObject);
//             _inBasePortalGameObject = null;
//
//             ProximityManager.Instance.AddConditionPositionCheckVisible(_lastOutBasePortalGatePosition, true);
//             // Wait for the camera to move back to the last out-base portal gate's position.
//             await GameplayVisualController.Instance.TranslateCameraAsync(_lastOutBasePortalGatePosition, cancellationToken);
//
//             // Wait for the camera to zoom in to the last out-base portal gate's position.
//             await GameplayVisualController.Instance.ZoomToOriginalCameraAsync(cancellationToken);
//
//             // Wait a bit before showing the hero appear effects.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_SHOWING_HERO_APPEAR_EFFECTS_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Create portal hero appear effects in all heroes.
//             var heroFragments = DataManager.Local.HeroFragments;
//             heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//             var spawnPoints = TransformUtility.GetPositionsAroundPoint(_lastOutBasePortalGatePosition);
//             var randomSpawnPointIndex = UnityRandom.Range(0, spawnPoints.Count);
//             var tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     var portalHeroAppearEffectPosition = spawnPoints[++tempSpawnPointIndex % spawnPoints.Count];
//                     await EntitiesManager.Instance.CreatePortalHeroEffectAsync(false, portalHeroAppearEffectPosition, cancellationToken);
//                 }
//             }
//
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_IN, cancellationToken).Forget();
//
//             // Wait a bit for the portal hero appear effects play and then respawn all heroes.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_RESPAWNED_IN_BASE_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Spawn all heroes back.
//             tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                    heroFragmentData.level,
//                                                                    MovementStrategyType.Follow,
//                                                                    spawnPoints[++tempSpawnPointIndex % spawnPoints.Count],
//                                                                    cancellationToken);
//                 }
//             }
//             // Unlock camera view.
//             GameplayVisualController.Instance.UnlockViewRegion(false);
//         }
//
//         private async UniTask RunTeleportToPortalAsync(string chunkId, string teleportPortalId, CancellationToken cancellationToken)
//         {
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_OUT, cancellationToken).Forget();
//
//             var teleportPortalPosition = GetTeleportPortalPosition(chunkId, teleportPortalId);
//
//             // Lock camera view.
//             GameplayVisualController.Instance.LockViewRegion();
//
//             // Wait for the camera to zoom out from the in-base portal gate.
//             await GameplayVisualController.Instance.ZoomOutCameraAsync(cancellationToken);
//
//             // Create portal hero disappear effects in all heroes.
//             foreach (var heroModelTransform in EntitiesManager.Instance.HeroModelTransforms)
//                 await EntitiesManager.Instance.CreatePortalHeroEffectAsync(true, heroModelTransform.Model.Position, cancellationToken);
//
//             // Wait a bit before removing (deleting) all heroes from the screen.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_DISAPPEAR_HEROES_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Handle characters before disppearing heroes.
//             TeleportHandleCharactersBeforeDisappearHeroes();
//
//             // Wait a bit for the portal hero disappear effects to complete.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_TELEPORT_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             ProximityManager.Instance.AddConditionPositionCheckVisible(teleportPortalPosition, true);
//
//             // Wait for the camera to move back to the last out-base portal gate's position.
//             await GameplayVisualController.Instance.TranslateCameraAsync(teleportPortalPosition, cancellationToken);
//
//             // Wait for the camera to zoom in to the last out-base portal gate's position.
//             await GameplayVisualController.Instance.ZoomToOriginalCameraAsync(cancellationToken);
//
//             // Wait a bit before showing the hero appear effects.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_SHOWING_HERO_APPEAR_EFFECTS_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Create portal hero appear effects in all heroes.
//             var heroFragments = DataManager.Local.HeroFragments;
//             heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//             var spawnPoints = TransformUtility.GetPositionsAroundPoint(teleportPortalPosition);
//             var randomSpawnPointIndex = UnityRandom.Range(0, spawnPoints.Count);
//             var tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     var portalHeroAppearEffectPosition = spawnPoints[++tempSpawnPointIndex % spawnPoints.Count];
//                     await EntitiesManager.Instance.CreatePortalHeroEffectAsync(false, portalHeroAppearEffectPosition, cancellationToken);
//                 }
//             }
//
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_IN, cancellationToken).Forget();
//
//             // Wait a bit for the portal hero appear effects play and then respawn all heroes.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_RESPAWNED_IN_BASE_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             var movementStrategyType = HeroesControlManager.Instance.IsInRestStation ? MovementStrategyType.Spread : MovementStrategyType.Follow;
//             tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                    heroFragmentData.level,
//                                                                    movementStrategyType,
//                                                                    spawnPoints[++tempSpawnPointIndex % spawnPoints.Count],
//                                                                    cancellationToken);
//                 }
//             }
//
//             // Unlock camera view.
//             GameplayVisualController.Instance.UnlockViewRegion(false);
//         }
//
//         private async UniTask RunTeleportToPositionAsync(Vector2 teleportPosition, CancellationToken cancellationToken)
//         {
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_OUT, cancellationToken).Forget();
//
//             // Lock camera view.
//             GameplayVisualController.Instance.LockViewRegion();
//
//             // Wait for the camera to zoom out from the in-base portal gate.
//             await GameplayVisualController.Instance.ZoomOutCameraAsync(cancellationToken);
//
//             // Create portal hero disappear effects in all heroes.
//             foreach (var heroModelTransform in EntitiesManager.Instance.HeroModelTransforms)
//                 await EntitiesManager.Instance.CreatePortalHeroEffectAsync(true, heroModelTransform.Model.Position, cancellationToken);
//
//             // Wait a bit before removing (deleting) all heroes from the screen.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_DISAPPEAR_HEROES_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Handle characters before disppearing heroes.
//             TeleportHandleCharactersBeforeDisappearHeroes();
//
//             // Wait a bit for the portal hero disappear effects to complete.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_TELEPORT_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Wait for the camera to move back to the teleport position.
//             await GameplayVisualController.Instance.TranslateCameraAsync(teleportPosition, cancellationToken);
//
//             // Wait for the camera to zoom in to the last out-base portal gate's position.
//             await GameplayVisualController.Instance.ZoomToOriginalCameraAsync(cancellationToken);
//
//             // Wait a bit before showing the hero appear effects.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_SHOWING_HERO_APPEAR_EFFECTS_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Create portal hero appear effects in all heroes.
//             var heroFragments = DataManager.Local.HeroFragments;
//             heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//             var spawnPoints = TransformUtility.GetPositionsAroundPoint(teleportPosition);
//             var randomSpawnPointIndex = UnityRandom.Range(0, spawnPoints.Count);
//             var tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     var portalHeroAppearEffectPosition = spawnPoints[++tempSpawnPointIndex % spawnPoints.Count];
//                     await EntitiesManager.Instance.CreatePortalHeroEffectAsync(false, portalHeroAppearEffectPosition, cancellationToken);
//                 }
//             }
//
//             // Play sound effect.
//             AudioController.Instance.PlaySoundEffectAsync(AudioConstants.GENERAL_TELEPORT_IN, cancellationToken).Forget();
//
//             // Wait a bit for the portal hero appear effects play and then respawn all heroes.
//             await UniTask.Delay(PortalConstant.DELAY_BEFORE_HEROES_RESPAWNED_IN_BASE_IN_MILLISECONDS, ignoreTimeScale: true, cancellationToken: cancellationToken);
//
//             // Spawn all heroes back.
//             var movementStrategyType = HeroesControlManager.Instance.IsInRestStation ? MovementStrategyType.Spread : MovementStrategyType.Follow;
//             tempSpawnPointIndex = randomSpawnPointIndex;
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                    heroFragmentData.level,
//                                                                    movementStrategyType,
//                                                                    spawnPoints[++tempSpawnPointIndex % spawnPoints.Count],
//                                                                    cancellationToken);
//                 }
//             }
//             // Unlock camera view.
//             GameplayVisualController.Instance.UnlockViewRegion(false);
//         }
//
//         private async UniTask HandleHeroTeamPickUpdatedAsync(CancellationToken cancellationToken)
//         {
//             var heroFragments = DataManager.Local.HeroFragments;
//             heroFragments = heroFragments.OrderByDescending(x => x.isLeader).ToList();
//             var oldMainHeroModel = EntitiesManager.Instance.MainHeroModel;
//             var oldMainHeroPosition = oldMainHeroModel.Position;
//
//             // Remove all heroes.
//             RemoveAllEquippedHeroes();
//
//             foreach (var heroFragmentData in heroFragments)
//             {
//                 if (heroFragmentData.isSelected)
//                 {
//                     var heroRestSpawnPosition = GetRestRandomHeroMoveToPosition(oldMainHeroPosition);
//                     var spawnPosition = heroFragmentData.isLeader ? oldMainHeroPosition : heroRestSpawnPosition;
//                     await EntitiesManager.Instance.CreateHeroAsync(heroFragmentData.id,
//                                                                    heroFragmentData.level,
//                                                                    MovementStrategyType.Spread,
//                                                                    spawnPosition,
//                                                                    cancellationToken);
//                 }
//             }
//         }
//
//         private async UniTask HandleSingleHeroSpawnedAsync(string heroId, Vector2 positionSpawn, CancellationToken cancellationToken, Action callback = null)
//         {
//             var hasHero = EntitiesManager.Instance.HeroModelTransforms.Any(x => x.Model.EntityId == heroId);
//             if (hasHero)
//                 return;
//
//             var movementStrategyType = EntitiesManager.Instance.MainHeroModel.MovementStrategyType;
//             var heroFragment = DataManager.Local.HeroFragments.FirstOrDefault(x => x.id == heroId);
//             var moveToPoints = TransformUtility.GetPositionsAroundPoint(EntitiesManager.Instance.MainHeroTransform.position);
//             var randomMoveToPointIndex = UnityRandom.Range(0, moveToPoints.Count);
//
//             if (positionSpawn == Vector2.zero)
//             {
//                 await EntitiesManager.Instance.CreateHeroAsync(heroId,
//                                                                heroFragment.level,
//                                                                movementStrategyType,
//                                                                moveToPoints[randomMoveToPointIndex],
//                                                                cancellationToken);
//             }
//             else
//             {
//                 await EntitiesManager.Instance.CreateHeroSaveAsync(heroId,
//                                                                    heroFragment.level,
//                                                                    movementStrategyType,
//                                                                    positionSpawn,
//                                                                    moveToPoints[randomMoveToPointIndex],
//                                                                    cancellationToken,
//                                                                    callback);
//             }
//         }
//
//         private void CountdownSpawnRandomChest(CancellationToken cancellationToken, bool isNotHaveSpawnPointOrNotHaveAds = false)
//         {
//             var canSpawnChest = DataManager.Local.CanSpawnChest;
//             if (canSpawnChest)
//             {
//                 var worldChestItemInfo = DataManager.Config.GetWorldChestItemInfo(ChestType.Random);
//                 Action tickCompletedCallbackAction = () => SpawnRandomChestAsync(cancellationToken).Forget();
//                 var randomChestRespawnDelay = worldChestItemInfo.respawnDelay;
//                 if (isNotHaveSpawnPointOrNotHaveAds)
//                 {
//                     randomChestRespawnDelay = Constant.DELAY_RECHECK_RANDOM_CHEST;
//                 }
//                 _countdownRespawnRandomChestTickId = TickManager.Instance.AddUnownedTimer(randomChestRespawnDelay, tickCompletedCallbackAction);
//             }
//         }
//
//         private async UniTask SpawnRandomChestAsync(CancellationToken cancellationToken)
//         {
//             _countdownRespawnRandomChestTickId = 0;
//             var worldChestItemInfo = DataManager.Config.GetWorldChestItemInfo(ChestType.Random);
//             var closestChestPoint = ClosestChestPoint;
//             if (closestChestPoint != Vector2.zero && AdsManager.Instance.IsRewardAdsAvailable())
//             {
//                 var randomChestId = worldChestItemInfo.chestId;
//                 var randomChestGameObject = await EntitiesManager.Instance.CreateChestAsync(randomChestId, closestChestPoint, cancellationToken);
//                 _presentRandomChestGameObject = randomChestGameObject;
//                 _presentRandomChestGameObject.GetComponent<ChestGate>().Init(worldChestItemInfo.ChestResourceData, ChestType.Random, () => RemovePresentRandomChest(cancellationToken));
//                 _spawnRandomChestTickId = TickManager.Instance.AddUnownedTimer(worldChestItemInfo.existTime, () => RemovePresentRandomChest(cancellationToken, true));
//                 Messenger.Publish(new ObjectWarningUpdatedMessage(ObjectWarningType.Chest, _presentRandomChestGameObject.transform, null, true));
//             }
//             else CountdownSpawnRandomChest(cancellationToken, true);
//         }
//
//         private async UniTask SpawnBossChestAsync(Vector2 chestSpawnPoint, CancellationToken cancellationToken)
//         {
//             var worldChestItemInfo = DataManager.Config.GetWorldChestItemInfo(ChestType.Boss);
//             var bossChestGameObject = await EntitiesManager.Instance.CreateChestAsync(worldChestItemInfo.chestId, chestSpawnPoint, cancellationToken);
//             _presentBossChestGameObject = bossChestGameObject;
//             _presentBossChestGameObject.GetComponent<ChestGate>().Init(worldChestItemInfo.ChestResourceData, ChestType.Boss, RemovePresentBossChest);
//             _spawnBossChestTickId = TickManager.Instance.AddUnownedTimer(worldChestItemInfo.existTime, RemovePresentBossChest);
//         }
//
//         private void RemovePresentRandomChest(CancellationToken cancellationToken, bool isPopUser = false)
//         {
//             if (_presentRandomChestGameObject != null)
//             {
//                 Messenger.Publish(new ObjectWarningUpdatedMessage(ObjectWarningType.Chest, _presentRandomChestGameObject.transform, null, false));
//                 TickManager.Instance.RemoveUnownedTimer(_spawnRandomChestTickId);
//                 EntitiesManager.Instance.RemoveGameObject(_presentRandomChestGameObject);
//                 if (isPopUser)
//                 {
//                     if (DataManager.Local.HasApplySkipAds)
//                     {
//                         AutoClaimTreasureChestBySkipAds();
//                     }
//                     else if (AdsManager.Instance.IsRewardAdsAvailable())
//                     {
//                         if (!DataManager.Transitioned.IsOnCamping)
//                             ShowMissedTreasureChest(cancellationToken);
//                     }
//                 }
//                 _presentRandomChestGameObject = null;
//             }
//
//             CountdownSpawnRandomChest(cancellationToken);
//         }
//
//         private void AutoClaimTreasureChestBySkipAds()
//         {
//             var worldChestItemInfo = DataManager.Config.GetWorldChestItemInfo(ChestType.Random);
//             var resourceRandom = worldChestItemInfo.ChestResourceData;
//             var ratioScaleByLevel = worldChestItemInfo.GetRatioScaleReturnResource(DataManager.Local.PlayerLevel);
//             var resourceTypeIgnoreScaleLevel = worldChestItemInfo.resourceTypeIgnoreScaleLevel;
//             int valueScaleAds = worldChestItemInfo.scaleAds;
//             if (resourceRandom.resourceType == resourceTypeIgnoreScaleLevel)
//             {
//                 ratioScaleByLevel = 1;
//             }
//             var finalResourceData = new ResourceData(resourceRandom.resourceType,
//                 resourceRandom.resourceId,
//                 (long)(resourceRandom.resourceNumber * ratioScaleByLevel * valueScaleAds));
//             DataManager.Local.AddResource(
//                 finalResourceData.resourceType,
//                 ResourceEarnSourceType.TreasureChest,
//                 finalResourceData.resourceId,
//                 finalResourceData.resourceNumber,
//                 true,
//                 earnSourceId: ResourceEarnSourceType.TreasureChest.ToTrackingFormat()
//             );
//             ToastManager.Instance.ShowToastWithVisualResource(finalResourceData, Vector3.zero);
//             ToastManager.Instance.Show(LocalizationUtils.GetToastLocalized(LocalizeKeys.CLAIM_MISSING_TREASURE_SUCCESS));
//             DataManager.Local.SubtractValueCurrentRemaining();
//         }
//
//         private void ShowMissedTreasureChest(CancellationToken cancellationToken)
//         {
//             var worldChestItemInfo = DataManager.Config.GetWorldChestItemInfo(ChestType.Random);
//             var resourceRandom = worldChestItemInfo.ChestResourceData;
//             var windowOptions = new WindowOptions(ModalId.TREASURE_CHEST, true);
//             var treasureChestModalData = new TreasureChestModalData(resourceRandom,
//                 worldChestItemInfo.chestType, isMissing: true);
//             ScreenNavigator.Instance.LoadModal(windowOptions, treasureChestModalData).Forget();
//         }
//
//         private void RemovePresentBossChest()
//         {
//             if (_presentBossChestGameObject != null)
//             {
//                 TickManager.Instance.RemoveUnownedTimer(_spawnBossChestTickId);
//                 EntitiesManager.Instance.RemoveGameObject(_presentBossChestGameObject);
//                 _presentBossChestGameObject = null;
//             }
//         }
//
//         private async UniTask SpawnOutAndInBasePortalGatesAsync(Vector2 outBasePortalGatePosition, CancellationToken cancellationToken)
//         {
//             if (_outBasePortalGameObject != null)
//                 PoolManager.Instance.Remove(_outBasePortalGameObject);
//
//             var outbasePortalGatePosition = outBasePortalGatePosition;
//             _lastOutBasePortalGatePosition = outbasePortalGatePosition;
//             _outBasePortalGameObject = await EntitiesManager.Instance.CreatePortalGateAsync(true, outbasePortalGatePosition, cancellationToken);
//             DataManager.Local.SaveOutBasePortalGatePosition(outbasePortalGatePosition);
//
//             if (_inBasePortalGameObject == null)
//                 _inBasePortalGameObject = await EntitiesManager.Instance.CreatePortalGateAsync(false, _basePortalGatePosition, cancellationToken);
//         }
//
//         private bool IsChestPointOffScreen(Vector2 chestPoint)
//         {
//             var chestScreenPosition = Camera.main.WorldToViewportPoint(chestPoint);
//             return chestScreenPosition.x <= 0 || chestScreenPosition.x >= 1 || chestScreenPosition.y <= 0 || chestScreenPosition.y >= 1;
//         }
//
//         private void LoadRestStationData(string activeRestStationId)
//         {
//             foreach (var activeWorldMapChunk in _activeWorldMapChunks)
//             {
//                 if (activeWorldMapChunk.RestStation != null && activeWorldMapChunk.RestStation.RestStationId == activeRestStationId)
//                 {
//                     _restRandomHeroMoveToPositions = activeWorldMapChunk.RestStation.RandomHeroMoveToPositions;
//                     _baseNoTrespassArea = activeWorldMapChunk.RestStation.NoTrespassArea;
//                     break;
//                 }
//             }
//         }
//
//         private Vector2 GetHeroesGroupCenterPosition()
//         {
//             var heroModelsTransforms = EntitiesManager.Instance.HeroModelTransforms;
//             var groupCenterPosition = Vector2.zero;
//             foreach (var heroModelTransform in heroModelsTransforms)
//                 groupCenterPosition += heroModelTransform.Model.Position;
//             groupCenterPosition /= heroModelsTransforms.Count;
//             return groupCenterPosition;
//         }
//
//         private void TeleportHandleCharactersBeforeDisappearHeroes()
//         {
//             EntitiesManager.CurrentBattleIndex += 1;
//             RemoveAllEquippedHeroes();
//             foreach (var activeEnemyModel in EntitiesManager.Instance.GetListEnemyActive())
//             {
//                 activeEnemyModel.ReactionChangedEvent.Invoke(CharacterReactionType.JustSawHeroTeleported);
//                 activeEnemyModel.RestoreHp();
//             }
//         }
//
//         private void RemoveAllEquippedHeroes()
//             => EntitiesManager.Instance.RemoveAllHeroes();

        #endregion Class Methods

        public override List<string> GetActiveMapAreaIds()
        {
            throw new NotImplementedException();
        }
    }
}