using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Runtime.Gameplay.Manager;
using Runtime.Manager.Data;
using Cysharp.Threading.Tasks;

namespace Runtime.Gameplay.Map
{
    public class MapFog : MonoBehaviour
    {
        #region Members

        public string fogId;
        private Tilemap _ownerTilemap;

        #endregion Members

        public string BelongedMapAreaId
        {
            get;
            private set;
        }

        public string FullFogId
            => $"{BelongedMapAreaId}_{fogId}";

    }
}