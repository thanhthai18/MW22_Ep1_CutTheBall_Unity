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
        protected Vector2 destinationPosition;
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

        public Vector2 DestinationPosition
        {
            get => destinationPosition;
            set => destinationPosition = value;
        } 
        
        public uint EntityUId { get { return entityUId; } }
        public string EntityId { get { return entityId; } }
        public abstract EntityType EntityType { get; }

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
}