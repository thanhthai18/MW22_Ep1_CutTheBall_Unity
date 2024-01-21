using System;

namespace Runtime.Definition
{
    public enum NavigationTargetType
    {
        None = 0,
        Shop = 1,
        Quest = 2,
    }

    [Serializable]
    public struct NavigationData
    {
        #region Members

        public NavigationTargetType navigationTargetType;
        public int targetId;

        #endregion Members

        #region Struct Methods

        public NavigationData(NavigationTargetType navigationType, int targetId)
        {
            this.navigationTargetType = navigationType;
            this.targetId = targetId;
        }

        #endregion Struct Methods
    }
}