using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BitFlyerDotNet.LightningApi
{
    internal static class EnumUtil
    {
        internal static string ToEnumString<TEnum>(this TEnum type) where TEnum : struct
        {
            var enumType = typeof(TEnum);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }
    }
}