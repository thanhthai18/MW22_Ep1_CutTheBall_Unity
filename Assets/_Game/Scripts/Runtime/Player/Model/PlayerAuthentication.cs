using System;
using Newtonsoft.Json;
using Runtime.Manager.Time;
using Runtime.Common.Singleton;
using Runtime.Definition;

namespace Runtime.PlayerManager
{
    public sealed class PlayerAuthentication : BasePlayerComponent
    {
        [JsonProperty("0")]
        public string PlayerName { get; set; }

        [JsonProperty("1")]
        public string DeviceName { get; set; }

        [JsonProperty("2")]
        public string DeviceId { get; set; }

        [JsonProperty("3")]
        public string DeviceModel { get; set; }

        [JsonProperty("4")]
        public string DeviceOS { get; set; }

        [JsonProperty("5")]
        public int State { get; set; }

        [JsonProperty("11")]
        public DateTime LoginTime { get; set; } = Constant.JAN1St1970;

        [JsonProperty("24")]
        public DateTime WeekTime { get; set; } = Constant.JAN1St1970;

        [JsonProperty("20")]
        public string SenderAddress { get; set; }

        [JsonProperty("21")]
        public string ContinentName { get; set; }

        [JsonProperty("22")]
        public string CountryName { get; set; }

        [JsonProperty("23")]
        public string CityName { get; set; }

        [JsonProperty("89")]
        public int LoginMethod { get; set; }

        public PlayerAuthentication()
        {
            LoginTime = Constant.JAN1St1970;
            WeekTime = Constant.JAN1St1970;
        }

        public override void ResetDaily(DateTime time)
        {
            base.ResetDaily(time);
            this.LoginTime = time;
        }

        public override void ResetWeekly(DateTime time)
        {
            base.ResetWeekly(time);
            this.WeekTime = time;
        }
    }
}