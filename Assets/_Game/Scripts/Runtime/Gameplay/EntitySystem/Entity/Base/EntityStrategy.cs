using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Definition;
using Runtime.Manager.Pool;
using UnityEngine;

namespace Runtime.Gameplay.EntitySystem
{
    public abstract class EntityStrategy<T> : Disposable, IEntityStrategy where T : EntityModel
    {
        #region Members

        protected T ownerModel;
        protected IEntityAnimationPlayer entityAnimationPlayer;

        #endregion Members

        #region Properties

        public uint EntityUId { get; private set; }
        public bool IsActive => ownerModel.IsActive;
        public float JumpPower { get; private set; }
        public float JumpDuration { get; private set; }

        #endregion Properties

        #region Class Methods
        
        public virtual void Build(EntityModel model, Vector3 position)
        {
            ownerModel = model as T;
            EntityUId = model.EntityUId;
            HasDisposed = false;
            SetUpPosition(position);
            SetUpScale();
            SetUpPhysics(ownerModel);
            ExecuteInitialize();
        }

        public virtual void Jump()
        {
            Vector3 jumpTarget = Constant.GetRandomStartPosition(); 

            transform.DOJump(jumpTarget, JumpPower, 1, JumpDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(Missed);
            SetAnimation(EntityAnimationState.Move);
        }
        
        public virtual void Collision()
        {
            SetAnimation(EntityAnimationState.Explore);
            GenerateVFXExplore(ownerModel).Forget();
        } 
        
        public virtual void Missed()
        {
            if (IsActive)
            {
                SetActive(false);
                PoolManager.Instance.Remove(gameObject);
            }
        }

        protected virtual void InitActions(T model)
        {
            Jump();
        }
        
        protected virtual void InitAnimations(T model)
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
        
        protected virtual void SetUpPosition(Vector3 position)
        {
            transform.position = position;
            ownerModel.Position = position;
            ownerModel.OriginalPosition = position;
        }
        
        protected virtual void SetUpScale()
            => transform.localScale = Vector3.one;

        protected virtual void SetUpPhysics(T model)
        {
            JumpPower = model.JumpPower;
            JumpDuration = model.JumpDuration;
        }
        
        protected void SetAnimation(EntityAnimationState state)
            => entityAnimationPlayer.Play(state);

        protected async UniTask GenerateVFXExplore(T model)
        {
            var entityType = model.EntityType;
            string prefabId = string.Format(VFXKey.EXPLORE_VFX, entityType);
            await PoolManager.Instance.Get(prefabId, gameObject.GetCancellationTokenOnDestroy());
        }
        
        protected virtual void ExecuteValidate() { }

        protected virtual void ExecuteInitialize()
        {
            InitAnimations(ownerModel);
            InitActions(ownerModel);
        }
        protected virtual void ExecuteDispose() { }

        #endregion Class Methods

    }
}