using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public static class EntityModelExtensions
    {
        #region Class Methods

        public static bool IsBall(this EntityType entityType)
            => entityType == EntityType.Ball;

        public static bool IsBoom(this EntityType entityType)
            => entityType == EntityType.Boom;


        #endregion Class Methods
    }
}