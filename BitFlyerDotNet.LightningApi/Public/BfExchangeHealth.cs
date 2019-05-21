using BitFlyerDotNet.LightningApi.DTOs;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BfExchangeHealth
    {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BoardHealth Status { get; private set; }
    }
}
