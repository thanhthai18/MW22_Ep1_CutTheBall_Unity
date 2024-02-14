using Runtime.Config;
using UnityEngine;
using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public abstract class EntityModel
    {
        #region Members
        
        protected uint entityUId;
        protected string entityId;
        protected Vector2 position;
        protected Vector2 originalPosition;
        protected bool isActive;
        protected float jumpPower;
        protected float jumpDuration;

        #endregion Members

        #region Properties

        public virtual bool IsDead
        {
            get => false;
        }

        public virtual bool IsActive
        {
            get => isActive;
        }

        public virtual float JumpDuration => jumpDuration;
        
        public virtual float JumpPower => jumpPower;

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public Vector2 OriginalPosition
        {
            get => originalPosition;
            set => originalPosition = value;
        }
        
        public uint EntityUId { get { return entityUId; } }
        public string EntityId { get { return entityId; } }
        public EntityType EntityType { get; private set; }

        #endregion Properties

        #region Class Methods

        public EntityModel(uint entityUId, string entityId, EntityModelData entityModelData)
        {
            InitEvents();
            this.entityUId = entityUId;
            this.entityId = entityId;
            jumpPower = entityModelData.configItem.jumpPower;
            jumpDuration = entityModelData.configItem.jumpDuration;
        }

        public void SetActive(bool isActive)
            => this.isActive = isActive;

        public static implicit operator bool(EntityModel entityModel)
            => entityModel != null;

        protected virtual void InitEvents()
        {
        }

        #endregion Class Methods
    }

    public static class EntityModelExtensions
    {
        #region Class Methods

        public static bool IsBall(this EntityType entityType)
            => entityType == EntityType.Ball;

        public static bool IsBoom(this EntityType entityType)
            => entityType == EntityType.Boom;


        #endregion Class Methods
    }
    
    public class EntityModelData
    {
        #region Members

        public EntityDataConfigItem configItem;
        
        #endregion Members
        
        #region Class Methods
        
        public EntityModelData(EntityDataConfigItem configItem)
            => this.configItem = configItem;

        #endregion Class Methods
    }
}