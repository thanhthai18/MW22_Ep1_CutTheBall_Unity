using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public class BallModel : EntityModel
    {
        #region Properties

        public override EntityType EntityType => EntityType.Ball;

        #endregion Properties
        
        public BallModel(uint entityUId, string entityId, EntityModelData entityModelData) : base(entityUId, entityId, entityModelData)
        {
        }

    }
}