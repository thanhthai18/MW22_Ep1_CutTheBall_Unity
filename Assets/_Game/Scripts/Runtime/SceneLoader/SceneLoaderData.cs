using System;
using System.Collections.Generic;

namespace Runtime.SceneLoading
{
    [Serializable]
    public struct SceneLoaderData
    {
        #region Members

        public List<SceneLoadingInfo> scenesLoadingInfo;

        #endregion Members
    }
}