using Cysharp.Threading.Tasks;

namespace Runtime.UI
{
    public interface IAwaitableModal
    {
        #region Interface Methods

        UniTask WaitAsync();

        #endregion Interface Methods
    }
}