using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public static class EntityFactory
    {
        public static IEntityStrategy GetEnityStrategy(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Ball:
                    return new BallStrategy();
                
                case EntityType.Boom:
                    return new BoomStrategy();
            }

            return null;
        }
    }
}