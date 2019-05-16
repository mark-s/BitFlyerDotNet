using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BfBoardStateResult
    {
        [JsonProperty(PropertyName = "health")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BfBoardHealth Health { get; private set; }

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BfBoardState State { get; private set; }
    }
}