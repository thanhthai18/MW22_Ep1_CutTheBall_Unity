using System.Collections.Generic;

namespace _Game.Scripts.Runtime.PlayerData
{
    public interface IFirestoreDeserialize
    {
        void Deserialize(Dictionary<string, object> data);
    }
}