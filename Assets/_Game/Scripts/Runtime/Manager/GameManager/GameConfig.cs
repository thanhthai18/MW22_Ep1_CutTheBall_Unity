using Runtime.Common.Singleton;

namespace Runtime.Manager.Game
{
    public class GameConfig : PersistentMonoSingleton<GameConfig>
    {
        #region Members

        public string appsFlyerDevKey;
        public string appsFlyerMonetizationKey;
        public string appLovinKey;
        public string appLovinAdUnitAndroid;
        public string appLovinAdUnitIOS;
        public string googleWebClientId;
        public string appleAppId;
        public string adjustToken;
        public string facebookAppId;

        #endregion Members
    }
}