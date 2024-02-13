using Newtonsoft.Json;

namespace Runtime.Tool.JsonConverter
{
    public sealed class JsonService
    {
        private readonly JsonSerializerSettings _settings;

        public JsonService()
        {
            this._settings = new JsonSerializerSettings();

            this._settings.Converters.Add(new Vector2IntConverter());

            this._settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            this._settings.DefaultValueHandling = DefaultValueHandling.Include;
            this._settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, this._settings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, this._settings);
        }
    }
}