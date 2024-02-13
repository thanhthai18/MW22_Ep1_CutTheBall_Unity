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
        public bool IsBall => Model.EntityType.IsBall();
        public bool IsBoom => Model.EntityType.IsBoom();
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