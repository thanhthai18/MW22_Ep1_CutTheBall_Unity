using Runtime.PlayerManager;
using UnityEngine;

namespace Runtime.Sync
{
    public sealed class PlayerLoadResult
    {
        public SingleLoadResult<PlayerAuthentication> AuthResult { get; set; }

        public bool IsNewAccount()
        {
            if (this.AuthResult.SnapshotFromFirestore == null && this.AuthResult.SnapshotFromDevice == null)
            {
                return true;
            }

            return false;
        }

        public bool IsBan()
        {
            if (AuthResult.SnapshotFromFirestore == null)
            {
                return false;
            }
            return false;
        }

        public bool IsConflicted()
        {
            if (this.AuthResult.IsConflicted())
            {
                Debug.LogError("[DATA] Auth conflicted");
                return true;
            }

            return false;
        }

        public Player ToPlayer()
        {
            return new Player {
                // Auth = this.AuthResult.GetSnapshot(),
            };
        }

        public Player ToPlayer(bool isSelectDevice)
        {
            return new Player {
                // Auth = this.AuthResult.GetSnapshot(isSelectDevice),
            };
        }
    }
}