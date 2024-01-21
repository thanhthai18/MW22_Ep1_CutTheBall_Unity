using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Runtime.UI;

namespace Runtime.PlayerManager
{
    public sealed class PlayerBasicInfo : BasePlayerComponent
    {

        [JsonProperty("0")]
        public int playerLevel { get; set; }

        [JsonProperty("1")]
        public Dictionary<int, long> resourcesDictionary { get; set; } = new();

        [JsonProperty("2")]
        public int RatUsState { get; set; }
        
        public PlayerBasicInfo()
        {
        }

        public override void InitNewPlayer()
        {
            base.InitNewPlayer();
            resourcesDictionary = new();
            playerLevel = 1;
        }

        public override void ResetDaily(DateTime time)
        {
            base.ResetDaily(time);
        }
    }
}