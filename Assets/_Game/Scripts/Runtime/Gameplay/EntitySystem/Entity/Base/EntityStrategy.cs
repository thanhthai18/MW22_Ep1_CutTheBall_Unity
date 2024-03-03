using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Definition;
using Runtime.Extensions;
using Runtime.Gameplay.Manager;
using Runtime.Manager.Pool;
using UnityEngine;

namespace Runtime.Gameplay.EntitySystem
{
    public abstract class EntityStrategy<T> : Disposable, IEntityStrategy, IFaceDirection where T : EntityModel
    {
        #region Members

        protected T ownerModel;
        protected IEntityAnimationPlayer entityAnimationPlayer;
        protected Vector3 previousPosition;
        private readonly int _numberObJumps = 1;
        private readonly float _powerRotate = 5;

        #endregion Members

        #region Properties

        public uint EntityUId { get; private set; }
        public bool IsActive => ownerModel.IsActive;
        public float JumpPower => ownerModel.JumpPower;
        public float JumpDuration => ownerModel.JumpDuration;
        public FaceDirectionType FaceDirection => ownerModel.DestinationPosition.x >= ownerModel.OriginalPosition.x ? FaceDirectionType.FaceRight : FaceDirectionType.FaceLeft;

        #endregion Properties

        #region API Methods

        private void OnMouseEnter()
        {
            if (CanAllowCollision())
                Collision();
        }

        #endregion API Methods

        #region Class Methods
        
        public virtual void Build(EntityModel model, Vector3 spawnPosition, Vector3 destinationPosition)
        {
            ownerModel = model as T;
            EntityUId = model.EntityUId;
            HasDisposed = false;
            SetUpPosition(spawnPosition, destinationPosition);
            SetUpScale();
            ExecuteInitialize();
        }

        public virtual void Jump()
        {
            Vector3 jumpTarget = Constant.GetRandomStartPosition();

            var finalJumpPower = JumpPower + GameExtensions.RandomTwoValue(1, -1) * Constant.SPREAD_JUMP_POWER;
            transform.DOJump(ownerModel.DestinationPosition, finalJumpPower, _numberObJumps, JumpDuration)
                .OnUpdate(RotateSelf)
                .SetEase(Ease.OutQuad)
                .OnComplete(Missed);
            SetAnimation(EntityAnimationState.Move);
        }

        public virtual void RotateSelf()
        {
            if (FaceDirection == FaceDirectionType.FaceRight)
            {
                transform.eulerAngles += new Vector3(0, 0, _powerRotate);
            }
            else if (FaceDirection == FaceDirectionType.FaceLeft)
            {
                transform.eulerAngles -= new Vector3(0, 0, _powerRotate);
            }
        }
        
        public virtual void Collision()
        {
            ownerModel.SetActive(false);
            SetAnimation(EntityAnimationState.Explore);
            //GenerateVFXExplore(ownerModel).Forget();
            transform.DOKill();
        } 
        
        public virtual void Missed()
        {
            if (IsActive)
            {
                SetActive(false);
                PoolManager.Instance.Remove(gameObject);
                transform.DOKill();
            }
        }

        public virtual bool CanAllowCollision() => ownerModel.IsActive && SplitManager.Instance.InSplit;

        protected virtual void InitActions(T model)
        {
            Jump();
        }
        
        protected virtual void InitAnimations()
        {
            entityAnimationPlayer = transform.GetComponentInChildren<IEntityAnimationPlayer>(true);
#if UNITY_EDITOR
            if (entityAnimationPlayer == null)
            {
                Debug.LogError($"Require a character animation player for this behavior!");
                return;
            }
#endif
            entityAnimationPlayer.Init();
            SetAnimation(EntityAnimationState.Idle);
        }
        
        public void SetActive(bool isActive)
        {
            ownerModel.SetActive(isActive);
            gameObject.SetActive(isActive);
        }
        
        public override void Dispose()
        {
            if (!HasDisposed)
            {
                HasDisposed = true;
                ExecuteDispose();
            }
        }
        
        protected virtual void SetUpPosition(Vector3 position, Vector3 desinationPosition)
        {
            transform.position = position;
            ownerModel.Position = position;
            ownerModel.OriginalPosition = position;
            ownerModel.DestinationPosition = desinationPosition;
        }
        
        protected virtual void SetUpScale()
            => transform.localScale = Vector3.one;

        protected void SetAnimation(EntityAnimationState state)
            => entityAnimationPlayer.Play(state);

        protected async UniTask GenerateVFXExplore(T model)
        {
            try
            {
                var entityType = model.EntityType;
                string prefabId = string.Format(VFXKey.EXPLORE_VFX, entityType);
                await PoolManager.Instance.Get(prefabId, gameObject.GetCancellationTokenOnDestroy());
            }
            catch
            {
                Debug.Log("chua co vfx");
            }
        }
        
        protected virtual void ExecuteValidate() { }

        protected virtual void ExecuteInitialize()
        {
            InitAnimations();
            InitActions(ownerModel);
        }
        protected virtual void ExecuteDispose() { }

        #endregion Class Methods

    }
}