using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Runtime.Gameplay.Map
{
    public abstract class MapArea<T> : MonoBehaviour where T : MapManager
    {
        #region Members

#if UNITY_EDITOR
        [Header("--- MANUALLY CACHING ---")]
        [SerializeField]
        private bool _manuallyCache;
#endif

        [Header("--- MAP AREA ID ---")]
        [SerializeField]
        private string _id;

        [Header("--- CACHED MAP ZONES ---")]
        [SerializeField]
        [ReadOnly]
        protected MapZone[] mapZones;

        protected bool isCurrentlyShown;
        protected T ownerMapManager;

        #endregion Members

        #region Properties

        public string Id
            => _id;

        public virtual MapZone[] MapZones
            => mapZones;

        #endregion Properties

        #region API Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

        }
#endif

        #endregion API Methods

        #region Class Methods

        public virtual void Init(T ownerMapManager)
        {
            this.ownerMapManager = ownerMapManager;
            foreach (var mapZone in mapZones)
                mapZone.SetMapAreaId(Id);
        }

        public virtual void Show()
        {
            if (!isCurrentlyShown)
            {
                isCurrentlyShown = true;
                SetUpShow();
            }
        }

        public virtual void Hide()
        {
            if (isCurrentlyShown)
            {
                isCurrentlyShown = false;
                SetUpHide();
            }
        }

        protected virtual void SetUpShow()
        {
            InitAreaData();
            SetVisibility(true);
        }

        protected virtual void SetUpHide()
            => SetVisibility(false);

        protected virtual void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);

        protected virtual void InitAreaData() { }


        #endregion Class Methods
    }
}