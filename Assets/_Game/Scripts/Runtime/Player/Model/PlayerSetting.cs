using Newtonsoft.Json;

namespace Runtime.PlayerManager
{
    public sealed class PlayerSetting : BasePlayerComponent
    {
        [JsonProperty("0")]
        public bool hasEnableMusic { get; set; }
        
        [JsonProperty("1")]
        public bool hasEnableSound { get; set; }
        
        [JsonProperty("2")]
        public string selectedLanguage { get; set; }

        public PlayerSetting()
        {
            selectedLanguage = "";
            hasEnableMusic = true;
            hasEnableSound = true;
        }
    }
}