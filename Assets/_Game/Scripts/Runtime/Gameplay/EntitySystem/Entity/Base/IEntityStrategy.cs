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

        void Build(EntityModel model, Vector3 position);
        void SetActive(bool isActive);
        void Jump();
        void Collision();
        void Missed();
     
        #endregion Interface Methods
    }
}