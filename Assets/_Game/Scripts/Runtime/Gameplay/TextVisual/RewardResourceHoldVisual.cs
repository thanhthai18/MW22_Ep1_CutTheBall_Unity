using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Runtime.Animation;

namespace Runtime.Gameplay.Visual
{
    public class RewardResourceHoldVisual : AnimatedVisual
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
        [SerializeField] private float _durationRotate = 2;
        [SerializeField] private float _baseDurationCollect = 0.2f;
        [SerializeField] private GameObject[] _vFXs;
        [SerializeField] private SpriteAnimator _spriteAnimator;
        [SerializeField] private SpriteAnimation _spriteAnimation;

        private Vector3 _initialPosition;
        private Vector3 _originScaleRewardText;
        private Action _onCollectResourceCallback;

        #endregion Members

        #region API Methods

        private void OnEnable()
        {
            _spriteAnimator.Stop();
        }

        private void OnDisable()
        {
            _spriteAnimator.Stop();
        }

        #endregion API Methods

        #region Class Methods

        public void Init(Vector2 spawnPosition, Sprite sprite, long number, Action onCollectResource, bool isDynamic = false, int colorIndex = 0, bool isGold = false)
        {
            if (!isGold)
            {
                _visualSpriteRenderer.sprite = sprite;
            }
            else
            {
                _spriteAnimator.enabled = true;
                _spriteAnimator.Play(_spriteAnimation, playOneShot: false);
            }
            Init(spawnPosition);
            if (isDynamic)
            {
                spawnPosition = new Vector2(spawnPosition.x, spawnPosition.y + (UnityEngine.Random.Range(-_dynamicPosRange, _dynamicPosRange)));
            }

            _initialPosition = spawnPosition;
            _trailRenderer.gameObject.SetActive(true);
            if (colorIndex < _colors.Length)
                _trailRenderer.colorGradient = _colors[colorIndex];

            ShowVFX(colorIndex);

            _onCollectResourceCallback = onCollectResource;
            PlayDropAnimation();
        }

        private void PlayDropAnimation()
        {
            _visualSpriteRenderer.transform.localEulerAngles = Vector3.zero;
            _visualSpriteRenderer.transform.DORotate(new Vector3(0, 0, _visualSpriteRenderer.transform.localEulerAngles.z + _eulerRotate), _jumpDuration, RotateMode.FastBeyond360);
            _visualSpriteRenderer.color = new Color(_visualSpriteRenderer.color.r, _visualSpriteRenderer.color.g, _visualSpriteRenderer.color.b, 1);
            float rangeRandom1 = UnityEngine.Random.Range(-_maxOffsetPos, _maxOffsetPos);
            float rangeRandom2 = UnityEngine.Random.Range(-_maxOffsetPos, _maxOffsetPos);
            var targetX = _initialPosition.x + (rangeRandom1 > 0 ? _minOffsetPos + rangeRandom1 : -_minOffsetPos + rangeRandom1);
            var targetY = _initialPosition.y + rangeRandom2;

            transform.DOJump(new Vector2(targetX, targetY), _jumpHeight, 1, _jumpDuration).SetEase(Ease.OutQuad).OnComplete(() => {
                _trailRenderer.gameObject.SetActive(false);
            });
        }

        private void ShowVFX(int colorIndex)
        {
            for (int i = 0; i < _vFXs.Length; i++)
            {
                _vFXs[i].SetActive(false);
                if (i == colorIndex - 1)
                {
                    _vFXs[i].SetActive(true);
                }
            }
        }

        public void OnCollect(Vector2 centerPositionHeroes)
        {
            var duration = UnityEngine.Random.Range(_baseDurationCollect, _baseDurationCollect * 2);
            transform.DOMove(centerPositionHeroes, duration).OnComplete(() => {
                _onCollectResourceCallback?.Invoke();
                _visualSpriteRenderer.transform.DOKill();
                OnCompletedAnimation();
            });
        }

        #endregion Class Methods
    }
}