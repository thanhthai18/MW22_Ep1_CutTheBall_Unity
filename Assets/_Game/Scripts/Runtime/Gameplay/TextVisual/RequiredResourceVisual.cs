using UnityEngine;
using DG.Tweening;
using Runtime.Definition;
using Cysharp.Threading.Tasks;

namespace Runtime.Gameplay.Visual
{
    public class RequiredResourceVisual : AnimatedVisual
    {
        #region Members

        [SerializeField]
        private SpriteRenderer _visualSpriteRenderer;
        [SerializeField]
        private float _eulerRotate = 360;
        [SerializeField]
        private float _dynamicPosRange = 0.1f;
        private Vector3 _initialPosition;

        #endregion Members

        #region Class Methods

        public void Init(Vector2 spawnPosition, Vector2 targetPosition, Sprite sprite)
        {
            _visualSpriteRenderer.sprite = sprite;
            spawnPosition = new Vector2(spawnPosition.x, spawnPosition.y + (UnityEngine.Random.Range(-_dynamicPosRange, _dynamicPosRange)));
            Init(spawnPosition);
            _initialPosition = spawnPosition;
            PlayDropAnimation(targetPosition);
        }

        private void PlayDropAnimation(Vector2 targetPosition)
        {
            _visualSpriteRenderer.transform.localEulerAngles = Vector3.zero;
            _visualSpriteRenderer.transform.DORotate(new Vector3(0, 0, _visualSpriteRenderer.transform.localEulerAngles.z + _eulerRotate), Constant.TIME_MOVE_RESOURCE, RotateMode.FastBeyond360);
            var originalScaleRenderer = _visualSpriteRenderer.transform.localScale;

            transform.DOMove(targetPosition, Constant.TIME_MOVE_RESOURCE)
           .SetEase(Ease.OutQuad)
           .OnComplete(() => {
               _visualSpriteRenderer.transform.DOScale(0, 0.1f).OnComplete(() => {
                   _visualSpriteRenderer.transform.localScale = originalScaleRenderer;
                   OnCompletedAnimation();
               });
           });
        }

        public int GetTimeMoveInMiliSecond()
        => (int)(Constant.TIME_MOVE_RESOURCE * 1000);

        #endregion Class Methods
    }
}