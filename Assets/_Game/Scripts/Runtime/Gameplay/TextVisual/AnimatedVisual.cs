using UnityEngine;
using Runtime.Manager.Pool;

namespace Runtime.Gameplay.Visual
{
    public abstract class AnimatedVisual : MonoBehaviour
    {
        #region Members

        [SerializeField]
        private float _visualHeightOffset;

        #endregion Members

        #region Class Methods

        public virtual void Init(Vector2 spawnPosition)
            => transform.position = spawnPosition + new Vector2(0, _visualHeightOffset);

        protected virtual void OnCompletedAnimation()
            => PoolManager.Instance.Remove(gameObject);

        #endregion Class Methods
    }
}