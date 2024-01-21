using UnityEngine;

namespace Runtime.Gameplay.EntitySystem
{
    public interface IInteractable
    {
        #region Properties

        public EntityModel Model { get; }
        public bool IsMainHero { get; }
        public bool IsDead => Model.IsDead;
        public uint EntityUId => Model.EntityUId;
        public Vector2 CenterPosition => Model.CenterPosition;
        public Vector2 Position => Model.Position;
        public bool IsHero => Model.EntityType.IsHero();
        public bool IsObject => Model.EntityType.IsObject();
        public bool IsEnemyOrBoss => Model.EntityType.IsEnemyOrBoss();
        public bool IsCharacter => Model.EntityType.IsCharacter();
        public bool IsObjectTree => Model.EntityType.IsObjectTree();
        public bool IsObjectCrystal => Model.EntityType.IsObjectCrystal();
        public int DetectedPriority => Model.DetectedPriority;

        #endregion Propreties

        #region Class Methods

        void GetHit();
        void GetAffected();
        bool CanGetAffected();

        public Vector2 GetEdgePoint(Vector2 centerToEdgeDirection)
            => Model.GetEdgePoint(centerToEdgeDirection);

        #endregion Class Methods
    }
}