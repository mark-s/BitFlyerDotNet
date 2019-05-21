using System.Runtime.Serialization;

namespace BitFlyerDotNet.LightningApi.DTOs
{
    public enum TradeSide
    {
        [EnumMember(Value = "")]
        Unknown,
        [EnumMember(Value = "BUY")]
        Buy,
        [EnumMember(Value = "SELL")]
        Sell,
        [EnumMember(Value = "BUYSELL")]
        BuySell,
    }
}
