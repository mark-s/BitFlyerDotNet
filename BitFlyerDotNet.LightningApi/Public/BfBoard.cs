using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BfBoard
    {
        [JsonProperty(PropertyName = "mid_price")]
        public decimal MidPrice { get; private set; }

        [JsonProperty(PropertyName = "bids")]
        public BfBoardOrder[] Bids { get; private set; }

        [JsonProperty(PropertyName = "asks")]
        public BfBoardOrder[] Asks { get; private set; }
    }
}
