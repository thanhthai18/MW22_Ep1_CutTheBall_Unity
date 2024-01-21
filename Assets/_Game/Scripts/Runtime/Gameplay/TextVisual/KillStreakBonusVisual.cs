using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using Runtime.Localization;

namespace Runtime.Gameplay.Visual
{
    public class KillStreakBonusVisual : AnimatedVisual
    {
        #region Members

        [SerializeField]
        private TextMeshPro _contentText;
        [SerializeField]
        private float _lifeTime = 0.5f;
        [SerializeField]
        private float _minX = -0.2f;
        [SerializeField]
        private float _maxX = 0.2f;
        [SerializeField]
        private float _minY = 0.2f;
        [SerializeField]
        private float _maxY = 0.5f;
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenMove;
        private Vector3 _originalScale;

        #endregion Members

        #region API Methods

        private void OnEnable()
        {
            _contentText.text = LocalizationUtils.GetDungeonGameplayLocalized("kill_streak_bonus");
            _originalScale = transform.localScale;
        }

        public void OnHide()
        {
            transform.DOMoveZ(transform.position.z, _lifeTime).OnComplete(() =>
            transform.DOScale(0, 0.1f).OnComplete(() => {
                transform.localScale = _originalScale;
                OnCompletedAnimation();
            }
            ));
        }

        private void OnDisable()
            => _tweenMove.Kill();

        #endregion API Methods

        #region Class Methods

        protected override void OnCompletedAnimation()
            => GameplayVisualController.Instance.HideKillStreakBonusVisual(this);

        #endregion Class Methods
    }
}