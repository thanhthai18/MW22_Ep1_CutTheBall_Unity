using Newtonsoft.Json;

namespace Runtime.PlayerManager
{
    public sealed class PlayerSetting : PlayerBase
    {
        [JsonProperty("0")]
        public bool hasEnableMusic { get; set; } = true;

        [JsonProperty("1")]
        public bool hasEnableSound { get; set; } = true;

        [JsonProperty("2")]
        public string selectedLanguage { get; set; } = "";

        public PlayerSetting() { }
    }
}