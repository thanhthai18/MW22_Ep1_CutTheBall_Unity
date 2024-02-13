using System;
using Newtonsoft.Json;

namespace Runtime.PlayerManager
{
    public sealed class PlayerBasicInfo : PlayerBase
    {
        [JsonProperty("0")]
        public int PlayerSkinId { get; set; }

        [JsonProperty("1")]
        public long LastScore { get; set; }
        
        [JsonProperty("2")]
        public long HighScore { get; set; }

        
        public PlayerBasicInfo() { }

        public override void InitNewPlayer()
        {
            base.InitNewPlayer();
            PlayerSkinId = 0;
            LastScore = 0;
            HighScore = 0;
        }

        public override void ResetDaily(DateTime time)
        {
            base.ResetDaily(time);
        }
        //
        // /// <summary>
        // /// Note: This is for saving data in the firestore, don't delete it!
        // /// </summary>
        // [JsonIgnore]
        // [FirestoreProperty(BasicTags.MONEY_TAG)]
        // private Dictionary<string, object> moneyInFirestore
        // {
        //     get => FirestoreUtils.ConvertToDictionary(resourcesDictionary);
        //     set
        //     {
        //         this.resourcesDictionary = new();
        //         foreach (KeyValuePair<string, object> entry in value)
        //         {
        //             int key = int.Parse(entry.Key);
        //             resourcesDictionary.Add(key, value.ToLong(entry.Key));
        //         }
        //     }
        // }
    }
}