using UnityEngine;

namespace Runtime.Gameplay.EntitySystem
{
    public interface IEntityStrategy
    {
        #region Properties

        uint EntityUId { get; }
        bool IsActive { get; }
        
        #endregion Properties

        #region Interface Methods

        void Build(EntityModel model, Vector3 spawnPosition, Vector3 destinationPosition);
        void SetActive(bool isActive);
        void Jump();
        void Collision();
        void Missed();
        bool CanAllowCollision();

        #endregion Interface Methods
    }
}