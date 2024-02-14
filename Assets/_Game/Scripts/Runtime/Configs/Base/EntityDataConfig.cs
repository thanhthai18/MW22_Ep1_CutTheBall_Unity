using System;
using UnityEngine;

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
    
    public class EntityDataConfig : ScriptableObject
    {
        public EntityDataConfigItem[] ballDataConfig;
        public EntityDataConfigItem[] boomDataConfig;
    }
}