using System;
using CsvReader;

namespace Runtime.Config
{
    [Serializable]
    public struct EntityDataConfigItem
    {
        #region Members

        public float jumpDuration;
        public float jumpPower;

        #endregion Members
    }
    
    public class EntityDataConfig : BaseConfig<EntityDataConfigItem>
    {
        
    }
}