using Runtime.Common.Singleton;
using Runtime.PlayerManager;
using Cysharp.Threading.Tasks;

namespace Runtime.Manager.Data
{
    public partial class LocalDataManager : PersistentMonoSingleton<LocalDataManager>
    {
        #region Class Methods

        private void Save()
            => Singleton.Of<PlayerService>().Save().Forget();

        #endregion Class Methods
    }
}