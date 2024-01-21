using UnityEngine;

namespace Runtime.Config
{
    public class BaseConfig<T> : ScriptableObject
    {
        #region Members

        public T[] items;

        #endregion Members
    }
}