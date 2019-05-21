using Newtonsoft.Json.Linq;

namespace BitFlyerDotNet.LightningApi.Realtime
{
    internal interface IRealtimeSource
    {
        string Channel { get; }
        void OnSubscribe(JToken token);
        void Subscribe();
    }
}
