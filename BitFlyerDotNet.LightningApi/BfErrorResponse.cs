using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi
{
    public class BfErrorResponse
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; private set; }

        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; private set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; private set; }

        public static readonly BfErrorResponse Default = default(BfErrorResponse);
    }
}