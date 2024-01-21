using UnityEngine;
using DG.Tweening;
using TMPro;
using Runtime.Localization;

namespace Runtime.Gameplay.Visual
{
    public class DamageVisual : AnimatedVisual
    {
        #region Members

        [SerializeField]
        private TextMeshPro _damageText;
        [SerializeField]
        private float _lifeTime = 0.5f;
        private Vector3 _originalScale;
        [SerializeField]
        private Color _colorAdd;
        [SerializeField]
        private Color _colorSub;
        [SerializeField]
        private Color _colorCrit;
        [SerializeField]
        private SpriteRenderer _spriteRendererCrit;
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

        public void OnHide()
        {
            // Random spawn
            float range = 1f;
            float randonX = Random.Range(-range, range);
            float randomY = Random.Range(-range, range);
            Vector2 spawnNew = transform.position;
            spawnNew.x += randonX;
            spawnNew.y += randomY;
            transform.DOMove(spawnNew, this._lifeTime).SetUpdate(true);
            transform.DOMoveZ(transform.position.z, _lifeTime / 2).SetUpdate(true).OnComplete(() => {
                if (_spriteRendererCrit != null)
                {
                    _spriteRendererCrit.DOFade(0, 0.2f).SetUpdate(true);
                }
                _damageText.DOFade(0, 0.2f).SetUpdate(true).OnComplete(() => {
                    transform.localScale = _originalScale;
                    OnCompletedAnimation();
                }
                );
            });
        }

        public void BeginShow()
        {
            var tween = transform.DOScale(Vector3.one * _valueFromBeginScale, _durationBeginScale).SetUpdate(true).SetEase(_animationCurve).OnComplete(() => {
                OnHide();
            });
            ((Tweener)tween).From(false);
        }


        private void OnDisable()
            => transform.DOKill();

        #endregion API Methods

        #region Class Methods

        public void Init(Vector2 spawnPosition, int value, bool isCrit)
        {
            transform.DOKill();
            BeginShow();
            Init(spawnPosition);
            if (value > 0)
            {
                _damageText.color = _colorAdd;
                _damageText.text = $"{value}";
            }
            else if (value < 0)
            {
                if (!isCrit)
                {
                    _damageText.color = _colorSub;
                }
                else
                {
                    _damageText.color = _colorCrit;
                }
                _damageText.text = $"{-value}";
            }
            else
            {
                _damageText.text = "";
            }

            if (_spriteRendererCrit != null)
            {
                _spriteRendererCrit.gameObject.SetActive(isCrit);
                _spriteRendererCrit.color = this._colorSub;
            }
        }

        public void InitMissedDamage(Vector2 spawnPosition)
        {
            transform.DOKill();
            BeginShow();
            Init(spawnPosition);
            _damageText.color = Color.white;
            _damageText.text = LocalizationUtils.GetGeneralLocalized(LocalizeKeys.MISS);
        }

        protected override void OnCompletedAnimation()
            => GameplayVisualController.Instance.HideDamageVisual(this);

        #endregion Class Methods
    }
}