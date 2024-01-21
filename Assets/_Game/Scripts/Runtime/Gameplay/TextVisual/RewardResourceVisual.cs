using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Runtime.Gameplay.Visual
{
    public class RewardResourceVisual : AnimatedVisual
    {
        #region Members

        [SerializeField] private TextMeshPro _rewardNumberText;
        [SerializeField] private SpriteRenderer _visualSpriteRenderer;
        [SerializeField] private float _jumpHeight = 3.0f;
        [SerializeField] private float _jumpDuration = 1.0f;
        [SerializeField] private float _maxOffsetPos = 2;
        [SerializeField] private float _minOffsetPos = 1.5f;
        [SerializeField] private float _fadeDuration = 0.2f;
        [SerializeField] private float _eulerRotate = 360;
        [SerializeField] private float _dynamicPosRange = 0.3f;
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private Gradient[] _colors;

        private Vector3 _initialPosition;
        private Vector3 _originScaleRewardText;

        #endregion Members

        #region API Methods

        private void OnDisable() => _trailRenderer.gameObject.SetActive(false);

        #endregion API Methods

        #region Class Methods

        public void Init(Vector2 spawnPosition, Sprite sprite, long number, Action onCollectResource, bool isDynamic = false, int colorIndex = 0)
        {
            _originScaleRewardText = _rewardNumberText.transform.localScale;
            _rewardNumberText.transform.localScale = Vector3.zero;
            _rewardNumberText.text = $"+{number}";
            _visualSpriteRenderer.sprite = sprite;
            Init(spawnPosition);
            if (isDynamic)
            {
                spawnPosition = new Vector2(spawnPosition.x, spawnPosition.y + (UnityEngine.Random.Range(-_dynamicPosRange, _dynamicPosRange)));
            }

            _initialPosition = spawnPosition;
            _trailRenderer.gameObject.SetActive(true);
            if (colorIndex < _colors.Length)
                _trailRenderer.colorGradient = _colors[colorIndex];
            PlayDropAnimation(onCollectResource);
        }

        private void PlayDropAnimation(Action onCollectResource)
        {
            _visualSpriteRenderer.transform.localEulerAngles = Vector3.zero;
            _visualSpriteRenderer.transform.DORotate(new Vector3(0, 0, _visualSpriteRenderer.transform.localEulerAngles.z + _eulerRotate), _jumpDuration, RotateMode.FastBeyond360);
            _visualSpriteRenderer.color = new Color(_visualSpriteRenderer.color.r, _visualSpriteRenderer.color.g, _visualSpriteRenderer.color.b, 1);
            float range = UnityEngine.Random.Range(-_maxOffsetPos, _maxOffsetPos);
            var targetX = _initialPosition.x + (range > 0 ? _minOffsetPos + range : -_minOffsetPos + range);

            transform.DOJump(new Vector2(targetX, _initialPosition.y), _jumpHeight, 1, _jumpDuration).SetEase(Ease.OutQuad).OnComplete(() => {
                DOVirtual.DelayedCall(0.1f, () => {
                    _visualSpriteRenderer.DOFade(0, _fadeDuration);
                    var tmpPosSprite = _visualSpriteRenderer.transform.position;
                    _visualSpriteRenderer.transform.DOMoveY(_visualSpriteRenderer.transform.position.y + _maxOffsetPos / 4, _fadeDuration).OnComplete(() => _visualSpriteRenderer.transform.position = tmpPosSprite);
                    _rewardNumberText.transform.DOScale(_originScaleRewardText, 0.1f).OnComplete(() => {
                        onCollectResource?.Invoke();
                        _rewardNumberText.transform.DOMoveZ(_rewardNumberText.transform.position.z, 0.5f).OnComplete(() => _rewardNumberText.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => {
                            _rewardNumberText.transform.localScale = _originScaleRewardText;
                            OnCompletedAnimation();
                        }));
                    });
                });
            });
        }

        #endregion Class Methods
    }
}