using Runtime.Config;

namespace Runtime.Gameplay.EntitySystem
{
    public class EntityModelData
    {
        #region Members

        public EntityDataConfigItem configItem;
        
        #endregion Members
        
        #region Class Methods
        
        public EntityModelData(EntityDataConfigItem configItem)
            => this.configItem = configItem;

        #endregion Class Methods
    }
}