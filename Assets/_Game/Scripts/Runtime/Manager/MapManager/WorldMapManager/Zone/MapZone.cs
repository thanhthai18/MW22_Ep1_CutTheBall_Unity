using System.Collections.Generic;
using UnityEngine;
using Runtime.Extensions;
using Runtime.Manager.Data;
using Runtime.Config;
using Runtime.Definition;
using UnityRandom = UnityEngine.Random;

namespace Runtime.Gameplay.Map
{
    public enum ConfinedAreaType
    {
        Grass = 1,
        Tree = 2,
        Enemy = 3,
        Crystal = 4,
        Boss = 5,
    }

    public struct SpawnedMapZoneInfo
    {
        #region Members

        public string zoneFullId;
        public string zoneId;
        public bool isTriggerImmortalZone;
        public bool isImmortalZone;
        public string spawnedEntityId;
        public int spawnedEntityLevel;
        public string spawnedChunkId;
        public List<Vector3> randomSpawnedPoints;
        public int zoneLevel;
        public bool isElite;
        public float dropEquipmentRate;
        public bool markRespawnable;

        #endregion Members

        #region Struct Methods

        public SpawnedMapZoneInfo(string zoneFullId, string zoneId, bool isTriggerImmortalZone, bool isImmortalZone,
                                  string spawnedEntityId, int spawnedEntityLevel, string spawnedChunkId, List<Vector3> randomSpawnedPoints,
                                  int zoneLevel, bool isElite, float dropEquipmentRate, bool markRespawnable)
        {
            this.zoneFullId = zoneFullId;
            this.zoneId = zoneId;
            this.isTriggerImmortalZone = isTriggerImmortalZone;
            this.isImmortalZone = isImmortalZone;
            this.spawnedEntityId = spawnedEntityId;
            this.spawnedEntityLevel = spawnedEntityLevel;
            this.randomSpawnedPoints = randomSpawnedPoints;
            this.spawnedChunkId = spawnedChunkId;
            this.zoneLevel = zoneLevel;
            this.isElite = isElite;
            this.dropEquipmentRate = dropEquipmentRate;
            this.markRespawnable = markRespawnable;
        }

        #endregion Struct Methods
    }

    public class MapZone : MonoBehaviour
    {
        #region Members

        private static readonly float s_minDistanceBetweenObjects = 0.2f;
        public ConfinedAreaType confinedAreaType = ConfinedAreaType.Grass;
        public bool showSections = false;
        public bool alwaysShowAreaPointVisual = false;
        public List<MapZoneSection> sections = new List<MapZoneSection>();
        public bool activeConstrains = true;
        public string zoneId;
        public float distanceBetweenObjects = 1.0f;
        public float objectVisualLength = 0.5f;
        public int minTestNumberSpawnedObjects;
        public int maxTestNumberSpawnedObjects;
        protected string belongedMapAreaId;

        #endregion Members

        #region Properties

        public virtual Color VisualColor
        {
            get
            {
                switch (confinedAreaType)
                {
                    case ConfinedAreaType.Grass:
                        return new Color(14.0f / 256.0f, 234.0f / 256.0f, 141.0f / 256.0f);

                    case ConfinedAreaType.Tree:
                        return new Color(14.0f / 256.0f, 234.0f / 256.0f, 20.0f / 256.0f);

                    case ConfinedAreaType.Enemy:
                        return new Color(234.0f / 256.0f, 104.0f / 256.0f, 14.0f / 256.0f);

                    case ConfinedAreaType.Boss:
                        return new Color(255.0f / 256.0f, 41.0f / 256.0f, 13.0f / 256.0f);

                    case ConfinedAreaType.Crystal:
                        return new Color(13.0f / 256.0f, 232.0f / 256.0f, 243.0f / 256.0f);
                }

                return Color.red;
            }
        }

        public virtual float VisualColorAlpha
        {
            get
            {
                return 0.5f;
            }
        }

        public string MapAreaId => belongedMapAreaId;
        public string ZoneFullId => $"{MapAreaId}-{TrimZoneId}";
        public virtual bool MarkRespawnable => true;
        public virtual bool IsTriggerImmortalZone => false;
        public virtual bool IsImmortalZone => false;

        private string TrimZoneId => zoneId.Trim();

        #endregion Properties

        #region API Methods

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            for (int i = 0; i < sections.Count; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(sections[i].point, 0.1f);
                Gizmos.DrawSphere(sections[i].limitUp, 0.1f);
                Gizmos.DrawSphere(sections[i].limitDown, 0.1f);
                Gizmos.color = Color.black;
                UnityEditor.Handles.color = Color.black;

                if (i + 1 < sections.Count)
                {
                    var pointsA = new List<Vector3>();
                    var pointsB = new List<Vector3>();
                    var pointsC = new List<Vector3>();
                    var pointsD = new List<Vector3>();

                    pointsA.Add(new Vector3(sections[i].limitDown.x, sections[i].limitDown.y));
                    pointsA.Add(new Vector3(sections[i].point.x, sections[i].point.y));
                    pointsA.Add(new Vector3(sections[i + 1].limitDown.x, sections[i + 1].limitDown.y));

                    pointsB.Add(new Vector3(sections[i + 1].point.x, sections[i + 1].point.y));
                    pointsB.Add(new Vector3(sections[i].point.x, sections[i].point.y));
                    pointsB.Add(new Vector3(sections[i].limitUp.x, sections[i].limitUp.y));

                    pointsC.Add(new Vector3(sections[i].point.x, sections[i].point.y));
                    pointsC.Add(new Vector3(sections[i + 1].point.x, sections[i + 1].point.y));
                    pointsC.Add(new Vector3(sections[i + 1].limitDown.x, sections[i + 1].limitDown.y));

                    pointsD.Add(new Vector3(sections[i + 1].point.x, sections[i + 1].point.y));
                    pointsD.Add(new Vector3(sections[i + 1].limitUp.x, sections[i + 1].limitUp.y));
                    pointsD.Add(new Vector3(sections[i].limitUp.x, sections[i].limitUp.y));

                    var color = VisualColor;
                    color.a = VisualColorAlpha;
                    UnityEditor.Handles.color = color;

                    UnityEditor.Handles.DrawAAConvexPolygon(pointsA.ToArray());
                    UnityEditor.Handles.DrawAAConvexPolygon(pointsB.ToArray());
                    UnityEditor.Handles.DrawAAConvexPolygon(pointsC.ToArray());
                    UnityEditor.Handles.DrawAAConvexPolygon(pointsD.ToArray());
                }

                Gizmos.DrawLine(sections[i].point, sections[i].limitDown);
                Gizmos.DrawLine(sections[i].point, sections[i].limitUp);
            }

            if (alwaysShowAreaPointVisual)
            {
                var areaRandomPoints = GetTestAreaSpawnedObjectPoints();
                Gizmos.color = Color.white;
                foreach (var areaRandomPoint in areaRandomPoints)
                    Gizmos.DrawWireCube(areaRandomPoint, Vector2.one * objectVisualLength);
            }
        }

        public GUIStyle GetStyle(Color color, TextAnchor align = TextAnchor.MiddleCenter, int size = 11, FontStyle st = FontStyle.Normal)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;
            style.alignment = align;
            style.fontSize = size;
            style.fontStyle = st;
            style.richText = true;
            return style;
        }
#endif

        #endregion API Methods

        #region Class Methods

        public void SetMapAreaId(string belongedMapAreaId)
            => this.belongedMapAreaId = belongedMapAreaId;

        public void ClearPath() => sections.Clear();

        // public SpawnedMapZoneInfo[] GetWorldSpawnedZonesInfo()
        // {
        //     var worldZoneItemInfo = DataManager.Config.GetWorldZoneItemInfo(MapAreaId, TrimZoneId);
        //     return GetSpawnedZonesInfo(worldZoneItemInfo);
        // }
        //
        // public SpawnedMapZoneInfo[] GetDungeonSpawnedZonesInfo(string dungeonId)
        // {
        //     var dungeonZoneItemInfo = DataManager.Config.GetDungeonZoneItemInfo(dungeonId, MapAreaId, TrimZoneId);
        //     return GetSpawnedZonesInfo(dungeonZoneItemInfo);
        // }
        //
        // public SpawnedMapZoneInfo[] GetTutorialSpawnedZonesInfo()
        // {
        //     var tutorialZoneItemInfo = DataManager.Config.GetTutorialZoneItemInfo(MapAreaId, TrimZoneId);
        //     return GetSpawnedZonesInfo(tutorialZoneItemInfo);
        // }
        //
        // public SpawnedMapZoneInfo[] GetGoldenChallengeSpawnedZonesInfo(int goldenChallengeStageIndex)
        // {
        //     var goldenChallengeZoneItemInfo = DataManager.Config.GetGoldenChallengeZoneItemInfo(goldenChallengeStageIndex, TrimZoneId);
        //     return GetSpawnedZonesInfo(goldenChallengeZoneItemInfo);
        // }
        //
        // public SpawnedMapZoneInfo[] GetTowerTrialChallengeSpawnedZonesInfo(TowerTrialChallengeModeType towerTrialChallengeModeType, int towerTrialChallengeFloorIndex)
        // {
        //     var towerTrialChallengeZoneItemInfo = DataManager.Config.GetTowerTrialChallengeZoneItemInfo(towerTrialChallengeModeType, towerTrialChallengeFloorIndex, TrimZoneId);
        //     return GetSpawnedZonesInfo(towerTrialChallengeZoneItemInfo);
        // }

        public List<Vector3> GetTestAreaSpawnedObjectPoints()
        {
            var areaRandomPoints = GetAreaRandomPoints();
            areaRandomPoints.Shuffle();
            var randomNumberSpawnedObjects = UnityRandom.Range(minTestNumberSpawnedObjects, maxTestNumberSpawnedObjects);
            var spawnedTestObjectsAreaPoints = new List<Vector3>();

            if (areaRandomPoints.Count < randomNumberSpawnedObjects)
                randomNumberSpawnedObjects = areaRandomPoints.Count;

            for (int i = 0; i < randomNumberSpawnedObjects; i++)
                spawnedTestObjectsAreaPoints.Add(areaRandomPoints[i]);

            return spawnedTestObjectsAreaPoints;
        }

//         private SpawnedMapZoneInfo[] GetSpawnedZonesInfo(ZoneItem zoneItem)
//         {
//             if (zoneItem.zoneSpawnedEntitiesInfo != null)
//             {
//                 var spawnedMapZonesInfo = new SpawnedMapZoneInfo[zoneItem.zoneSpawnedEntitiesInfo.Length];
//                 for (int i = 0; i < zoneItem.zoneSpawnedEntitiesInfo.Length; i++)
//                 {
//                     var zoneSpawnedEntityInfo = zoneItem.zoneSpawnedEntitiesInfo[i];
//                     var areaRandomPoints = GetAreaRandomPoints();
//                     areaRandomPoints.Shuffle();
//                     var randomNumberSpawnedObjects = UnityRandom.Range(zoneSpawnedEntityInfo.minSpawnedObjects, zoneSpawnedEntityInfo.maxSpawnedObjects);
//                     if (areaRandomPoints.Count < randomNumberSpawnedObjects)
//                         randomNumberSpawnedObjects = areaRandomPoints.Count;
//
//                     var entitySpawnedPoints = new List<Vector3>();
//                     for (int j = 0; j < randomNumberSpawnedObjects; j++)
//                         entitySpawnedPoints.Add(areaRandomPoints[j]);
//
//                     var spawnedMapZoneInfo = new SpawnedMapZoneInfo(ZoneFullId,
//                                                                     this.zoneId,
//                                                                     IsTriggerImmortalZone,
//                                                                     IsImmortalZone,
//                                                                     zoneSpawnedEntityInfo.spawnedEntityId,
//                                                                     zoneSpawnedEntityInfo.spawnedEntityLevel,
//                                                                     MapAreaId, 
//                                                                     entitySpawnedPoints,
//                                                                     zoneItem.zoneLevel,
//                                                                     zoneSpawnedEntityInfo.isElite,
//                                                                     zoneSpawnedEntityInfo.dropEquipmentRate,
//                                                                     MarkRespawnable);
//                     spawnedMapZonesInfo[i] = spawnedMapZoneInfo;
//                 }
//                 return spawnedMapZonesInfo;
//             }
//             else
//             {
// #if UNITY_EDITOR
//                 Debug.LogWarning("Something wrong with map area Id = " + MapAreaId + " with zone id = " + TrimZoneId);
// #endif
//                 return null;
//             }
//         }

        private List<Vector3> GetAreaRandomPoints()
        {
            var areaRandomPoints = new List<Vector3>();
            var areaRectanglePoints = new List<Vector3>();
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var section in sections)
            {
                minX = Mathf.Min(minX, section.GetMinX);
                minY = Mathf.Min(minY, section.GetMinY);
                maxX = Mathf.Max(maxX, section.GetMaxX);
                maxY = Mathf.Max(maxY, section.GetMaxY);
            }

            distanceBetweenObjects = Mathf.Max(s_minDistanceBetweenObjects, distanceBetweenObjects);
            for (float x = minX; x <= maxX; x += distanceBetweenObjects)
            {
                for (float y = minY; y <= maxY; y += distanceBetweenObjects)
                {
                    Vector2 point = new Vector2(x, y);
                    areaRectanglePoints.Add(point);
                }
            }

            foreach (var areaRectanglePoint in areaRectanglePoints)
            {
                var constrainedAreaRectanglePoint = ConstrainsPosition(areaRectanglePoint, areaRectanglePoint);
                if (CanBeAreaPoint(areaRandomPoints, constrainedAreaRectanglePoint))
                    areaRandomPoints.Add(constrainedAreaRectanglePoint);
            }

            return areaRandomPoints;
        }

        private bool CanBeAreaPoint(List<Vector3> areaRandomPoints, Vector3 areaRectanglePoint)
        {
            foreach (var areaPoint in areaRandomPoints)
            {
                if (Vector2.SqrMagnitude(areaPoint - areaRectanglePoint) < distanceBetweenObjects * distanceBetweenObjects)
                    return false;
            }

            return true;
        }

        public void UpdateLimitsUpDownToCenter()
        {
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i].limitUp = new Vector3(sections[i].point.x, sections[i].limitUp.y);
                sections[i].limitDown = new Vector3(sections[i].point.x, sections[i].limitDown.y);
            }
        }

        public void UpdateLimitUpDownToCenter(int i)
        {
            sections[i].limitUp = new Vector3(sections[i].point.x, sections[i].limitUp.y);
            sections[i].limitDown = new Vector3(sections[i].point.x, sections[i].limitDown.y);
        }

        public MapZoneSection GetCurrentPathSection(Transform transform)
        {
            var currentPoint = GetCurrentPoint(transform);
            if (currentPoint >= 0)
                return sections[currentPoint];
            return null;
        }

        public Vector3 GetCenterSection(Transform transform)
        {
            var currentIndexPoint = GetCurrentPoint(transform);
            return Vector3.Lerp(sections[currentIndexPoint].point, sections[currentIndexPoint + 1].point, 0.5f);
        }

        /// <summary>
        /// Get current section index with transform position.
        /// </summary>
        public int GetCurrentPoint(Transform transform)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                if (i <= sections.Count - 2)
                {
                    if (transform.position.x >= sections[i].GetMinX && transform.position.x < sections[i + 1].GetMaxX)
                    {
                        return i;
                    }

                    if (transform.position.x >= sections[i].GetMinX && transform.position.x < sections[i + 1].GetMaxX)
                    {
                        return i;
                    }
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// Get current section index with Vector3 position.
        /// </summary>
        public int GetCurrentPoint(Vector3 position)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                if (i <= sections.Count - 2)
                {
                    if (position.x >= sections[i].GetMinX && position.x < sections[i + 1].GetMaxX)
                    {
                        return i;
                    }

                    if (position.x >= sections[i].GetMinX && position.x < sections[i + 1].GetMaxX)
                    {
                        return i;
                    }
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// Constrains the desired position in the confined area.
        /// </summary>
        /// <param name="desiredPosition">Target position to check.</param>
        /// <param name="transformCheck">This transform is used to get section index.</param>
        /// <returns>A valid position on section.</returns>
        public Vector3 ConstrainsTransform(Vector3 desiredPosition, Transform transformCheck)
        {
            var currentPoint = GetCurrentPoint(transformCheck);
            var resultPoint = GetConstrainsVectorOnPath(desiredPosition, currentPoint, transformCheck);
            return resultPoint;
        }

        /// <summary>
        /// Constrains the desired position in the confined area.
        /// </summary>
        /// <param name="desiredPosition">Target position to check.</param>
        /// <param name="baseCheck">This position is used to get section index.</param>
        /// <returns>A valid position on section.</returns>
        public Vector3 ConstrainsPosition(Vector3 desiredPosition, Vector3 baseCheck)
        {
            var currentPoint = GetCurrentPoint(baseCheck);
            var resultPoint = GetConstrainsVectorOnPath(desiredPosition, currentPoint, baseCheck);
            return resultPoint;
        }

        public Vector3 ConstrainsTransform(Vector3 desiredPosition, Transform transformCheck, float offsetMutipler)
        {
            var currentPoint = GetCurrentPoint(transformCheck);
            if (currentPoint < 0)
                currentPoint = 0;
            return GetConstrainsVectorOnPath(desiredPosition, currentPoint, transformCheck, offsetMutipler);
        }

        private MapZoneObstacleData CheckObstacle(Vector3 point, Transform transformCheck)
        {
            var hits = Physics2D.OverlapCircleAll(point, 0.2f);
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    var obstacle = hit.transform.GetComponent<MapZoneObstacle>();
                    if (obstacle)
                    {
                        var pResult = obstacle.boxCollider.bounds.ClosestPoint(transformCheck.position);
                        var dir = transformCheck.position - pResult;
                        dir = dir.normalized;
                        //    dir.y = 0;
                        return new MapZoneObstacleData(true, pResult + (dir * 0.2f));
                    }
                }
            }

            return new MapZoneObstacleData(false, Vector3.zero);
        }

        private MapZoneObstacleData CheckObstacle(Vector3 point, Vector3 positionCheck)
        {
            var hits = Physics2D.OverlapCircleAll(point, 0.2f);
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    var obstacle = hit.transform.GetComponent<MapZoneObstacle>();
                    if (obstacle)
                    {
                        var pResult = obstacle.boxCollider.bounds.ClosestPoint(positionCheck);
                        var dir = positionCheck - pResult;
                        dir = dir.normalized;
                        return new MapZoneObstacleData(true, pResult + (dir * 0.2f));
                    }
                }
            }

            return new MapZoneObstacleData(false, Vector3.zero);
        }

        private Vector3 GetConstrainsVectorOnPath(Vector3 targetPosition, int currentPoint, Transform transformCheck, float offsetMutipler = 0)
        {
            var lastPoint = sections.Count - 1;
            var firstAviablePoint = 0;
            if (activeConstrains)
            {
                var limitLef = GetLimitLeft(firstAviablePoint, transformCheck, offsetMutipler);
                var limitRight = GetLimitRight(lastPoint, transformCheck, offsetMutipler);
                var limitDown = GetLimitDown(currentPoint, transformCheck, offsetMutipler);
                var limitUp = GetLimitUp(currentPoint, transformCheck, offsetMutipler);
                targetPosition.y = Mathf.Clamp(targetPosition.y, limitDown, limitUp);
                targetPosition.x = Mathf.Clamp(targetPosition.x, limitLef, limitRight);
            }

            return targetPosition;
        }

        private Vector3 GetConstrainsVectorOnPath(Vector3 targetPosition, int currentPoint, Vector3 positionCheck, float offsetMutipler = 0)
        {
            var lastPoint = sections.Count - 1;
            var firstAviablePoint = 0;
            if (activeConstrains)
            {
                var limitLef = GetLimitLeft(firstAviablePoint, positionCheck, offsetMutipler);
                var limitRight = GetLimitRight(lastPoint, positionCheck, offsetMutipler);
                var limitDown = GetLimitDown(currentPoint, positionCheck, offsetMutipler);
                var limitUp = GetLimitUp(currentPoint, positionCheck, offsetMutipler);
                targetPosition.y = Mathf.Clamp(targetPosition.y, limitDown, limitUp);
                targetPosition.x = Mathf.Clamp(targetPosition.x, limitLef, limitRight);
            }

            return targetPosition;
        }

        private float GetLimitUp(int point, Transform transformCheck, float offsetMultipler = 0)
        {
            var pointUp = sections[point].limitUp;
            var pointCenter = sections[point].point;

            var pointCenterNext = sections[point + 1].point;
            var pointUpNext = sections[point + 1].limitUp;

            var lerpVectorTarget = pointUpNext;
            var lerpVectorFrom = pointUp;

            if (transformCheck.position.x < pointUp.x && point == 0)
                lerpVectorTarget = pointCenter;

            if (point == sections.Count - 2 && transformCheck.position.x > pointUpNext.x)
            {
                if (pointCenterNext.x > pointUpNext.x)
                {
                    lerpVectorTarget = pointCenterNext;
                    lerpVectorFrom = pointUpNext;
                }
            }

            return Vector3.Lerp(lerpVectorFrom, lerpVectorTarget, Mathf.InverseLerp(lerpVectorFrom.x, lerpVectorTarget.x, transformCheck.position.x)).y;
        }

        private float GetLimitUp(int point, Vector3 positionCheck, float offsetMultipler = 0)
        {
            var pointUp = sections[point].limitUp;
            var pointCenter = sections[point].point;

            var pointCenterNext = sections[point + 1].point;
            var pointUpNext = sections[point + 1].limitUp;

            var lerpVectorTarget = pointUpNext;
            var lerpVectorFrom = pointUp;

            if (positionCheck.x < pointUp.x && point == 0)
                lerpVectorTarget = pointCenter;

            if (point == sections.Count - 2 && positionCheck.x > pointUpNext.x)
            {
                if (pointCenterNext.x > pointUpNext.x)
                {
                    lerpVectorTarget = pointCenterNext;
                    lerpVectorFrom = pointUpNext;
                }
            }

            return Vector3.Lerp(lerpVectorFrom, lerpVectorTarget, Mathf.InverseLerp(lerpVectorFrom.x, lerpVectorTarget.x, positionCheck.x)).y;
        }

        private float GetLimitDown(int point, Transform transformCheck, float offsetMultipler = 0)
        {
            var pointDown = sections[point].limitDown;
            var pointCenter = sections[point].point;

            var pointDownNext = sections[point + 1].limitDown;
            var pointCenterNext = sections[point + 1].point;

            var lerpVectorTarget = pointDownNext;
            var lerpVectorFrom = pointDown;

            if (transformCheck.position.x < pointDown.x && point == 0)
                lerpVectorTarget = pointCenter;

            if (point == sections.Count - 2 && transformCheck.position.x > pointDownNext.x)
            {
                if (pointCenterNext.x > pointDownNext.x)
                {
                    lerpVectorTarget = pointCenterNext;
                    lerpVectorFrom = pointDownNext;
                }
            }

            return Vector3.Lerp(lerpVectorFrom, lerpVectorTarget, Mathf.InverseLerp(lerpVectorFrom.x, lerpVectorTarget.x, transformCheck.position.x)).y;
        }

        private float GetLimitDown(int point, Vector3 positionCheck, float offsetMultipler = 0)
        {
            var pointDown = sections[point].limitDown;
            var pointCenter = sections[point].point;

            var pointDownNext = sections[point + 1].limitDown;
            var pointCenterNext = sections[point + 1].point;

            var lerpVectorTarget = pointDownNext;
            var lerpVectorFrom = pointDown;

            if (positionCheck.x < pointDown.x && point == 0)
                lerpVectorTarget = pointCenter;

            if (point == sections.Count - 2 && positionCheck.x > pointDownNext.x)
            {
                if (pointCenterNext.x > pointDownNext.x)
                {
                    lerpVectorTarget = pointCenterNext;
                    lerpVectorFrom = pointDownNext;
                }
            }

            return Vector3.Lerp(lerpVectorFrom, lerpVectorTarget, Mathf.InverseLerp(lerpVectorFrom.x, lerpVectorTarget.x, positionCheck.x)).y;
        }

        private float GetLimitLeft(int point, Transform transformCheck, float offsetMultipler = 0)
        {
            var pointUp = sections[point].limitUp;
            var pointDown = sections[point].limitDown;
            var pointCenter = sections[point].point;

            var pointUpFix = pointUp;
            if (pointCenter.x < pointUpFix.x)
                pointUpFix = pointCenter;

            var pointDownFix = pointDown;
            if (pointCenter.x < pointDownFix.x)
                pointDownFix = pointCenter;

            return Vector3.Lerp(pointUpFix, pointDownFix, Mathf.InverseLerp(pointUpFix.y, pointDownFix.y, transformCheck.position.y)).x + 0.01f + offsetMultipler;
        }

        private float GetLimitLeft(int point, Vector3 positionCheck, float offsetMultipler = 0)
        {
            var pointUp = sections[point].limitUp;
            var pointDown = sections[point].limitDown;
            var pointCenter = sections[point].point;

            var pointUpFix = pointUp;
            if (pointCenter.x < pointUpFix.x)
                pointUpFix = pointCenter;

            var pointDownFix = pointDown;
            if (pointCenter.x < pointDownFix.x)
                pointDownFix = pointCenter;

            return Vector3.Lerp(pointUpFix, pointDownFix, Mathf.InverseLerp(pointUpFix.y, pointDownFix.y, positionCheck.y)).x + 0.01f + offsetMultipler;
        }

        private float GetLimitRight(int point, Transform transformCheck, float offsetMultipler = 0)
        {
            var pointUp = sections[point].limitUp;
            var pointDown = sections[point].limitDown;
            var pointCenter = sections[point].point;

            var pointUpFix = pointUp;
            if (pointCenter.x > pointUpFix.x)
                pointUpFix = pointCenter;

            var pointDownFix = pointDown;
            if (pointCenter.x > pointDownFix.x)
                pointDownFix = pointCenter;

            return Vector3.Lerp(pointUpFix, pointDownFix, Mathf.InverseLerp(pointUpFix.y, pointDownFix.y, transformCheck.position.y)).x - 0.01f + offsetMultipler;
        }

        private float GetLimitRight(int point, Vector3 positionCheck, float offsetMultipler = 0)
        {
            var pointUp = sections[point].limitUp;
            var pointDown = sections[point].limitDown;
            var pointCenter = sections[point].point;

            var pointUpFix = pointUp;
            if (pointCenter.x > pointUpFix.x)
                pointUpFix = pointCenter;

            var pointDownFix = pointDown;
            if (pointCenter.x > pointDownFix.x)
                pointDownFix = pointCenter;

            return Vector3.Lerp(pointUpFix, pointDownFix, Mathf.InverseLerp(pointUpFix.y, pointDownFix.y, positionCheck.y)).x - 0.01f + offsetMultipler;
        }

        #endregion Class Methods
    }
}