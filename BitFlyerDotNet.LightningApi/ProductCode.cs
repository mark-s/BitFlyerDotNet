using System.Runtime.Serialization;

namespace BitFlyerDotNet.LightningApi
{
    public enum ProductCode
    {
        [EnumMember(Value = "")]
        Unknown,

        [EnumMember(Value = "BTC_JPY")]
        BTCJPY,

        [EnumMember(Value = "FX_BTC_JPY")]
        FXBTCJPY,

        [EnumMember(Value = "ETH_BTC")]
        ETHBTC,

        [EnumMember(Value = "BCH_BTC")]
        BCHBTC,

        [EnumMember(Value = "BTC_USD")]
        BTCUSD,

        [EnumMember(Value = "BTC_EUR")]
        BTCEUR,

        [EnumMember(Value = "BTCJPY_MAT1WK")]
        BTCJPYMAT1WK,

        [EnumMember(Value = "BTCJPY_MAT2WK")]
        BTCJPYMAT2WK,

        [EnumMember(Value = "BTCJPY_MAT3M")]
        BTCJPYMAT3M,
    }
}