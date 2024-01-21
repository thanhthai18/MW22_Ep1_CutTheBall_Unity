using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

namespace Runtime.Gameplay.Visual
{
    public class ExpVisual : AnimatedVisual
    {
        #region Members

        [SerializeField]
        private TextMeshPro _damageText;
        [SerializeField]
        private float _lifeTime = 0.5f;
        private Vector3 _originalScale;
        [SerializeField]
        private Color _colorAdd;
        private Action _onCollectResource;
        [SerializeField]
        private AnimationCurve _animationCurve;
        [SerializeField]
        private float _valueFromBeginScale;
        [SerializeField]
        private float _durationBeginScale;

        #endregion Members

        #region API Methods

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void BeginShow()
        {
            transform.localScale = Vector3.one * _valueFromBeginScale;
            transform.DOScale(_originalScale, _durationBeginScale).SetUpdate(true).SetEase(_animationCurve).OnComplete(() => {
                OnHide();
            });
        }

        public void OnHide()
        {
            transform.DOMoveY(transform.position.y + 1, _lifeTime).SetUpdate(true);
            transform.DOMoveZ(transform.position.z, _lifeTime / 2).SetUpdate(true).OnComplete(() => {
                transform.DOScale(0, 0.2f).SetUpdate(true).OnComplete(() => {
                    transform.localScale = _originalScale;
                    OnCompletedAnimation();
                }
                );
            });
        }

        private void OnDisable()
            => transform.DOKill();

        #endregion API Methods

        #region Class Methods

        public void Init(Vector2 spawnPosition, long number, Action onCollectResource)
        {
            transform.DOKill();
            BeginShow();
            Init(spawnPosition);
            if (number > 0)
            {
                _damageText.color = _colorAdd;
                _damageText.text = $"EXP {number}";
            }
            else
                _damageText.text = "";
            _onCollectResource = onCollectResource;
        }

        protected override void OnCompletedAnimation()
        {
            _onCollectResource?.Invoke();
            GameplayVisualController.Instance.HideExpVisual(this);
        }

        #endregion Class Methods
    }
}