using System.Runtime.Serialization;

namespace BitFlyerDotNet.LightningApi.DTOs
{
    public enum BoardHealth
    {
        [EnumMember(Value = "")]
        Unknown,
        [EnumMember(Value = "NORMAL")]
        Normal,
        [EnumMember(Value = "BUSY")]
        Busy,
        [EnumMember(Value = "VERY BUSY")]
        VeryBusy,
        [EnumMember(Value = "SUPER BUSY")]
        SuperBusy,
        [EnumMember(Value = "NO ORDER")]
        NoOrder,
        [EnumMember(Value = "STOP")]
        Stop,
    }
}
