using System;
using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BfChat
    {
        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; private set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; private set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; private set; }
    }
}