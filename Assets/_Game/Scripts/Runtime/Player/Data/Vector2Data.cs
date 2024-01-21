using Newtonsoft.Json;
using Runtime.PlayerManager;
using UnityEngine;

namespace _Game.Scripts.Runtime.Player.Data
{
    public class Vector2Data
    {
        [JsonProperty("0")]
        public float X { get; set; }
        
        [JsonProperty("1")]
        public float Y { get; set; }

        public Vector2Data()
        {
            X = -1;
            Y = -1;
        }
        
        public Vector2Data(Vector2 vector2)
        {
            X = vector2.x;
            Y = vector2.y;
        }
        
        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
        
        public void FromVector2(Vector2 vector2)
        {
            X = vector2.x;
            Y = vector2.y;
        }
    }
}