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
        protected int detectedPriority;
        protected Vector2 originalPosition;
        protected bool isActive;
        protected Bound bound;

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

        public Bound Bound
        {
            get => bound;
            set => bound = value;
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public Vector2 CenterPosition
        {
            get => position + Vector2.up * Height * 0.5f;
        }

        public Vector2 TopPosition
        {
            get => position + Vector2.up * Height;
        }

        public Vector2 OriginalPosition
        {
            get => originalPosition;
            set => originalPosition = value;
        }

        public virtual int Level { get { return 1; } }
        public float Height { get { return bound.Height; } }
        public float Radius { get { return bound.Width; } }
        public uint EntityUId { get { return entityUId; } }
        public string EntityId { get { return entityId; } }
        public int DetectedPriority { get { return detectedPriority; } }
        public abstract EntityType EntityType { get; }

        #endregion Properties

        #region Class Methods

        public EntityModel(uint entityUId, string entityId, int detectedPriority)
        {
            InitEvents();
            this.entityUId = entityUId;
            this.entityId = entityId;
            this.detectedPriority = detectedPriority;
        }

        public Vector2 GetEdgePoint(Vector2 centerToEdgeDirection)
            => Bound.GetEdgePoint(centerToEdgeDirection);

        public virtual float GetTotalStatValue(StatType statType)
            => 0;

        public void SetActive(bool isActive)
            => this.isActive = isActive;

        public static implicit operator bool(EntityModel entityModel)
            => entityModel != null;

        protected virtual void InitEvents() { }

        #endregion Class Methods
    }

    public static class EntityModelExtensions
    {
        #region Class Methods

        public static bool IsHero(this EntityType entityType)
            => entityType == EntityType.Hero;

        public static bool IsEnemyOrBoss(this EntityType entityType)
            => entityType == EntityType.Enemy || entityType == EntityType.Boss;

        public static bool IsCharacter(this EntityType entityType)
            => IsHero(entityType) || IsEnemyOrBoss(entityType);

        public static bool IsObjectTree(this EntityType entityType)
            => entityType == EntityType.ObjectTree;

        public static bool IsObjectCrystal(this EntityType entityType)
            => entityType == EntityType.ObjectCrystal;

        public static bool IsObject(this EntityType entityType)
            => entityType == EntityType.ObjectCrystal || entityType == EntityType.ObjectTree;

        #endregion Class Methods
    }
}