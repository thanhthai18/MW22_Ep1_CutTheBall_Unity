using System.Linq;
using UnityEngine;
using Runtime.Definition;
using Runtime.Gameplay.Manager;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Runtime.Gameplay.EntitySystem;

namespace Runtime.Gameplay.Map
{
    public class WorldMapChunk : MapArea<WorldMapManager>
    {
        #region Members

        [Header("--- CACHED MAP FOGS ---")]
        [SerializeField]
        [ReadOnly]
        private MapFog[] _mapFogs;

        [Header("--- CHEST SPAWN POINTS CONTAINER TRANSFORM ---")]
        [SerializeField]
        private Transform _chestSpawnPointsContainerTransform;

        [Header("--- CACHED CHEST SPAWN POSITIONS ---")]
        [SerializeField]
        [ReadOnly]
        private Vector2[] _chestSpawnPositions;

        private bool _hasSpawnedEntitiesInZones;

        #endregion Members

        #region Properties

     

        public Vector2[] ChestSpawnPositions
        {
            get
            {
                return _chestSpawnPositions;
            }
        }

        #endregion Properties

        #region Class Methods

        protected override void SetUpShow()
        {
            base.SetUpShow();
        }

        protected override void SetUpHide()
        {
            base.SetUpHide();
        }

        protected override void InitAreaData()
        {
            base.InitAreaData();

        }

        #endregion Class Methods
    }
}