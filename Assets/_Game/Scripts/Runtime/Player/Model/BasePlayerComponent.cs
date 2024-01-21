using System;
using Newtonsoft.Json;

namespace Runtime.PlayerManager
{
    [Serializable]
    public abstract class BasePlayerComponent
    {
        [JsonProperty("90")]
        public string PlayerId { get; set; }
        
        protected BasePlayerComponent()
        {
        }

        public virtual void ResetDaily(DateTime time)
        {
        }

        public virtual void ResetWeekly(DateTime time)
        {
        }

        public virtual void InitNewPlayer()
        {
        }

        public virtual void PublishChangeData()
        {
            
        }
    }
}