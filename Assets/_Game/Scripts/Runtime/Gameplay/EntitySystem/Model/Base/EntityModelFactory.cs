using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public static class EntityModelFactory
    {
        public static EntityModel GetEntityModel(EntityType entityType, uint entityUId, string entityId, EntityModelData entityModelData)
        {
            switch (entityType)
            {
                case EntityType.Ball:
                    return new BallModel(entityUId, entityId, entityModelData);
                
                case EntityType.Boom:
                    return new BoomModel(entityUId, entityId, entityModelData);
            }

            return null;
        }
    }
}