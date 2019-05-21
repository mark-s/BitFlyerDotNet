using BitFlyerDotNet.LightningApi.DTOs;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BfBoardStateResult
    {
        [JsonProperty(PropertyName = "health")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BoardHealth Health { get; private set; }

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BoardState State { get; private set; }
    }
}
