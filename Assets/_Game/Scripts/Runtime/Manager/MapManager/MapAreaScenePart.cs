using UnityEngine;

namespace Runtime.Gameplay.Map
{
    public abstract class MapAreaScenePart<T1, T2> : MonoBehaviour where T1 : MapArea<T2> where T2 : MapManager
    {
        #region Members

        [SerializeField]
        private T1 _mapArea;
        private bool _isCurrentlyShown;

        #endregion Members

        #region Properties

        public string MapAreaId
            => _mapArea.Id;

        public T1 MapArea
            => _mapArea;

        #endregion Properties

        #region API Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            _mapArea = gameObject.GetComponentInChildren<T1>(true);
        }
#endif

        #endregion API Methods

        #region Class Methods

        public virtual void Show()
        {
            if (!_isCurrentlyShown)
            {
                _mapArea.Show();
                _isCurrentlyShown = true;
            }
        }

        public virtual void Hide()
        {
            if (_isCurrentlyShown)
            {
                _mapArea.Hide();
                _isCurrentlyShown = false;
            }
        }

        #endregion Class Methods
    }
}