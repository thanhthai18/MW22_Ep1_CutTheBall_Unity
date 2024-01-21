namespace Runtime.Manager.Data
{
    public static class DataManager
    {
        #region Members

        private static TransitionedData s_transitionedData = new TransitionedData();

        #endregion Members

        #region Properties

        public static TransitionedData Transitioned => s_transitionedData;
        public static ConfigDataManager Config => ConfigDataManager.Instance;
        public static LocalDataManager Local => LocalDataManager.Instance;

        #endregion Properties
    }
}