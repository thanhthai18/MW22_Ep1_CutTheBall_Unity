using System;
using Newtonsoft.Json;

namespace Runtime.PlayerManager
{
    [Serializable]
    public abstract class PlayerBase
    {
        [JsonProperty("90")]
        public string PlayerId { get; set; }
        
        protected PlayerBase() { }
        public virtual void ResetDaily(DateTime time) { }
        public virtual void ResetWeekly(DateTime time) { }
        public virtual void InitNewPlayer() { }
        public virtual void PublishChangeData() { }
    }
}