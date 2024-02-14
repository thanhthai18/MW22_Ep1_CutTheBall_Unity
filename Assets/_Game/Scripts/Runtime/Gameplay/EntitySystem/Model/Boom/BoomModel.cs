using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public class BoomModel : EntityModel
    {
        #region Properties

        public override EntityType EntityType => EntityType.Boom;

        #endregion Properties

        public BoomModel(uint entityUId, string entityId, EntityModelData entityModelData) : base(entityUId, entityId, entityModelData)
        {
        }
    }
}