using Cysharp.Threading.Tasks;
using Runtime.Common.Singleton;
using Runtime.Definition;
using Runtime.Message;
using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Sync
{
    public class SaveLoadListenerService : PersistentMonoSingleton<SaveLoadListenerService>
    {
        public async UniTask InitAsync()
        {
            Player player = Singleton.Of<PlayerService>().Player;

            Debug.Log($"[REMOTE] listen for changes in ID={player.Auth.PlayerId}");

            // Create a listener for a specific document

            await UniTask.Yield();
        }
    }
}