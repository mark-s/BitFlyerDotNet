using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BfMarket
    {
        [JsonProperty(PropertyName = "product_code")]
        public string ProductCode { get; private set; }

        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; private set; }
    }
}
