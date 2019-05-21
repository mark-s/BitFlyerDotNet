using System.Runtime.Serialization;

namespace BitFlyerDotNet.LightningApi.DTOs
{
    public enum BoardState
    {
        [EnumMember(Value = "")]
        Unknown,

        [EnumMember(Value = "RUNNING")]
        Running,

        [EnumMember(Value = "CLOSED")]
        Closed,

        [EnumMember(Value = "STARTING")]
        Starting,

        [EnumMember(Value = "PREOPEN")]
        Preopen,

        [EnumMember(Value = "CIRCUIT BREAK")]
        CircuitBreak,

        [EnumMember(Value = "AWAITING SQ")]
        AwaitingSQ,

        [EnumMember(Value = "MATURED")]
        Matured,
    }
}
