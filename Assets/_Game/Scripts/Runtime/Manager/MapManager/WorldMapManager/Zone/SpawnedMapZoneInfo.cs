using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Gameplay.Map
{
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
}