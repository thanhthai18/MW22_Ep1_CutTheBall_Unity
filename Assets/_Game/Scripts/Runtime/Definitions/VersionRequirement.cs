using Newtonsoft.Json;

namespace Runtime.Definition
{
    public struct VersionRequirement
    {
        [JsonProperty("0")] public string version;
    }
}